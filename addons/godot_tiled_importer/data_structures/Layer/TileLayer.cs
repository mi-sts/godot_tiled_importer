using Godot;
using System;

public class TileLayer : Layer {

    public TileLayerData data { get; private set; } // (for not infinite maps).
    public Chunk[] chunks { get; private set; } // Array of chunks (for infinite maps). 

    public TileLayer(LayerInfo layerInfo, TileLayerData data) : base(layerInfo) {
        if (data == null) {
            GD.PushError("Data of the tile layer is not initialized!");
            return;
        }
    }

    public TileLayer(LayerInfo layerInfo, Chunk[] chunks) : base(layerInfo) {
        if (chunks == null) {
            GD.PushError("Chunks of the tile layer is not initialized!");
        }
        this.chunks = chunks ?? new Chunk[0];
    }
}
