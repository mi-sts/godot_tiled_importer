using Godot;
using System;
using System.Linq;

public enum TileObjectsAlignment {
    Uncspecified, TopLeft, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight
}

public struct TileSetInfo {
    public string name;
    public int? firstGID;
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
}

public class TileSet {
    public string name { get; private set; }
    public int firstGID { get; private set; }
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

    public TileSet(TileSetInfo tileSetInfo) {
        var requiredFields = new object[] {
            tileSetInfo.name,
            tileSetInfo.firstGID,
            tileSetInfo.image,
            tileSetInfo.imageHeight,
            tileSetInfo.imageWidth,
            tileSetInfo.margin,
            tileSetInfo.spacing,
            tileSetInfo.tileCount,
            tileSetInfo.tileHeight,
            tileSetInfo.tileWidth
        };
        if (requiredFields.Any(field => field == null)) {
            GD.PushError("Not all of the required tile set parameters are initialized!");
        }
        name = tileSetInfo.name;
        firstGID = tileSetInfo.firstGID ?? -1;
        image = tileSetInfo.image;
        imageHeight = tileSetInfo.imageHeight ?? 0;
        imageWidth = tileSetInfo.imageWidth ?? 0;
        margin = tileSetInfo.margin ?? 0;
        spacing = tileSetInfo.spacing ?? 0;
        tileCount = tileSetInfo.tileCount ?? 0;
        tileHeight = tileSetInfo.tileHeight ?? 0;
        tileWidth = tileSetInfo.tileWidth ?? 0;

        tiles = tileSetInfo.tiles;
        source = tileSetInfo.source;
        backgroundColor = tileSetInfo.backgroundColor;
        grid = tileSetInfo.grid;
        objectsAlignment = tileSetInfo.objectsAlignment ?? TileObjectsAlignment.Uncspecified;
        terrains = tileSetInfo.terrains;
        tileOffset = tileSetInfo.tileOffset;
        transfromations = tileSetInfo.transfromations;
        transparentColor = tileSetInfo.transparentColor;
    }
}

