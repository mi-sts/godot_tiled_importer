using Godot;
using System;
using System.IO;
using System.IO.Compression;

public class ZlibDecompressor : Decompressor
{
    public override byte[] Decompress(byte[] compressedData) 
    {
        // First two bytes are the RFC 1950 magic bytes need to be cut.
        var compressedDataStream = new MemoryStream(compressedData, 2, compressedData.Length - 2);
        var decompressedDataStream = new MemoryStream();

        var decompressor = new DeflateStream(compressedDataStream, CompressionMode.Decompress);
        decompressor.CopyTo(decompressedDataStream);

        return decompressedDataStream.ToArray();
    }
}
