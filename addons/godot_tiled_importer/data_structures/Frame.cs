using Godot;
using System;

namespace TiledImporter.Structures
{
    public struct Frame
    {
        public int tileID { get; private set; }
        public int duration { get; private set; }

        public Frame(int tileID, int duration)
        {
            this.tileID = tileID;
            this.duration = duration;
        }
    }
}
