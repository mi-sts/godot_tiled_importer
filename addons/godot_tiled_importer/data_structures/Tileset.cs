using Godot;
using System;
using System.Linq;

namespace TiledImporter.Structures
{
    public enum TileObjectsAlignment
    {
        Uncspecified, TopLeft, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight
    }

    public enum TileSetType
    {
        SingleImageTileSet, MultupleImagesTileSet
    }

    public struct TileSetInfo
    {
        public string name;
        public uint? firstGID;
        public int? tileWidth;
        public int? tileHeight;
        public Tile[] tiles;
        public string image;
        public int? imageWidth;
        public int? imageHeight;
        public string source;
        public Property[] properties;
        public int? tileCount;
        public int? columns;
        public WangSet[] wangSets;
        public Color? backgroundColor;
        public Grid? grid;
        public int? margin;
        public TileObjectsAlignment? objectsAlignment;
        public int? spacing;
        public Terrain[] terrains;
        public IntPoint? tileOffset;
        public Transfromations? transfromations;
        public Color? transparentColor;
        public TileSetType? type;
    }

    public class TileSet
    {
        public string name { get; private set; }
        public uint firstGID { get; private set; }
        public int tileWidth { get; private set; }
        public int tileHeight { get; private set; }
        public Tile[] tiles { get; private set; } // (optional).
        public string image { get; private set; }
        public int imageWidth { get; private set; }
        public int imageHeight { get; private set; }
        public string source { get; private set; } // The external file that contains this tilesets data.
        public Property[] properties { get; private set; } // (optional).
        public int tileCount { get; private set; }
        public int columns { get; private set; }
        public WangSet[] wangSets { get; private set; } // (optional).
        public Color? backgroundColor { get; private set; } // (optional).
        public Grid? grid { get; private set; } // (optional).
        public int margin { get; private set; }
        public TileObjectsAlignment objectsAlignment { get; private set; } // Alignment to use for tile objects (default: unspecified).
        public int spacing { get; private set; }
        public Terrain[] terrains { get; private set; } // (optional).
        public IntPoint? tileOffset { get; private set; } // (optional).
        public Transfromations? transfromations { get; private set; } // Allowed transformations (optional).
        public Color? transparentColor { get; private set; } // (optional).
        public TileSetType type { get; private set; }

        public TileSet(TileSetInfo tileSetInfo)
        {
            var requiredFields = new object[] {
            tileSetInfo.name,
            tileSetInfo.firstGID,
            tileSetInfo.margin,
            tileSetInfo.spacing,
            tileSetInfo.tileCount,
            tileSetInfo.tileHeight,
            tileSetInfo.tileWidth,
            tileSetInfo.type,
            tileSetInfo.columns
        };
            if (requiredFields.Any(field => field == null))
            {
                GD.PushError("Not all of the required tile set parameters are initialized!");
            }
            switch (tileSetInfo.type)
            {
                case TileSetType.MultupleImagesTileSet:
                    if (tileSetInfo.tiles == null)
                    {
                        GD.PushError("Tiles field of the multiple images tile set is null!");
                    }
                    break;
                case TileSetType.SingleImageTileSet:
                    if (tileSetInfo.image == null ||
                        tileSetInfo.imageWidth == null ||
                        tileSetInfo.imageHeight == null)
                    {
                        GD.PushError("One of the required fields of the single image tile set is null!");
                    }
                    break;
            }
            name = tileSetInfo.name;
            firstGID = tileSetInfo.firstGID ?? 0u;
            image = tileSetInfo.image;
            imageHeight = tileSetInfo.imageHeight ?? 0;
            imageWidth = tileSetInfo.imageWidth ?? 0;
            margin = tileSetInfo.margin ?? 0;
            spacing = tileSetInfo.spacing ?? 0;
            tileCount = tileSetInfo.tileCount ?? 0;
            tileHeight = tileSetInfo.tileHeight ?? 0;
            tileWidth = tileSetInfo.tileWidth ?? 0;
            type = tileSetInfo.type ?? TileSetType.SingleImageTileSet;
            columns = tileSetInfo.columns ?? 0;

            tiles = tileSetInfo.tiles;
            source = tileSetInfo.source;
            backgroundColor = tileSetInfo.backgroundColor;
            grid = tileSetInfo.grid;
            objectsAlignment = tileSetInfo.objectsAlignment ?? TileObjectsAlignment.Uncspecified;
            terrains = tileSetInfo.terrains;
            tileOffset = tileSetInfo.tileOffset;
            transfromations = tileSetInfo.transfromations;
            transparentColor = tileSetInfo.transparentColor;
            properties = tileSetInfo.properties ?? new Property[0];
        }
    }
}
