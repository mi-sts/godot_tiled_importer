using Godot;
using System;

public enum GridOrientation { 
    Orthogonal, Isometric
} 

public struct Grid 
{
    public GridOrientation orintation { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }

    public Grid(int width, int height, GridOrientation orientation = GridOrientation.Orthogonal) {
        this.width = width;
        this.height = height;
        this.orintation = orientation;
    }
}

