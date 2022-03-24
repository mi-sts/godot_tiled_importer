using Godot;
using System;

public struct WangColor 
{
    public static WangColor NullWangColor = new WangColor() {
        name = "",
        color = new Color(),
        properties = new Property[0],
        probability = 0.0,
        tileID = -1
    };

    public string name { get; private set; }
    public Color color { get; private set; }
    public Property[] properties { get; private set; }
    public double probability { get; private set; }
    public int tileID { get; private set; }

    public WangColor(string name, Color color, Property[] properties, double probability, int tileID) {
        if (name == null || properties == null) {
            GD.PushError("Properties of the wang color are not initialized!");
            this = NullWangColor;
            return;
        }
        this.name = name;
        this.color = color;
        this.properties = properties;
        this.probability = probability;
        this.tileID = tileID;
    }
}

