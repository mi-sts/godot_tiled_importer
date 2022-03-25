using Godot;
using System;

public enum HorizontalAlgignment {
    Center, Right, Justify, Left
}

public enum VerticalAlignment {
    Center, Bottom, Top
}

public struct TextInfo {
    public string text;
    public int? pixelSize;
    public bool? bold;
    public bool? italic;
    public string fontFamily;
    public Color? color;
    public HorizontalAlgignment? halign;
    public bool? kerning;
    public bool? strikeout;
    public bool? underline;
    public VerticalAlignment? valing;
    public bool? wrap;
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
        } 
        text = textInfo.text ?? "";
        pixelSize = textInfo.pixelSize ?? 16;
        bold = textInfo.bold ?? false;
        italic = textInfo.italic ?? false;
        fontFamily = textInfo.fontFamily ?? "sans-serif";
        color = textInfo.color ?? new Color("#000000");
        halign = textInfo.halign ?? HorizontalAlgignment.Left;
        kerning = textInfo.kerning ?? true;
        strikeout = textInfo.strikeout ?? false;
        underline = textInfo.underline ?? false;
        valing = textInfo.valing ?? VerticalAlignment.Top;
        wrap = textInfo.wrap ?? false;
    }
}

