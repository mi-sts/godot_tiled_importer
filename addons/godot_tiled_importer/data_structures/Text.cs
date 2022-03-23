using Godot;
using System;

public enum HorizontalAlgignment {
    CENTER,
    RIGHT,
    JUSTIFY,
    LEFT
}

public enum VerticalAlignment {
    CENTER,
    BOTTOM,
    TOP
}

public struct Text 
{
    public string text { get; private set; }
    public int pixelSize { get; private set; }
    public bool bold { get; private set; }
    public bool italic { get; private set; }
    public string fontfamily { get; private set; }
    public Color color { get; private set; }
    public HorizontalAlgignment halign { get; private set; }
    public bool kerning { get; private set; }
    public bool strikeout { get; private set; }
    public bool underline { get; private set; }
    public VerticalAlignment valing { get; private set; }
    public bool wrap { get; private set; }
}

