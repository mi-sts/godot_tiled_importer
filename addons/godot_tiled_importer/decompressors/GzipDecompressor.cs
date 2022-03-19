using Godot;
using System;
using System.IO;
using System.IO.Compression;

public class GzipDecompressor : Decompressor
{
    public override byte[] Decompress(byte[] compressedData) 
    {
        var compressedDataStream = new MemoryStream(compressedData);
        var decompressedDataStream = new MemoryStream();

        var decompressor = new GZipStream(compressedDataStream, CompressionMode.Decompress);
        decompressor.CopyTo(decompressedDataStream);

        return decompressedDataStream.ToArray();
    }
}