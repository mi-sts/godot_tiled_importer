using Godot;
using System;
using System.Text;

public class Base64Decoder : Decoder
{
    protected byte[] DecodeToByteData(string encodedString)
    {
        if (encodedString == null) {
            GD.PushError("Decoding string are empty!");
            return null;
        }

        byte[] bytesData;
        try {
            bytesData = Convert.FromBase64String(encodedString);
        }
        catch (FormatException) {
            GD.PushError("String format not suitable for base64 decoding!");
            return null;
        }

        return bytesData;
    }

    private uint[] ConvertByteArrayToUIntArray(byte[] byteArray) {
        uint[] tileIDs = new uint[byteArray.Length / 4];
        try {
            Buffer.BlockCopy(byteArray, 0, tileIDs, 0, byteArray.Length);
        }
        catch (ArgumentOutOfRangeException) {
            GD.PushError("Number of bytes copied doesn't match the size of the array!");
            return null;
        }

        return tileIDs;
    }

    protected LayerData DecodedByteDataToLayerData(byte[] decodedData, int mapWidth, int mapHeight) {
        if (decodedData.Length % 4 != 0) {
            GD.PushError("Incorrect data. Decoded data can't be devided into uint IDs");
            return null;
        }

        // If the system architecture is big-endian.
        // reverse the byte array to keep the little-endian reading.
        if (!BitConverter.IsLittleEndian) 
            Array.Reverse(decodedData);

        uint[] tileIDs = ConvertByteArrayToUIntArray(decodedData);
        if (tileIDs == null) 
            return null;
        bool[][] flipFlags = DecodeFlipFlagsAndClear(ref tileIDs);

        return CreateLayerData(tileIDs, flipFlags, mapWidth, mapHeight );
    }

    public override LayerData Decode(string encodedString, int mapWidth, int mapHeigth)
    {
        byte[] decodedData = DecodeToByteData(encodedString);
        return DecodedByteDataToLayerData(decodedData, mapWidth, mapHeigth);
    }
}
