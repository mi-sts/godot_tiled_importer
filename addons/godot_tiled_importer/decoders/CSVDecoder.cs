using System;
using System.Linq;
using Godot;

public class CSVDecoder : Decoder {
    private uint[] ParseRowTileIDs(string[] rowElements) {
        uint parseResult = 0;
        return Array.ConvertAll(rowElements, strID => {
            if (uint.TryParse(strID, out parseResult))
                return parseResult;
            else {
                GD.PushError("Incorrect tile ID data recieved!");
                return 0u;
            }
        });
    }

    public override LayerData Decode(string encodedString, int mapWidth, int mapHeight) {
        string[] rows = encodedString.Split('\n');
        if (rows.Length != mapHeight) {
            GD.PushError("Number of the map rows doesn't math the height of the map!");
            return null;
        }

        uint[] tileIDs = new uint[mapWidth * mapHeight];
        bool[][] flipFlags = new bool[mapWidth * mapHeight][];

        for (int y = 0; y < mapHeight; ++y) {
            string[] rowElements = rows[y].Split(',').ToArray();
            bool lastIsBreakLine = rowElements.Length == (mapWidth + 1) && rowElements[mapWidth] == "";
            if (!lastIsBreakLine && rowElements.Length != mapWidth) {
                GD.PushError("Number of elements in the map row doesn't match the width of the map!");
                return null;
            }
            uint[] rowTileIDs = ParseRowTileIDs(rowElements);
            bool[][] rowfilpFlags = DecodeFlipFlagsAndClear(ref rowTileIDs);

            try {
                Array.Copy(rowTileIDs, 0, tileIDs, y * mapWidth, mapWidth);
            } 
            catch (ArgumentOutOfRangeException) {
                GD.PushError("Number of tiles ID copied doesn't match the size of one of the arrays!");
                return null;
            }

            try {
                Array.Copy(rowfilpFlags, 0, flipFlags, y * mapWidth, mapWidth);
            } 
            catch (ArgumentOutOfRangeException) {
                GD.PushError("Number of set of the flags copied doesn't match the size of one of the arrays!");
                return null;
            }
        }

        return CreateLayerData(tileIDs, flipFlags, mapWidth, mapHeight);
    }
}
