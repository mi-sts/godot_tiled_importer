using Godot;
using System;

namespace TiledImporter.Structures
{
    public class Terrain
    {
        public string name { get; private set; }
        public Property[] properties { get; private set; } // (optional).
        public int tileID { get; private set; }

        public Terrain(string name, int tileID, Property[] properties = null)
        {
            if (name == null)
            {
                GD.PushError("Name of the terrain is not initialized!");
            }
            this.name = name ?? "";
            this.tileID = tileID;
            this.properties = properties;
        }
    }
}
