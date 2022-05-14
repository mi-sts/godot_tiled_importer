using Godot;
using System;
using System.Linq;

namespace TiledImporter.Structures
{
    public struct TileInfo
    {
        public int? id;
        public int? imageWidth;
        public int? imageHeight;
        public string type;
        public Property[] properties;
        public string image;
        public Layer objectGroup;
        public double? probability;
        public int[] terrainIndex;
        public Frame[] animation;
        public Grid? grid;
    }

    public struct Tile
    {
        public int id { get; private set; }
        public int? imageWidth { get; private set; } // (optional).
        public int? imageHeight { get; private set; } // (optional).
        public string type { get; private set; } // The type of the tile (optional).
        public Property[] properties { get; private set; }
        public string image { get; private set; } // Image representing this tile (optional).
        public Layer objectGroup { get; private set; } // Layer with type objectgroup (optional).
        public double? probability { get; private set; } // Percentage chance this tile is chosen when competing with others in the editor (optional).
        public int[] terrainIndex { get; private set; } // Index of terrain for each corner of tile (optional).
        public Frame[] animation { get; private set; } // Array of Frames (optional).
        public Grid? grid { get; private set; } // (optional).

        public Tile(TileInfo tileInfo)
        {
            if (tileInfo.id == null)
            {
                GD.PushError("Tile id is not initialized!");
            }

            id = tileInfo.id ?? -1;
            imageWidth = tileInfo.imageWidth;
            imageHeight = tileInfo.imageHeight;
            properties = tileInfo.properties ?? new Property[0];
            type = tileInfo.type;
            image = tileInfo.image;
            objectGroup = tileInfo.objectGroup;
            probability = tileInfo.probability;
            terrainIndex = tileInfo.terrainIndex;
            animation = tileInfo.animation;
            grid = tileInfo.grid;
        }
    }
}
