using Godot;
using System;

public struct Terrain 
{
    public string name { get; private set; }
    public Property[] properties { get; private set; }
    public int tile { get; private set; }
}

