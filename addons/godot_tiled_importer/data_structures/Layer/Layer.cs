using Godot;
using System;
using System.Linq;

namespace TiledImporter.Structures
{
    public enum LayerType
    {
        TileLayer, ObjectGroup, ImageLayer, Group
    }

    public struct LayerInfo
    {
        public string name;
        public int? id;
        public IntPoint? startPoint;
        public Property[] properties;
        public IntPoint? tilesOffset;
        public bool? visible;
        public LayerType? type;
        public double? opacity;
        public bool? locked;
        public IntPoint? pixelsOffset;
        public Point? parallaxFactor;
        public Color? tintColor;
        public bool? infinite;
    }

    public abstract class Layer
    {
        public string name { get; private set; }
        public int id { get; private set; } // Incremental ID - unique across all layers.
        public IntPoint? startPoint { get; private set; } // Point where content starts (for infinite maps).
        public Property[] properties { get; private set; }
        public IntPoint tilesOffset { get; private set; } // Layer offset in tiles. Always 0.
        public bool visible { get; private set; }
        public LayerType type { get; private set; }
        public double opacity { get; private set; }
        public bool locked { get; private set; } // Whether layer is locked in the editor (since 1.8.2).
        public IntPoint? pixelsOffset { get; private set; } // Layer offset vector in pixels.
        public Point? parallaxFactor { get; private set; } // Parallax factor vector for this layer (since 1.5).
        public Color? tintColor { get; private set; } // Color multiplied with any graphics drawn (optional).
        public bool infinite { get; private set; }

        public Layer(LayerInfo layerInfo)
        {
            var requiredFields = new object[] {
            layerInfo.name,
            layerInfo.id,
            layerInfo.tilesOffset,
            layerInfo.visible,
            layerInfo.type,
            layerInfo.opacity,
            layerInfo.infinite
        };
            if (requiredFields.Any(field => field == null))
            {
                GD.PushError("Not all of the required layer parameters are initialized!");
            }
            name = layerInfo.name ?? "";
            id = layerInfo.id ?? 0;
            properties = layerInfo.properties ?? new Property[0];
            tilesOffset = layerInfo.tilesOffset ?? new IntPoint(0, 0);
            visible = layerInfo.visible ?? false;
            type = layerInfo.type ?? LayerType.ObjectGroup;
            opacity = layerInfo.opacity ?? 0.0;
            infinite = layerInfo.infinite ?? false;

            pixelsOffset = layerInfo.pixelsOffset;
            locked = layerInfo.locked ?? false;
            parallaxFactor = layerInfo.parallaxFactor;
            tintColor = layerInfo.tintColor;
            startPoint = layerInfo.startPoint;
        }
    }
}
