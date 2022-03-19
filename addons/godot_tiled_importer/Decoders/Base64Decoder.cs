using Godot;
using System;
using System.Text;

public class Base64Decoder : Decoder
{
    const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
    const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
    const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;
    const uint ROTATED_HEXAGONAL_120_FLAG = 0x10000000;

    protected byte[] DecodeToByteData(string encodedString)
    {
        if (encodedString == null) {
            GD.PrintErr("Decoding string are empty!");
            return null;
        }

        byte[] bytesData;
        try {
            bytesData = Convert.FromBase64String(encodedString);
        }
        catch (FormatException) {
            GD.PrintErr("String format not suitable for base64 decoding!");
            return null;
        }

        return bytesData;
    }

    private bool[] DecodeFlipFlags(uint encodedFlags) {
        bool[] flags = new bool[4];
        flags[0] = (encodedFlags & FLIPPED_HORIZONTALLY_FLAG) != 0; 
        flags[1] = (encodedFlags & FLIPPED_VERTICALLY_FLAG) != 0; 
        flags[2] = (encodedFlags & FLIPPED_DIAGONALLY_FLAG) != 0;
        flags[3] = (encodedFlags & ROTATED_HEXAGONAL_120_FLAG) != 0;

        return flags;
    }

    private void ClearFlipFlagsBits(ref uint encodedTileData) {
        encodedTileData &= ~(FLIPPED_HORIZONTALLY_FLAG |
                             FLIPPED_VERTICALLY_FLAG |
                             FLIPPED_DIAGONALLY_FLAG |
                             ROTATED_HEXAGONAL_120_FLAG);
    } 

    protected LayerData DecodedByteDataToLayerData(int mapWidth, int mapHeight, byte[] decodedData) {
        // If the system architecture is big-endian.
        // reverse the byte array to keep the little-endian reading.
        if (!BitConverter.IsLittleEndian) 
            Array.Reverse(decodedData);

        // Convert a byte array to an uint array.
        uint[] convertedData = new uint[decodedData.Length / 4];
        Buffer.BlockCopy(decodedData, 0, convertedData, 0, decodedData.Length);

        // Extrude flip flags from encoded tiles data and clear flag bits.
        bool[][] flipFlags = new bool[convertedData.Length][];
        for (int i = 0; i < convertedData.Length; ++i) {
            flipFlags[i] = DecodeFlipFlags(convertedData[i]);
            ClearFlipFlagsBits(ref convertedData[i]);
        }

        return new LayerData(mapWidth, mapHeight, convertedData, flipFlags);
    }

    public override LayerData Decode(int mapWidth, int mapHeidth, string encodedString)
    {
        byte[] decodedData = DecodeToByteData(encodedString);
        return DecodedByteDataToLayerData(mapWidth, mapHeidth, decodedData);
    }
}
