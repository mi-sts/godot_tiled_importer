using Godot;
using System;

namespace TiledImporter.Structures
{
    public class Chunk
    {
        public TileLayerData data { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public IntPoint position { get; private set; }

        public Chunk(TileLayerData data, int width, int height, IntPoint position)
        {
            if (data == null)
            {
                GD.PushError("Data of the chunk are not initialized!");
                width = 0;
                height = 0;
                position = new IntPoint(0, 0);
            }
            this.data = data ?? new TileLayerData(new TileData[0, 0], 0, 0);
            this.width = width;
            this.height = height;
            this.position = position;
        }
    }
}
