using System;
using System.Linq;
using Godot;

public class CSVDecoder : Decoder {
    private uint[] ParseTileIDs(string[] elements) {
        uint parseResult = 0;
        return Array.ConvertAll(elements, strID => {
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

        encodedString = encodedString.Substr(1, encodedString.Length - 2); // Remove '[' and ']' array symbols at the beginning and end.
        string[] elements = encodedString.Split(',').ToArray();
        if (elements.Length != layerWidth * layerHeight) {
            GD.PushError("Number of the map layer doesn't math the height of the layer!");
            return null;
        }
        uint[] tileIDs = ParseTileIDs(elements);
        bool[][] flipFlags = DecodeFlipFlagsAndClear(ref tileIDs);

        return CreateLayerData(tileIDs, flipFlags, layerWidth, layerHeight);
    }
}
