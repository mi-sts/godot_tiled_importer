using Godot;
using System;

public struct WangColor 
{
    public string name { get; private set; }
    public Color color { get; private set; }
    public Property properties { get; private set; }
    public double probability { get; private set; }
    public int tile { get; private set; }
}

