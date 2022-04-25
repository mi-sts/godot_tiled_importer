using Godot;
using System;

namespace TiledImporter.Structures
{
    public struct Transfromations
    {
        public bool hflip { get; private set; }
        public bool vflip { get; private set; }
        public bool rotate { get; private set; }
        public bool preferUntransformed { get; private set; }

        public Transfromations(bool hflip, bool vflip, bool rotate, bool preferUntransformed)
        {
            this.hflip = hflip;
            this.vflip = vflip;
            this.rotate = rotate;
            this.preferUntransformed = preferUntransformed;
        }
    }
}
