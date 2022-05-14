using Godot;
using System;
using System.IO;
using System.IO.Compression;

namespace TiledImporter.Decompressors
{
    public class GZipDecompressor : Decompressor
    {
        public override byte[] Decompress(byte[] compressedData)
        {
            if (compressedData == null)
            {
                GD.PushError("Decompressing data is null!");
                return null;
            }
            var compressedDataStream = new MemoryStream(compressedData);
            var decompressedDataStream = new MemoryStream();

            var decompressor = new GZipStream(compressedDataStream, CompressionMode.Decompress);
            decompressor.CopyTo(decompressedDataStream);

            return decompressedDataStream.ToArray();
        }
    }
}
