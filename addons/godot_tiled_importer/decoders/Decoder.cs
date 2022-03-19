using System;
using Godot;

public abstract class Decoder 
{
    protected const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
    protected const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
    protected const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;
    protected const uint ROTATED_HEXAGONAL_120_FLAG = 0x10000000;

    // Decodes flip flags from the tile ID number (not cleared GID).
    protected bool[] DecodeFlipFlags(uint tileID) {
        bool[] flags = new bool[4];
        flags[0] = (tileID & FLIPPED_HORIZONTALLY_FLAG) != 0; 
        flags[1] = (tileID & FLIPPED_VERTICALLY_FLAG) != 0; 
        flags[2] = (tileID & FLIPPED_DIAGONALLY_FLAG) != 0;
        flags[3] = (tileID & ROTATED_HEXAGONAL_120_FLAG) != 0;

        return flags;
    }

    // Clears flip flags bits in the tile ID number to recieve the GID number.
    protected void ClearFlipFlagsBits(ref uint tileID) {
        tileID &= ~(FLIPPED_HORIZONTALLY_FLAG |
                             FLIPPED_VERTICALLY_FLAG |
                             FLIPPED_DIAGONALLY_FLAG |
                             ROTATED_HEXAGONAL_120_FLAG);
    } 

    // Creates a LayerData object from a recieved tiles data.
    protected LayerData CreateLayerData(uint[] mapGIDs, bool[][] flipFlags, int mapWidth, int mapHeight) {
        if (mapGIDs.Length != mapHeight * mapWidth) {
            GD.PushError("Number of tiles doesn't math the size of the map!");
            return null;
        }
        if (flipFlags.Length != mapHeight * mapWidth) {
            GD.PushError("Number of tile flags doesn't math the size of the map!");
            return null;
        }

        TileData[,] tiles = new TileData[mapHeight, mapWidth];
        
        for (int y = 0; y < mapHeight; ++y) {
            for (int x = 0; x < mapWidth; ++x) {
                int tileIndex = y * mapWidth + x;

                bool[] tileFlags = flipFlags[tileIndex];
                bool horizontallyFlipped = tileFlags[0];
                bool verticallyFlipped = tileFlags[1];
                bool diagonallyFlipped = tileFlags[2];
                bool rotated120 = tileFlags[3]; // Considered only for hexagonal maps.

                tiles[y, x] = new TileData(
                    mapGIDs[tileIndex], new Vector2(x, y), horizontallyFlipped, verticallyFlipped, 
                    diagonallyFlipped, rotated120
                    );
            }
        }

        return new LayerData(tiles, mapWidth, mapHeight);
    }

    // Extrudes flip flags from encoded tile IDs data and clear flag bits.
    protected bool[][] DecodeFlipFlagsAndClear(ref uint[] tileIDs) {
        bool[][] filpFlags = new bool[tileIDs.Length][];
        for (int x = 0; x < tileIDs.Length; ++x) {
            filpFlags[x] = DecodeFlipFlags(tileIDs[x]);
            ClearFlipFlagsBits(ref tileIDs[x]);
        }

        return filpFlags;
    }

    public abstract LayerData Decode(string encodedString, int mapWidth, int mapHeidth);
}
