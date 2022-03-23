using Godot;
using System;

public struct Chunk 
{
    public string[] data { get; private set; }
    public int height { get; private set; }
    public int width { get; private set; }
    public Point position { get; private set; }
}

