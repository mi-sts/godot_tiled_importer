using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum HorizontalAlignment
    {
        Center, Right, Justify, Left
    }

    public enum VerticalAlignment
    {
        Center, Bottom, Top
    }

    public struct TextInfo
    {
        public string text;
        public int? pixelSize;
        public bool? bold;
        public bool? italic;
        public string fontFamily;
        public Color? color;
        public HorizontalAlignment? halign;
        public VerticalAlignment? valign;
        public bool? kerning;
        public bool? strikeout;
        public bool? underline;
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
        public HorizontalAlignment halign { get; private set; }
        public VerticalAlignment valign { get; private set; }
        public bool kerning { get; private set; }
        public bool strikeout { get; private set; }
        public bool underline { get; private set; }
        public bool wrap { get; private set; }

        public Text(TextInfo textInfo)
        {
            if (textInfo.text == null)
            {
                GD.PushError("Text field of the text object is not initialized!");
            }
            text = textInfo.text ?? "";
            pixelSize = textInfo.pixelSize ?? 16;
            bold = textInfo.bold ?? false;
            italic = textInfo.italic ?? false;
            fontFamily = textInfo.fontFamily ?? "sans-serif";
            color = textInfo.color ?? new Color("#000000");
            halign = textInfo.halign ?? HorizontalAlignment.Left;
            kerning = textInfo.kerning ?? true;
            strikeout = textInfo.strikeout ?? false;
            underline = textInfo.underline ?? false;
            valign = textInfo.valign ?? VerticalAlignment.Top;
            wrap = textInfo.wrap ?? false;
        }
    }
}
