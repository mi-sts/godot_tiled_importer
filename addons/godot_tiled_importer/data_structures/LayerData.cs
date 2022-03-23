using Godot;
using System;

// Сontains information about the layer that was parsed from the data field.
public class LayerData 
{
    public TileData[,] tiles;
    public int mapWidth { get; private set; }
    public int mapHeight { get; private set; }

    public LayerData(TileData[,] tiles, int mapWidth, int mapHeight) {
        if (mapWidth <= 0 || mapHeight <= 0) {
            GD.PushError("Incorrect size of the map!");
            return;
        }
        if (tiles.GetLength(0) != mapHeight || tiles.GetLength(1) != mapWidth) {
            GD.PushError("Number of tiles doesn't math the size of the map!");
            return;
        }
        
        this.tiles = tiles;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
    }

    public TileData this[int x, int y] 
    {
        get {
            if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) {
                GD.PushError("Tile position index is out of range!");
                return TileData.EMPTY;
            }
            
            return tiles[y, x];
        }
    }

    public override string ToString()
    {
        string strRepresentation = "";

        for (int y = 0; y < mapWidth; ++y) {
            strRepresentation += "| ";
            for (int x = 0; x < mapHeight; ++x)
                strRepresentation += this[x, y].GID + " | ";
            strRepresentation += "\n";
        }

        return strRepresentation;
    }
}

