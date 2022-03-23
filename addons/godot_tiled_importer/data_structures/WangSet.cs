using Godot;
using System;

public enum WangSetType {
    Corner, Edge, Mixed
}

public struct WangSet 
{
    
    public string name { get; private set; }
    public WangTile[] wangTiles { get; private set; }
    public WangColor[] colors { get; private set; }
    public int tile { get; private set; }
    public Property properties { get; private set; }
    public WangSetType type { get; private set; }
}

