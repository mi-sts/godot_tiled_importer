using Godot;
using System;

public enum HorizontalAlgignment {
    Center, Right, Justify, Left
}

public enum VerticalAlignment {
    Center, Bottom, Top
}

public class TextInfo {
    public string text;
    public int pixelSize = 16;
    public bool bold = false;
    public bool italic = false;
    public string fontFamily = "sans-serif";
    public Color color = new Color("#000000");
    public HorizontalAlgignment halign = HorizontalAlgignment.Left;
    public bool kerning = true;
    public bool strikeout = false;
    public bool underline = false;
    public VerticalAlignment valing = VerticalAlignment.Top;
    public bool wrap = false;
}

public struct Text 
{
    public string text { get; private set; }
    public int pixelSize { get; private set; }
    public bool bold { get; private set; }
    public bool italic { get; private set; }
    public string fontFamily { get; private set; }
    public Color color { get; private set; }
    public HorizontalAlgignment halign { get; private set; }
    public bool kerning { get; private set; }
    public bool strikeout { get; private set; }
    public bool underline { get; private set; }
    public VerticalAlignment valing { get; private set; }
    public bool wrap { get; private set; }

    public Text(TextInfo textInfo) {
        if (textInfo.text == null) {
            GD.PushError("Text field of the text object is not initialized!");
            text = "";
        } else {
            text = textInfo.text;
        }
        pixelSize = textInfo.pixelSize;
        bold = textInfo.bold;
        italic = textInfo.italic;
        fontFamily = textInfo.fontFamily;
        color = textInfo.color;
        halign = textInfo.halign;
        kerning = textInfo.kerning;
        strikeout = textInfo.strikeout;
        underline = textInfo.underline;
        valing = textInfo.valing;
        wrap = textInfo.wrap;
    }
}

