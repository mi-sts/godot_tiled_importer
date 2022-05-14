using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum GridOrientation
    {
        Orthogonal, Isometric
    }

    public struct Grid
    {
        public GridOrientation orientation { get; private set; } // (default: orthogonal).
        public int width { get; private set; }
        public int height { get; private set; }

        public Grid(int width, int height, GridOrientation orientation = GridOrientation.Orthogonal)
        {
            this.width = width;
            this.height = height;
            this.orientation = orientation;
        }
    }
}
