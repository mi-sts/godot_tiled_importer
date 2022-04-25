using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum DrawOrder
    {
        TopDown, Index
    }

    public class ObjectGroupLayer : Layer
    {
        public Object[] objects { get; private set; }
        public DrawOrder drawOrder { get; private set; } // topdown (default) or index.

        public ObjectGroupLayer(LayerInfo layerInfo, Object[] objects, DrawOrder drawOrder = DrawOrder.TopDown) : base(layerInfo)
        {
            if (objects == null)
            {
                GD.PushError("Objects of the object group layer are not initialized!");
            }
            this.objects = objects ?? new Object[0];
            this.drawOrder = drawOrder;
        }
    }
}
