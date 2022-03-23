using Godot;
using System;

public enum MapOrientation {
    Orthogonal, Isometric, Staggered, Hexagonal
}

public enum RenderOrder {
    RightDown, RightUp, LeftDown, LeftUp 
}

public class Map
{   
    public LayerData[] layers { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }
    public int compressionLevel { get; private set; }
    public MapOrientation mapOrientation { get; private set; }
    public Property[] properties { get; private set; }
    public int tileWidth { get; private set;}
    public int tileHeight { get; private set; }
    public Tileset[] tilesets { get; private set; }
    public int hexSideLength { get; private set; } // Hexagonal maps only.
    public Color backgroundColor { get; private set; }
    public bool infinite { get; private set; }
    public Point parallaxOriginPoint { get; private set; }
    public RenderOrder renderOrder { get; private set; }
    public int staggerAxis { get; private set; } // Staggered / hexagonal maps only.
    public int staggerIndex { get; private set; } // Staggered / hexagonal maps only.
    public int tiledVersion { get; private set; }
    public string type { get; private set; }
}

