using System;

namespace TiledImporter.Decompressors
{
    public abstract class Decompressor
    {
        public abstract byte[] Decompress(byte[] compressedData);
    }
}
