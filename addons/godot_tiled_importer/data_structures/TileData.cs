using Godot;
using System;

namespace TiledImporter.Structures
{
    // Сontains information about the tile that was parsed from the data field of the layer.
    public struct TileData
    {
        public static TileData EMPTY = new TileData(0, IntPoint.Zero, false, false, false);
        public uint gID { get; private set; }
        public IntPoint position { get; private set; }
        public bool horizontallyFlipped { get; private set; }
        public bool verticallyFlipped { get; private set; }
        // In hexagonal maps it indicates whether the tile is rotated 60 degrees clockwise.
        public bool diagonallyFlipped { get; private set; }
        // Only for hexagonal tiles;
        public bool rotated120 { get; private set; }

        public TileData(
            uint GID, IntPoint position, bool horizontallyFlipped, bool verticallyFlipped, bool diagonallyFlipped
            )
        {
            this.gID = GID;
            this.position = position;
            this.horizontallyFlipped = horizontallyFlipped;
            this.verticallyFlipped = verticallyFlipped;
            this.diagonallyFlipped = diagonallyFlipped;
            rotated120 = false;
        }

        // Hexagonal tile constructor.
        public TileData(
            uint GID, IntPoint position, bool horizontallyFlipped, bool verticallyFlipped, bool rotated60, bool rotated120
            )
        {
            this.gID = GID;
            this.position = position;
            this.horizontallyFlipped = horizontallyFlipped;
            this.verticallyFlipped = verticallyFlipped;
            this.diagonallyFlipped = rotated60;
            this.rotated120 = rotated120;
        }
    }
}
