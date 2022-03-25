using Godot;
using System;

public struct WangColor 
{
    public string name { get; private set; }
    public Color color { get; private set; }
    public Property[] properties { get; private set; }
    public double probability { get; private set; }
    public int tileID { get; private set; }

    public WangColor(string name, Color color, Property[] properties, double probability, int tileID) {
        if (name == null || properties == null) {
            GD.PushError("Not all properties of the wang color are not initialized!");
        }
        this.name = name ?? "";
        this.color = color;
        this.properties = properties ?? new Property[0];
        this.probability = probability;
        this.tileID = tileID;
    }
}

