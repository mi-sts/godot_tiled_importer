using Godot;
using System;

public struct Terrain 
{
    public string name { get; private set; }
    public Property[] properties { get; private set; }
    public int tile { get; private set; }

    public Terrain(string name, Property[] properties, int tile) {
        if (name == null) {
            GD.PushError("Name of the terrain is not initialized!");
        }
        this.name = name ?? "";
        this.properties = properties;
        this.tile = tile;
    }
}

