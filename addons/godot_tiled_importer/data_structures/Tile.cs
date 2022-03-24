using Godot;
using System;
using System.Linq;

public struct TileInfo {
    public int? id;
    public int? imageWidth;
    public int? imageHeight;
    public string type;
    public Property[] properties;
    public string image;
    public Layer objectGroup;
    public double probability;
    public Terrain[] terrain;
    public Frame[] animation;
}

public struct Tile 
{
    public static Tile NullTile = new Tile(new TileInfo() {
        id = -1,
        imageWidth = 0,
        imageHeight = 0,
        properties = new Property[0]
    } );
    
    public int id { get; private set; }
    public int imageWidth { get; private set; }
    public int imageHeight { get; private set; }
    public string type { get; private set; } // The type of the tile (optional).
    public Property[] properties { get; private set; }
    public string image { get; private set; } // Image representing this tile (optional).
    public Layer objectGroup { get; private set; } // Layer with type objectgroup (optional).
    public double probability { get; private set; } // Percentage chance this tile is chosen when competing with others in the editor (optional).
    public Terrain[] terrain { get; private set; } // Index of terrain for each corner of tile (optional).
    public Frame[] animation { get; private set; } // Array of Frames (optional).

    public Tile(TileInfo tileInfo) {
        var requiredParameters = new object[] { 
            tileInfo.id,
            tileInfo.imageWidth, 
            tileInfo.imageHeight, 
            tileInfo.properties 
        };
        if (requiredParameters.Any(argument => argument == null)) {
            GD.PushError("Not all of the required tile parameters are initialized!");
            this = NullTile;
            return;
        }

        id = tileInfo.id ?? -1;
        imageWidth = tileInfo.imageWidth ?? 0;
        imageHeight = tileInfo.imageHeight ?? 0;
        type = tileInfo.type;
        properties = tileInfo.properties;
        image = tileInfo.image;
        objectGroup = tileInfo.objectGroup;
        probability = tileInfo.probability;
        terrain = tileInfo.terrain;
        animation = tileInfo.animation;
    }
}

