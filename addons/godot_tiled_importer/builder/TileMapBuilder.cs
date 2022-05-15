using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TiledImporter.MapBuilder
{
    public class TileMapBuilder
    {
        private List<int> atlasFirstTileGIDs = new List<int>();
        private List<int> singleTileGIDs = new List<int>();

        private HashSet<int> atlasFirstTileGIDsSet = new HashSet<int>();
        private HashSet<int> singleTileGIDsSet = new HashSet<int>();

        private Dictionary<int, int> atlasesWidth = new Dictionary<int, int>(); // key - atlas first GID, value - number of columns in the atlas. 

        private string mapFileDirectoryPath = "";

        public PackedScene GenerateTileMapScene(string sceneName, Structures.Map mapData, string mapFileDirectoryPath)
        {
            if (mapData == null)
            {
                GD.PushError("Data of the generating map is null!");
                return null;
            }
            this.mapFileDirectoryPath = mapFileDirectoryPath;
            var rootNode = new Godot.Node2D();
            rootNode.Name = sceneName;

            var packedScene = new PackedScene();
            var mapTileSet = CreateMapTileSet(mapData.tileSets, mapData.mapOrientation);
            Godot.Node2D layerParentNode = null;

            foreach (Structures.Layer layerData in mapData.layers)
            {
                if (layerData is Structures.TileLayer tileLayerData)
                {
                    layerParentNode = BuildTileLayer(tileLayerData, mapData, mapTileSet, rootNode);
                }
                else if (layerData is Structures.ObjectGroupLayer objectLayerData)
                {
                    layerParentNode = BuildObjectGroupLayer(objectLayerData, mapTileSet, rootNode);
                }
                layerParentNode.Modulate = new Color(layerData.tintColor ?? new Color(1, 1, 1), (float)layerData.opacity);
                layerParentNode.Name = layerData.name;
            }

            packedScene.Pack(rootNode);
            return packedScene;
        }

        private Godot.Node2D BuildTileLayer(
            Structures.TileLayer tileLayerData,
            Structures.Map mapData,
            Godot.TileSet mapTileSet,
            Godot.Node2D rootNode
            )
        {
            switch (mapData.mapOrientation)
            {
                case Structures.MapOrientation.Hexagonal:
                    var tileLayerParentNode = new Godot.Node2D();
                    rootNode.AddChild(tileLayerParentNode);
                    tileLayerParentNode.Owner = rootNode;

                    if (tileLayerData.tileLayerType == Structures.TileLayerType.Infinite)
                        foreach (Structures.Chunk chunk in tileLayerData.chunks)
                            DrawHexagonalChunk(chunk.data, chunk.position, tileLayerParentNode, rootNode, mapTileSet, mapData);
                    else
                    {
                        DrawHexagonalChunk(
                            tileLayerData.data,
                            Structures.IntPoint.Zero,
                            tileLayerParentNode,
                            rootNode,
                            mapTileSet,
                            mapData
                        );
                    }

                    return tileLayerParentNode;
                default:
                    var layerMapNode = new Godot.TileMap();
                    layerMapNode.Mode = ConvertMapOrientationToMapMode(mapData.mapOrientation);
                    layerMapNode.CellSize = new Vector2(mapData.tileWidth, mapData.tileHeight);
                    layerMapNode.TileSet = mapTileSet;
                    rootNode.AddChild(layerMapNode);
                    layerMapNode.Owner = rootNode;

                    if (tileLayerData.tileLayerType == Structures.TileLayerType.Infinite)
                        foreach (Structures.Chunk chunk in tileLayerData.chunks)
                        {
                            DrawChunk(chunk.data, chunk.position, layerMapNode, mapData.mapOrientation);
                        }
                    else
                        DrawChunk(tileLayerData.data, Structures.IntPoint.Zero, layerMapNode, mapData.mapOrientation);

                    return layerMapNode;
            }
        }

        private Godot.Node2D BuildObjectGroupLayer(
            Structures.ObjectGroupLayer objectLayerData,
            Godot.TileSet mapTileSet,
            Godot.Node2D rootNode
        )
        {
            var objectLayerParentNode = new Godot.Node2D();
            rootNode.AddChild(objectLayerParentNode);
            objectLayerParentNode.Owner = rootNode;
            Structures.Object[] orderedObjects =
                GetObjectsOrderedByDrawOrder(objectLayerData.objects, objectLayerData.drawOrder);
            foreach (Structures.Object objectData in orderedObjects)
            {
                if (objectData is Structures.TileObject tileObjectData)
                {
                    DrawSpriteTile(
                        tileObjectData.objectTileData.gID,
                        new Vector2(
                            (float)tileObjectData.coordinates.x,
                            (float)tileObjectData.coordinates.y
                        ),
                        (float)tileObjectData.rotation,
                        tileObjectData.objectTileData.horizontallyFlipped,
                        tileObjectData.objectTileData.verticallyFlipped,
                        mapTileSet,
                        objectLayerParentNode,
                        rootNode,
                        true,
                        new Vector2((float)tileObjectData.width, (float)tileObjectData.height)
                    );
                }
            }

            return objectLayerParentNode;
        }

        private Structures.Object[] GetObjectsOrderedByDrawOrder(Structures.Object[] objects, Structures.DrawOrder drawOrder)
        {
            switch (drawOrder)
            {
                case Structures.DrawOrder.TopDown:
                    return objects.OrderBy(objectData => objectData.coordinates.y).ToArray();
                case Structures.DrawOrder.Index:
                    return objects;
                default:
                    GD.PushError("Not determined draw order!");
                    return null;
            }
        }
        private Godot.TileMap.ModeEnum ConvertMapOrientationToMapMode(Structures.MapOrientation orientation)
        {
            switch (orientation)
            {
                case Structures.MapOrientation.Orthogonal:
                    return Godot.TileMap.ModeEnum.Square;
                case Structures.MapOrientation.Staggered:
                    return Godot.TileMap.ModeEnum.Isometric;
                case Structures.MapOrientation.Isometric:
                    return Godot.TileMap.ModeEnum.Isometric;
                case Structures.MapOrientation.Hexagonal:
                    return Godot.TileMap.ModeEnum.Square;
                default:
                    GD.PushError("Not determined orientation of the tile map!");
                    return Godot.TileMap.ModeEnum.Square;
            }
        }

        private Godot.TileSet CreateMapTileSet(
            Structures.TileSet[] tileSetsData,
            Structures.MapOrientation mapOrientation
            )
        {
            var tileSet = new Godot.TileSet();
            foreach (Structures.TileSet tileSetData in tileSetsData)
            {
                AddTilesToTileSetNode(tileSet, tileSetData, mapOrientation);
            }

            return tileSet;
        }

        private void DrawChunk(
            Structures.TileLayerData chunkData,
            Structures.IntPoint chunkPosition,
            Godot.TileMap mapNode,
            Structures.MapOrientation mapOrientation
            )
        {
            foreach (Structures.TileData tileData in chunkData.tiles)
            {
                if (tileData.gID == 0)
                    continue;

                var position = new Structures.IntPoint(
                    chunkPosition.x + tileData.position.x,
                    chunkPosition.y + tileData.position.y
                );

                if (mapOrientation == Structures.MapOrientation.Staggered)
                {
                    position = StaggeredIsometricToIsometricCoordinates(position);
                }

                if (singleTileGIDsSet.Contains((int)tileData.gID))
                { // If drawing tile is a sigle tile.
                    mapNode.SetCell(
                        position.x,
                        position.y,
                        (int)tileData.gID,
                        tileData.horizontallyFlipped,
                        tileData.verticallyFlipped,
                        tileData.diagonallyFlipped,
                        new Vector2()
                    );
                }
                else
                {                                       // If drawing tile is an atlas tile.     
                    int atlasFirstGID = FindAtlasFirstGID(tileData.gID);
                    int atlasTileLocalID = (int)tileData.gID - atlasFirstGID;
                    int atlasWidth = atlasesWidth[(int)atlasFirstGID];
                    int tileXIndex = atlasTileLocalID % atlasWidth;
                    int tileYIndex = atlasTileLocalID / atlasWidth;
                    mapNode.SetCell(
                        position.x,
                        position.y,
                        (int)atlasFirstGID,
                        tileData.horizontallyFlipped,
                        tileData.verticallyFlipped,
                        tileData.diagonallyFlipped,
                        new Vector2(tileXIndex, tileYIndex)
                    );
                }
            }
        }

        private void DrawHexagonalChunk(
            Structures.TileLayerData chunkData,
            Structures.IntPoint chunkPosition,
            Godot.Node2D chunkParentNode,
            Godot.Node2D rootNode,
            Godot.TileSet mapTileSet,
            Structures.Map mapData
        )
        {
            int staggerRemainder = mapData.staggerIndex == Structures.StaggerIndex.Odd ? 1 : 0;
            foreach (Structures.TileData tileData in chunkData.tiles)
            {
                if (tileData.gID == 0)
                    continue;

                float spriteTileRotation = tileData.rotated120 ? 120 : 0;
                bool spriteTileHorizontallyFlipped = tileData.horizontallyFlipped;
                bool spriteTileVerticallyFlipped = tileData.verticallyFlipped;

                var axesFactorVector = Vector2.Zero;
                var staggerAxisFactor = 0.75f;
                if (mapData.staggerAxis == Structures.StaggerAxis.Y)
                    axesFactorVector = new Vector2(1f, staggerAxisFactor);
                else
                    axesFactorVector = new Vector2(staggerAxisFactor, 1f);

                var staggerOffsetVector = Vector2.Zero;
                if (mapData.staggerAxis == Structures.StaggerAxis.X && tileData.position.x % 2 == staggerRemainder)
                    staggerOffsetVector = new Vector2(0f, 0.5f);
                else if (mapData.staggerAxis == Structures.StaggerAxis.Y && tileData.position.y % 2 == staggerRemainder)
                    staggerOffsetVector = new Vector2(0.5f, 0f);

                var spriteTilePosition = new Vector2(
                    (chunkPosition.x + tileData.position.x + staggerOffsetVector.x) *
                        axesFactorVector.x * mapData.tileWidth,
                    (chunkPosition.y + tileData.position.y + staggerOffsetVector.y) *
                        axesFactorVector.y * mapData.tileHeight
                );

                DrawSpriteTile(
                    tileData.gID,
                    spriteTilePosition,
                    spriteTileRotation,
                    spriteTileHorizontallyFlipped,
                    spriteTileVerticallyFlipped,
                    mapTileSet,
                    chunkParentNode,
                    rootNode
                );
            }
        }

        private void DrawSpriteTile(
            uint tileGID,
            Vector2 position,
            float rotation,
            bool horizontallyFlipped,
            bool verticallyFlipped,
            Godot.TileSet mapTileSet,
            Godot.Node2D spriteParentNode,
            Godot.Node2D rootNode,
            bool isObjectLayerSpriteTile = false,
            Vector2? spriteSize = null
            )
        {
            var spriteTile = new Godot.Sprite();
            spriteTile.RotationDegrees = rotation;
            spriteTile.FlipH = horizontallyFlipped;
            spriteTile.FlipV = verticallyFlipped;
            spriteTile.Centered = false;

            if (singleTileGIDsSet.Contains((int)tileGID))
            { // If drawing tile is a sigle tile.
                spriteTile.Texture = mapTileSet.TileGetTexture((int)tileGID);
            }
            else
            {                                       // If drawing tile is an atlas tile.
                spriteTile.RegionEnabled = true;
                int atlasFirstGID = FindAtlasFirstGID(tileGID);
                int atlasTileLocalID = (int)tileGID - atlasFirstGID;
                int atlasWidth = atlasesWidth[(int)atlasFirstGID];
                int tileXIndex = atlasTileLocalID % atlasWidth;
                int tileYIndex = (int)(atlasTileLocalID / atlasWidth);
                Vector2 tileSize = mapTileSet.AutotileGetSize((int)atlasFirstGID);
                spriteTile.RegionRect = new Rect2(
                    tileXIndex * tileSize.x,
                    tileYIndex * tileSize.y,
                    tileSize
                );
                spriteTile.Texture = mapTileSet.TileGetTexture((int)atlasFirstGID);
            }

            if (spriteTile.Texture != null && isObjectLayerSpriteTile)
            {
                spriteTile.Offset = new Vector2(spriteTile.Offset.x, -spriteTile.Texture.GetSize().y);
                if (spriteSize != null && spriteTile.Texture != null)
                {
                    var spriteScale = new Vector2(
                        spriteSize.GetValueOrDefault().x / spriteTile.Texture.GetSize().x,
                        spriteSize.GetValueOrDefault().y / spriteTile.Texture.GetSize().y
                    );
                    spriteTile.Scale = spriteScale;
                }
            }

            spriteTile.Position = position;

            spriteParentNode.AddChild(spriteTile);
            spriteTile.Owner = rootNode;
        }


        private int FindAtlasFirstGID(uint tileGID)
        {
            int binSearchIndex = atlasFirstTileGIDs.BinarySearch((int)tileGID);
            if (binSearchIndex < 0)
                binSearchIndex = ~binSearchIndex - 1;

            if (binSearchIndex < 0 || binSearchIndex >= atlasFirstTileGIDs.Count)
                return atlasFirstTileGIDs[0];

            return atlasFirstTileGIDs[binSearchIndex];
        }

        private void AddTilesToTileSetNode(
            Godot.TileSet tileSetNode,
            Structures.TileSet tileSetData,
            Structures.MapOrientation mapOrientation
            )
        {
            switch (tileSetData.type)
            {
                case Structures.TileSetType.SingleImageTileSet:
                    AddAtlasToTileSetNode(tileSetNode, tileSetData, mapOrientation);
                    break;
                case Structures.TileSetType.MultupleImagesTileSet:
                    AddSingleTilesToTileSetNode(tileSetNode, tileSetData, mapOrientation);
                    break;
                default:
                    GD.PushError("Not determined tile set type!");
                    break;
            }
        }

        private bool IsMapIsometric(Structures.MapOrientation orientation) =>
            orientation == Structures.MapOrientation.Isometric ||
            orientation == Structures.MapOrientation.Staggered;

        private void AddSingleTilesToTileSetNode(
            Godot.TileSet tileSetNode,
            Structures.TileSet tileSetData,
            Structures.MapOrientation mapOrientation
            )
        {
            uint firstGID = tileSetData.firstGID;
            for (int i = 0; i < tileSetData.tiles.Length; ++i)
            {
                Structures.Tile tileData = tileSetData.tiles[i];
                var tilePath = $"res://{mapFileDirectoryPath}{tileData.image}";
                var tileTexture = Godot.ResourceLoader.Load(tilePath) as Godot.Texture;
                if (tileTexture == null)
                {
                    GD.PushError("Loaded tile texture is null!");
                    continue;
                }
                int tileGID = (int)(firstGID + tileData.id);
                tileSetNode.CreateTile(tileGID);
                tileSetNode.TileSetTileMode(tileGID, TileSet.TileMode.SingleTile);
                tileSetNode.TileSetTexture(tileGID, tileTexture);
                tileSetNode.TileSetRegion(tileGID, new Rect2(0f, 0f, tileTexture.GetWidth(), tileTexture.GetHeight()));
                bool isIsometric = IsMapIsometric(mapOrientation);
                tileSetNode.TileSetTextureOffset(
                    tileGID,
                    new Vector2(
                        tileSetData.tileOffset?.x ?? 0,
                        -tileSetData.tileOffset?.y ?? 0 - (isIsometric ? tileSetData.tileHeight : 0)
                        )
                );
                tileSetNode.TileSetName(tileGID, $"{tileSetData.name}_{tileData.id}");
                singleTileGIDs.Add(tileGID);
                singleTileGIDsSet.Add(tileGID);
            }
        }

        private void AddAtlasToTileSetNode(
            Godot.TileSet tileSetNode,
            Structures.TileSet tileSetData,
            Structures.MapOrientation mapOrientation
            )
        {
            int firstGID = (int)tileSetData.firstGID;
            var texturePath = $"res://{mapFileDirectoryPath}{tileSetData.image}";
            var texture = Godot.ResourceLoader.Load(texturePath) as Texture;
            if (texture == null)
            {
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
            bool isMapIsometric = IsMapIsometric(mapOrientation);
            tileSetNode.TileSetTextureOffset(
                firstGID,
                new Vector2(
                    tileSetData.tileOffset?.x ?? 0,
                    -tileSetData.tileOffset?.y ?? 0 - (isMapIsometric ? tileSetData.tileHeight : 0)
                    )
            );

            atlasFirstTileGIDs.Add(firstGID);
            atlasFirstTileGIDsSet.Add(firstGID);
            atlasesWidth.Add(firstGID, tileSetData.columns);
        }

        public static Structures.IntPoint StaggeredIsometricToIsometricCoordinates(Structures.IntPoint coordinates)
        {
            int xCoordinate = coordinates.y + coordinates.x - coordinates.y / 2;
            int yCoordinate = coordinates.y - xCoordinate;

            return new Structures.IntPoint(xCoordinate, yCoordinate);
        }
    }
}
