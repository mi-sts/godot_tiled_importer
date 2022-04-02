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

    public override TileLayerData Decode(string encodedString, int layerWidth, int layerHeight) {
        if (encodedString == null) {
            GD.PushError("Decoding string is null!");
            return null;
        }
        string[] rows = encodedString.Split('\n');
        if (rows.Length != layerHeight) {
            GD.PushError("Number of the map layer doesn't math the height of the layer!");
            return null;
        }

        uint[] tileIDs = new uint[layerWidth * layerHeight];
        bool[][] flipFlags = new bool[layerWidth * layerHeight][];

        for (int y = 0; y < layerHeight; ++y) {
            string[] rowElements = rows[y].Split(',').ToArray();
            bool lastIsBreakLine = rowElements.Length == (layerWidth + 1) && rowElements[layerWidth] == "";
            if (!lastIsBreakLine && rowElements.Length != layerWidth) {
                GD.PushError("Number of elements in the layer row doesn't match the width of the layer!");
                return null;
            }
            uint[] rowTileIDs = ParseRowTileIDs(rowElements);
            bool[][] rowfilpFlags = DecodeFlipFlagsAndClear(ref rowTileIDs);

            try {
                Array.Copy(rowTileIDs, 0, tileIDs, y * layerWidth, layerWidth);
            }
            catch (ArgumentOutOfRangeException) {
                GD.PushError("Number of tiles ID copied doesn't match the size of one of the arrays!");
                return null;
            }

            try {
                Array.Copy(rowfilpFlags, 0, flipFlags, y * layerWidth, layerWidth);
            }
            catch (ArgumentOutOfRangeException) {
                GD.PushError("Number of set of the flags copied doesn't match the size of one of the arrays!");
                return null;
            }
        }

        return CreateLayerData(tileIDs, flipFlags, layerWidth, layerHeight);
    }
}
