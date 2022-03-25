using Godot;
using System;

public struct Chunk 
{
    public uint[] data { get; private set; }
    
    public int width { get; private set; }
    public int height { get; private set; }
    public Point position { get; private set; }

    public Chunk(uint[] data, int width, int height, Point position) {
        if (data == null) {
            GD.PushError("Data of the chunk are not initialized!");
        }
        this.data = data ?? new uint[0];
        this.width = width;
        this.height = height;
        this.position = position;
    }
}
