using Godot;
using System;
using System.Linq;

public enum Compression {
    None, ZLib, GZip, ZStd
}

public enum DrawOrder {
    TopDown, Index
}

public enum Encoding {
    CSV, Base64,
}

public enum LayerType {
    TileLayer, ObjectGroup, ImageLayer, Group
}

public struct LayerInfo {
    public string name;
    public uint[] data;
    public int? id;
    public Point? startPoint;
    public int? width;
    public int? height;
    public Property[] properties;
    public Vector2? tilesOffset;
    public bool? visible;
    public double? opacity;
    public bool? locked;
    public Vector2 pixelsOffset;
    public Vector2 parallaxFactor;
    public Color tintColor;
}

public abstract class Layer
{
    public string name { get; private set; }
    public uint[] data { get; private set; }
    public int id { get; private set; } // Incremental ID - unique across all layers.
    public Point? startPoint { get; private set; } // Point where content starts (for infinite maps).
    public int width { get; private set; }
    public int height { get; private set; }
    public Property[] properties { get; private set; }
    public Vector2 tilesOffset { get; private set; } // Layer offset in tiles. Always 0.
    public bool visible { get; private set; }
    public LayerType type { get; private set; }
    public double opacity { get; private set; }
    public bool? locked { get; private set; } // Whether layer is locked in the editor (since 1.8.2).
    public Vector2 pixelsOffset { get; private set; } // Layer offset vector in pixels.
    public Vector2? parallaxFactor { get; private set; } // Parallax factor vector for this layer (since 1.5).
    public Color? tintColor { get; private set; } // Color multiplied with any graphics drawn (optional).
    public Layer(LayerInfo layerInfo) {
        var requiredParameters = new object[] { 
            layerInfo.name,
            layerInfo.data,
            layerInfo.id,
            layerInfo.width,
            layerInfo.height,
            layerInfo.properties,
            layerInfo.tilesOffset,
            layerInfo.visible,
            layerInfo.opacity,
            layerInfo.pixelsOffset
        }; 
        if (requiredParameters.Any(parameter => parameter == null)) {
            GD.PushError("Not all of the required layer parameters are initialized!");
        }
        name = layerInfo.name ?? "";
        data = layerInfo.data ?? new uint[0];
        id = layerInfo.id ?? 0;
        width = layerInfo.width ?? 0;
        height = layerInfo.height ?? 0;
        properties = layerInfo.properties ?? new Property[0];
        tilesOffset = layerInfo.tilesOffset ?? new Vector2(0, 0);
        visible = layerInfo.visible ?? false;
        opacity = layerInfo.opacity ?? 0.0;
        pixelsOffset = layerInfo.pixelsOffset;
        locked = layerInfo.locked;
        parallaxFactor = layerInfo.parallaxFactor;
        tintColor = layerInfo.tintColor;
        startPoint = layerInfo.startPoint;
    }
}

