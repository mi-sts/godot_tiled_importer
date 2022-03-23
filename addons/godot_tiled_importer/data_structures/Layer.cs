using Godot;
using System;

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
public class Layer
{
    public string name { get; private set; }
    public string[] data { get; private set; }
    public int id { get; private set; }
    public Point startPoint { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }
    public Property[] properties { get; private set; }
    public Vector2 tilesOffset { get; private set; }
    public bool visible { get; private set; }
    public LayerType type { get; private set; }
    public double opacity { get; private set; }
    public Chunk[] chunks { get; private set; } // tilelayer only.
    public Compression compression { get; private set; } // tilelayer only.
    public DrawOrder drawOrder { get; private set; } // objectgroup only.
    public Encoding encoding { get; private set; }
    public string image { get; private set; } // imagelayer only.
    public Layer[] layers { get; private set; } // group only.
    public bool locked { get; private set; } 
    public Object[] objects { get; private set; } // objectgroup only.
    public Vector2 pixelsOffset { get; private set; }
    public Vector2 parallaxFactor { get; private set; }
    public bool repeatX { get; private set; }
    public bool repeatY { get; private set; }
    public Color tintColot { get; private set; }
    public Color transparentColot { get; private set; }
    public WangSet[] wangSets { get; private set; }
}

