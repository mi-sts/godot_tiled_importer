using Godot;
using System;

public class TileLayer : Layer
{
    public Chunk[] chunks { get; private set; } // Array of chunks (optional). 

    public TileLayer(LayerInfo layerInfo, Chunk[] chunks) : base(layerInfo)  {
        if (chunks == null) {
            GD.PushError("Chunks of the tile layer is not initialized!");
        }
        this.chunks = chunks ?? new Chunk[0];
    }
}
