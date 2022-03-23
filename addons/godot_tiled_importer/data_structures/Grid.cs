using Godot;
using System;

public enum GridOrientation { 
    Orthogonal, Isometric
} 

public struct Grid 
{
    public int height { get; private set; }
    public int width { get; private set; }
    public GridOrientation orintation { get; private set; }
}

