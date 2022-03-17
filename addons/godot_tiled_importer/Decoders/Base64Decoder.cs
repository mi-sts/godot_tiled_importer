using Godot;
using System;
using System.Text;

public class Base64Decoder : Decoder
{
    public override string Decode(string encodedString)
    {
        byte[] bytesData = Convert.FromBase64String(encodedString);
        return Encoding.UTF8.GetString(bytesData);
    }
}
