using Godot;
using System;
using System.IO;
using System.IO.Compression;

namespace TiledImporter.Decompressors
{
    public class ZLibDecompressor : Decompressor
    {
        public override byte[] Decompress(byte[] compressedData)
        {
            if (compressedData == null)
            {
                GD.PushError("Decompressing data is null!");
                return null;
            }
            // First two bytes are the RFC 1950 magic bytes need to be cut.
            var compressedDataStream = new MemoryStream(compressedData, 2, compressedData.Length - 2);
            var decompressedDataStream = new MemoryStream();

            var decompressor = new DeflateStream(compressedDataStream, CompressionMode.Decompress);
            decompressor.CopyTo(decompressedDataStream);

            return decompressedDataStream.ToArray();
        }
    }
}
