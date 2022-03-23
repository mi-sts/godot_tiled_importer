using Godot;
using System;

public struct Tile 
{
    public int ID { get; private set; }
    public string imageWidth { get; private set; }
    public string imageHeight { get; private set; }
    public string type { get; private set; }
    public Property[] properties { get; private set; }
    public string image { get; private set; }
    public Layer objectGroup { get; private set; } // Layer with type objectgroup (optional).
    public double probability { get; private set; }
    public Terrain[] terrain { get; private set; }
    public Frame[] animation { get; private set; }
}

