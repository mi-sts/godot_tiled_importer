using Godot;
using System;

public enum ObjectType {
    Object, Ellipse, Rectangle, Point, Polygon, Polyline, Text
}

public struct Object 
{
    public string name { get; private set; }
    public ObjectType objectType { get; private set; }
    public int id { get; private set; }
    public Point coordinates;
    public double width { get; private set; }
    public double height { get; private set; }
    public double rotation { get; private set; }
    public int GID { get; private set; } // Only if object represents a tile.
    public Point[] points { get; private set; }
    public string template { get; private set; }
    public Text text { get; private set; } // Only for text objects.
    public string type { get; private set; }
    public bool visible { get; private set; }
}

