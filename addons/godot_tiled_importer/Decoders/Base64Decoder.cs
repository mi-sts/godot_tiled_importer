using Godot;
using System;
using System.Text;

public class Base64Decoder : Decoder
{
    public byte[] DecodeToBytes(string encodedString)
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

    public override LayerData Decode(string encodedString)
    {
        throw new NotImplementedException();
    }
}
