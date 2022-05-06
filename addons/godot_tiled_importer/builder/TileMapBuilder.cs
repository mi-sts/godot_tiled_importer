using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TiledImporter.MapBuilder {
public class TileMapBuilder
{
    private List<int> atlasFirstGIDs = new List<int>();
    private Dictionary<int, int> atlasesWidth = new Dictionary<int, int>(); // key - atlas first GID, value - number of columns in the atlas. 

    public void GenerateTileMap(Structures.Map mapData)
    {
        if (mapData == null) {
            GD.PushError("Data of the generating map is null!");
            return;
        }

        var rootNode = new Godot.Node2D();
        var packedScene = new PackedScene();
        var mapTileSet = CreateMapTileSet(mapData.tileSets);

        foreach (Structures.Layer layerData in mapData.layers) {
            if (layerData is Structures.TileLayer) {    
                var mapNode = new Godot.TileMap();
                mapNode.Mode = ConvertMapOrientationToMapMode(mapData.mapOrientation);
                mapNode.CellSize = new Vector2(mapData.tileWidth, mapData.tileHeight);
                mapNode.TileSet = mapTileSet;
                var tileLayerData = layerData as Structures.TileLayer;
                switch (tileLayerData.tileLayerType) {
                    case Structures.TileLayerType.Infinite:
                        foreach (Structures.Chunk chunk in tileLayerData.chunks) {
                            DrawChunk(mapNode, chunk.data, chunk.position);
                        }
                        break;
                    case Structures.TileLayerType.NotInfinite:
                        DrawChunk(mapNode, tileLayerData.data, Structures.IntPoint.Zero);
                        break;
                }                
                rootNode.AddChild(mapNode);
                mapNode.Owner = rootNode;
            }
        }
        
        packedScene.Pack(rootNode);
        ResourceSaver.Save($"res://tile_map_scene_.tscn", packedScene);
    }

    private Godot.TileMap.ModeEnum ConvertMapOrientationToMapMode(Structures.MapOrientation orientation) {
        switch (orientation) {
            case Structures.MapOrientation.Orthogonal:
                return Godot.TileMap.ModeEnum.Square;
            case Structures.MapOrientation.Staggered:
                return Godot.TileMap.ModeEnum.Isometric;
            case Structures.MapOrientation.Isometric:
                return Godot.TileMap.ModeEnum.Isometric;
            case Structures.MapOrientation.Hexagonal:
                return Godot.TileMap.ModeEnum.Square;
            default:
                GD.Print("Not determined orientation of the tile map!");
                return Godot.TileMap.ModeEnum.Square;
        }
    }

    private Godot.TileSet CreateMapTileSet(Structures.TileSet[] tileSetsData) {
        var tileSet = new Godot.TileSet();
        foreach (Structures.TileSet tileSetData in tileSetsData) {
            AddTilesToTileSetNode(tileSet, tileSetData);
        }

        return tileSet;
    }

    private void DrawChunk(
        Godot.TileMap mapNode, 
        Structures.TileLayerData tileLayerData,
        Structures.IntPoint chunkPosition
        ) {
        HashSet<int> tilesGIDsSet = mapNode.TileSet.GetTilesIds().Cast<int>().ToHashSet<int>();
        HashSet<int> atlasFirstTilesGIDsSet = atlasFirstGIDs.ToHashSet<int>();
        foreach (Structures.TileData tileData in tileLayerData.tiles) {
            if (tileData.gID == 0)
                continue;

            if (tilesGIDsSet.Contains((int)tileData.gID) && !atlasFirstTilesGIDsSet.Contains((int)tileData.gID)) { // If drawing tile is a sigle tile.
                mapNode.SetCell(
                    chunkPosition.x + tileData.position.x, 
                    chunkPosition.y + tileData.position.y,
                    (int)tileData.gID,
                    tileData.horizontallyFlipped,
                    tileData.verticallyFlipped,
                    tileData.diagonallyFlipped,
                    new Vector2()
                );
            } else {                                       // If drawing tile is an atlas tile.     
                int atlasFirstGID = FindAtlasFirstGID((int)tileData.gID);
                int atlasTileLocalID = (int)tileData.gID - atlasFirstGID;
                int atlasWidth = atlasesWidth[atlasFirstGID];
                mapNode.SetCell(
                    chunkPosition.x + tileData.position.x, 
                    chunkPosition.y + tileData.position.y,
                    atlasFirstGID,
                    tileData.horizontallyFlipped,
                    tileData.verticallyFlipped,
                    tileData.diagonallyFlipped,
                    new Vector2(
                        atlasTileLocalID % atlasWidth, 
                        (int)(atlasTileLocalID / atlasWidth)
                    )
                );
            }
        }
    }

    private int FindAtlasFirstGID(int tileGID) {
        int binSearchIndex = atlasFirstGIDs.BinarySearch(tileGID);
        if (binSearchIndex < 0) 
            binSearchIndex = ~binSearchIndex - 1;

        if (binSearchIndex < 0 || binSearchIndex >= atlasFirstGIDs.Count)
            return atlasFirstGIDs[0];

        return atlasFirstGIDs[binSearchIndex];
    }

    private void AddTilesToTileSetNode(
        Godot.TileSet tileSetNode, 
        Structures.TileSet tileSetData
        )
    {        
        switch (tileSetData.type) {
            case Structures.TileSetType.SingleImageTileSet:
                AddAtlasToTileSetNode(tileSetNode, tileSetData);
                break;
            case Structures.TileSetType.MultupleImagesTileSet:
                AddSingleTilesToTileSetNode(tileSetNode, tileSetData);
                break;
            default:
                GD.PushError("Not determined tile set type!");
                break;
        }
    }

    private void AddSingleTilesToTileSetNode(
        Godot.TileSet tileSetNode,
        Structures.TileSet tileSetData        
        ) {
        int firstGID = tileSetData.firstGID;
        for (int i = 0; i < tileSetData.tiles.Length; ++i) {
            var tileData = tileSetData.tiles[i];
            var tilePath = $"res://{tileData.image}";
            var tileTexture = Godot.ResourceLoader.Load(tilePath) as Godot.Texture;
            if (tileTexture == null) {
                GD.PushError("Loaded tile texture is null!");
                continue;
            }
            tileSetNode.CreateTile(firstGID + tileData.id);
            tileSetNode.TileSetTileMode(firstGID + tileData.id, TileSet.TileMode.SingleTile);
            tileSetNode.TileSetTexture(firstGID + tileData.id, tileTexture);
            tileSetNode.TileSetRegion(firstGID + tileData.id, new Rect2(0f, 0f, tileTexture.GetWidth(), tileTexture.GetHeight()));
            tileSetNode.TileSetTextureOffset(
                firstGID, 
                new Vector2(tileSetData.tileOffset?.x ?? 0, tileSetData.tileOffset?.y ?? 0)
            );
            tileSetNode.TileSetName(firstGID + tileData.id, $"{tileSetData.name}_{tileData.id}");
        }
    }

    private void AddAtlasToTileSetNode(
        Godot.TileSet tileSetNode,
        Structures.TileSet tileSetData
        ) {
        int firstGID = tileSetData.firstGID;
        var texturePath = $"res://{tileSetData.image}";
        var texture = Godot.ResourceLoader.Load(texturePath) as Texture;
        if (texture == null) {
            GD.PushError("Loaded atlas texture is null!");
            return;
        }

        tileSetNode.CreateTile(firstGID);
        tileSetNode.TileSetTileMode(firstGID, Godot.TileSet.TileMode.AtlasTile);
        tileSetNode.TileSetTexture(firstGID, texture);
        tileSetNode.TileSetRegion(
            firstGID, 
            new Rect2(
                tileSetData.spacing, 
                tileSetData.spacing, 
                tileSetData.imageWidth + tileSetData.spacing, 
                tileSetData.imageHeight + tileSetData.spacing
            )
        );
        tileSetNode.AutotileSetSize(firstGID, new Vector2(tileSetData.tileWidth, tileSetData.tileHeight));
        tileSetNode.TileSetName(firstGID, tileSetData.name);
        tileSetNode.AutotileSetSpacing(firstGID, tileSetData.spacing);

        atlasFirstGIDs.Add(firstGID);
        atlasesWidth.Add(firstGID, tileSetData.columns);
    }
}
}
