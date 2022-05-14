using Godot;
using System;

namespace TiledImporter.Structures
{
    public class GroupLayer : Layer
    {
        public Layer[] layers { get; private set; }

        public GroupLayer(LayerInfo layerInfo, Layer[] layers) : base(layerInfo)
        {
            if (layers == null)
            {
                GD.PushError("Layers of the group layer are not initialized!");
            }
            this.layers = layers ?? new Layer[0];
        }
    }
}
