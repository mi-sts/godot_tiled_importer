using Godot;
using System;

public struct TileData 
{
    public static TileData EMPTY = new TileData(0, Vector2.Zero, false, false, false);
    public uint GID;
    public Vector2 position;
    public bool horizontallyFlipped;
    public bool verticallyFlipped;
    // In hexagonal maps it indicates whether the tile is rotated 60 degrees clockwise.
    public bool diagonallyFlipped;
    // Only for hexagonal tiles;
    public bool rotated120;

    public TileData(
        uint GID, Vector2 position, bool horizontallyFlipped, bool verticallyFlipped, bool diagonallyFlipped
        ){
        this.GID = GID;
        this.position = position;
        this.horizontallyFlipped = horizontallyFlipped;
        this.verticallyFlipped = verticallyFlipped;
        this.diagonallyFlipped = diagonallyFlipped;
        rotated120 = false;
    }

    // Hexagonal tile constructor.
    public TileData(
        uint GID, Vector2 position, bool horizontallyFlipped, bool verticalFlipped, bool rotated60, bool rotated120
        ) {
        this.GID = GID;
        this.position = position;
        this.horizontallyFlipped = horizontallyFlipped;
        this.verticallyFlipped = verticalFlipped;
        this.diagonallyFlipped = rotated60;
        this.rotated120 = rotated120;
    }
}

