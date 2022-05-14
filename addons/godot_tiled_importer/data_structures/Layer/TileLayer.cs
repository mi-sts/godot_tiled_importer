using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum TileLayerType
    {
        Infinite, NotInfinite
    }

    public class TileLayer : Layer
    {

        public TileLayerData data { get; private set; } // (for not infinite maps).
        public Chunk[] chunks { get; private set; } // Array of chunks (for infinite maps). 
        public int width { get; private set; }
        public int height { get; private set; }
        public TileLayerType tileLayerType;

        public TileLayer(LayerInfo layerInfo, int width, int height, TileLayerData data) : base(layerInfo)
        {
            if (data == null)
            {
                GD.PushError("Data of the tile layer is not initialized!");
                return;
            }
            this.data = data;
            this.width = width;
            this.height = height;
            tileLayerType = TileLayerType.NotInfinite;
        }

        public TileLayer(LayerInfo layerInfo, int width, int height, Chunk[] chunks) : base(layerInfo)
        {
            if (chunks == null)
            {
                GD.PushError("Chunks of the tile layer is not initialized!");
                return;
            }
            this.chunks = chunks;
            this.width = width;
            this.height = height;
            tileLayerType = TileLayerType.Infinite;
        }
    }
}
