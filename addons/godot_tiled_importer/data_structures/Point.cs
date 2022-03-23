using Godot;
using System;

public struct Point 
{
    public double x { get; private set; }
    public double y { get; private set; }

    public Point(double x, double y) {
        this.x = x;
        this.y = y;
    }
}

