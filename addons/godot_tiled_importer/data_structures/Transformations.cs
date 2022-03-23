using Godot;
using System;

public struct Transfromations 
{
    public bool hflip { get; private set; }
    public bool vflip { get; private set; }
    public bool rotate { get; private set; }
    public bool preferUntransformed { get; private set; }
}

