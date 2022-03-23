using Godot;
using System;

public enum TileObjectsAlignment {
    Uncspecified, TopLeft, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight
}

public class Tileset 
{
    public string name { get; private set; }
    public int tileWidth { get; private set; }
    public int tileHeight { get; private set; }
    public Tile[] tiles { get; private set; }
    public string image { get; private set; }
    public int imageWidth { get; private set; }
    public int imageHeight { get; private set; }
    public string source { get; private set; }
    public Property[] properties { get; private set; }
    public int firstGID { get; private set; }
    public int tileCount { get; private set; }
    public int columns { get; private set; }
    public Color backgroundColor { get; private set; }
    public Grid grid { get; private set; }
    public int margin { get; private set; }
    public TileObjectsAlignment objectsAlignment { get; private set; }
    public int spacing { get; private set; }
    public Terrain terrains { get; private set; } 
    public string tiledVersion { get; private set; }
    public Vector2 tileOffset { get; private set; }
    public Transfromations transfromations { get; private set; }
    public Color transparentColot { get; private set; }
    public string type { get; private set; }
    
}

