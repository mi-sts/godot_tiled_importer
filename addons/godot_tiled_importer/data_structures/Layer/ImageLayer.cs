using Godot;
using System;

namespace TiledImporter.Structures
{
    public class ImageLayer : Layer
    {
        public string image { get; private set; } // Image used by this layer.
        public bool repeatX { get; private set; } // Whether the image drawn is repeated along the axis (since 1.8).
        public bool repeatY { get; private set; }
        public Color? transparentColor { get; private set; } // (optional).

        public ImageLayer(
            LayerInfo layerInfo,
            string image,
            bool repeatX = false,
            bool repeatY = false,
            Color? transparentColor = null
            ) : base(layerInfo)
        {
            if (image == null)
            {
                GD.PushError("Image of the image layer is not initialized!");
            }
            this.image = image ?? "";
            this.repeatX = repeatX;
            this.repeatY = repeatY;
            this.transparentColor = transparentColor;
        }
    }
}
