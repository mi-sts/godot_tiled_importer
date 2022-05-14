using Godot;
using System;


namespace TiledImporter.Structures
{
    // Сontains information about the layer that was parsed from the data field.
    public class TileLayerData
    {
        public TileData[,] tiles;
        public int layerWidth { get; private set; }
        public int layerHeight { get; private set; }

        public TileLayerData(TileData[,] tiles, int layerWidth, int layerHeight)
        {
            if (layerWidth <= 0 || layerHeight <= 0)
            {
                GD.PushError("Incorrect size of the map!");
                return;
            }
            if (tiles.GetLength(0) != layerHeight || tiles.GetLength(1) != layerWidth)
            {
                GD.PushError("Number of tiles doesn't math the size of the map!");
                return;
            }

            this.tiles = tiles;
            this.layerWidth = layerWidth;
            this.layerHeight = layerHeight;
        }

        public TileData this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= layerWidth || y < 0 || y >= layerHeight)
                {
                    GD.PushError("Tile position index is out of range!");
                    return TileData.EMPTY;
                }

                return tiles[y, x];
            }
        }

        public override string ToString()
        {
            string strRepresentation = "";

            for (int y = 0; y < layerWidth; ++y)
            {
                strRepresentation += "| ";
                for (int x = 0; x < layerHeight; ++x)
                    strRepresentation += this[x, y].gID + " | ";
                strRepresentation += "\n";
            }

            return strRepresentation;
        }
    }
}
