using Godot;
using System;

public class LayerData 
{
    private TileData[,] tiles;
    private int mapWidth = 0;
    private int mapHeight = 0;

    public LayerData(int mapWidth, int mapHeight, uint[] mapGIDs, bool[][] flipFlags) {
        if (mapWidth <= 0 || mapHeight <= 0) {
            GD.PrintErr("Incorrect size of the map!");
            return;
        }
        if (mapGIDs.Length != mapHeight * mapWidth) {
            GD.PrintErr("Count of tiles doesn't math the size of the map!");
            return;
        }
        if (flipFlags.Length != mapHeight * mapWidth) {
            GD.PrintErr("Count of tile flags doesn't math the size of the map!");
            return;
        }

        this.mapHeight = mapHeight;
        this.mapWidth = mapWidth;
        tiles = new TileData[mapHeight, mapWidth];
        
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
    }

    public TileData this[int x, int y] 
    {
        get {
            if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) {
                GD.PrintErr("Tile position index is out of range!");
                return TileData.EMPTY;
            }
            
            return tiles[y, x];
        }
    }

    public override string ToString()
    {
        string strRepresentation = "";

        for (int x = 0; x < mapWidth; ++x) {
            strRepresentation += "| ";
            for (int y = 0; y < mapHeight; ++y)
                strRepresentation += this[x, y].GID + " | ";
            strRepresentation += "\n";
        }

        return strRepresentation;
    }
}

