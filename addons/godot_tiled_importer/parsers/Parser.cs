using Godot;
using System;

public enum Compression {
    None, ZLib, GZip, ZStd
}
public enum Encoding {
    CSV, Base64,
}

public abstract class Parser {
    public abstract Map Parse(string data);

    protected int? ToInt(object data) {
        if (int.TryParse(ToString(data), out int result))
            return result;

        return null;
    }

    protected uint? ToUInt(object data) {
        if (uint.TryParse(ToString(data), out uint result))
            return result;
        
        return null;
    }

    protected double? ToDouble(object data) {
        if (double.TryParse(ToString(data), out double result))
            return result;

        return null;
    }

    protected ushort? ToUShort(object data) {
        if (ushort.TryParse(ToString(data), out ushort result))
            return result;

        return null;
    }

    protected bool? ToBool(object data) {
        if (bool.TryParse(ToString(data), out bool result))
            return result;

        return null;
    }

    protected string ToString(object data) {
        return data as string;
    }

    protected MapOrientation? DetermineMapOrientation(string orientation) {
        switch (orientation) {
            case "orthogonal":
                return MapOrientation.Orthogonal;
            case "isometric":
                return MapOrientation.Isometric;
            case "staggered":
                return MapOrientation.Staggered;
            case "hexagonal":
                return MapOrientation.Hexagonal;
            default:
                GD.PushError("Can't determine the map orientation!");
                return null;
        }
    }

    protected LayerType? DetermineLayerType(string type) {
        switch (type) {
            case "tilelayer":
                return LayerType.TileLayer;
            case "objectgroup":
                return LayerType.ObjectGroup;
            case "imagelayer":
                return LayerType.ImageLayer;
            case "group":
                return LayerType.Group;
            default:
                GD.PushError("Can't determine a layer type!");
                return null;
        }
    }

    protected Compression? DetermineCompression(string compression) {
        switch (compression) {
            case "zlib":
                return Compression.ZLib;
            case "gzip":
                return Compression.GZip;
            case "zstd":
                return Compression.ZStd;
            case "":
                return Compression.None;
            default:
                GD.PushError("Can't deterimine a compression type of the level data!");
                return null;
        }
    }

    protected Encoding? DetermineEncoding(string encoding) {
        switch (encoding) {
            case "csv":
                return Encoding.CSV;
            case "base64":
                return Encoding.Base64;
            default:
                GD.PushError("Can't determine an encoding type of the level data!");
                return null;
        }
    }

    protected DrawOrder? DetermineDrawOrder(string drawOrder) {
        switch (drawOrder) {
            case "topdown":
                return DrawOrder.TopDown;
            case "index":
                return DrawOrder.Index;
            default:
                GD.PushError("Can't determine an draw order of the object layer!");
                return null;
        }
    }

    protected PropertyType? DeterminePropertyType(string type) {
        switch (type) {
            case "string":
                return PropertyType.String;
            case "int":
                return PropertyType.Int;
            case "float":
                return PropertyType.Float;
            case "bool":
                return PropertyType.Bool;
            case "color":
                return PropertyType.Color;
            case "file":
                return PropertyType.File;
            case "object":
                return PropertyType.Object;
            case "class":
                return PropertyType.Class;
            default:
                GD.PushError("Can't determine a property type!");
                return null;
        }
    }

    protected HorizontalAlignment? DetermineHorizontalAlignment(string alignment) {
        switch (alignment) {
            case "center":
                return HorizontalAlignment.Center;
            case "right":
                return HorizontalAlignment.Right;
            case "justify":
                return HorizontalAlignment.Justify;
            case "left":
                return HorizontalAlignment.Justify;
            default:
                GD.PushError("Can't determine a horizontal alignment type!");
                return null;
        }
    }

    protected VerticalAlignment? DetermineVerticalAlignment(string alignment) {
        switch (alignment) {
            case "center":
                return VerticalAlignment.Center;
            case "bottom":
                return VerticalAlignment.Bottom;
            case "top":
                return VerticalAlignment.Top;
            default:
                GD.PushError("Can't determine a vertical alignment type!");
                return null;
        }
    }

    protected StaggerAxis? DetermineStaggerAxis(string staggerAxis) {
        switch (staggerAxis) {
            case "x":
                return StaggerAxis.X;
            case "y":
                return StaggerAxis.Y;
            default:
                GD.PushError("Can't detemine a stagger axis type!");
                return null;
        }
    }

    protected StaggerIndex? DetermineStaggerIndex(string staggerIndex) {
        switch (staggerIndex) {
            case "odd":
                return StaggerIndex.Odd;
            case "even":
                return StaggerIndex.Even;
            default:
                GD.PushError("Can't determine a stagger index type!");
                return null;
        }
    }

    protected RenderOrder? DetermineRenderOrder(string renderOrder) {
        switch (renderOrder) {
            case "right-down":
                return RenderOrder.RightDown;
            case "right-up":
                return RenderOrder.RightUp;
            case "left-down":
                return RenderOrder.LeftDown;
            case "left-up":
                return RenderOrder.LeftUp;
            default:
                GD.PushError("Can't determine a map render order type!");
                return null;
        }
    }

    protected WangSetType? DetermineWangSetType(string type) {
        switch (type) {
            case "corner":
                return WangSetType.Corner;
            case "edge":
                return WangSetType.Edge;
            case "mixed":
                return WangSetType.Mixed;
            default:
                GD.PushError("Can't determine a wang set type!");
                return null;
        }
    }

    protected GridOrientation? DetermineGridOrientation(string orientation) {
        switch (orientation) {
            case "orthogonal":
                return GridOrientation.Orthogonal;
            case "isometric":
                return GridOrientation.Isometric;
            default:
                GD.PushError("Can't determine a grid orientation type!");
                return null;
        }
    }

    protected TileObjectsAlignment? DetermineTileObjectAlignment(string alignment) {
        switch (alignment) {
            case "unspecified":
                return TileObjectsAlignment.Uncspecified;
            case "topleft":
                return TileObjectsAlignment.TopLeft;
            case "top":
                return TileObjectsAlignment.Top;
            case "topright":
                return TileObjectsAlignment.TopRight;
            case "left":
                return TileObjectsAlignment.Left;
            case "center":
                return TileObjectsAlignment.Center;
            case "right":
                return TileObjectsAlignment.Right;
            case "bottomleft":
                return TileObjectsAlignment.BottomLeft;
            case "bottom":
                return TileObjectsAlignment.Bottom;
            case "bottomright":
                return TileObjectsAlignment.BottomRight;
            default:
                GD.PushError("Can't determine a tile object alignment type!");
                return null;
        }
    }

    protected Color? ParseColor(string colorHexCode) {
        return new Color("#" + colorHexCode);
    }

    protected ushort[] ParseWangID(Godot.Collections.Array wangIDArray) {
        var wangID = new ushort[8];
        for (int i = 0; i < 8; ++i) {
            ushort? wangIDIndex = ToUShort(wangIDArray[i]);
            if (wangIDIndex == null) {
                GD.PushError("Can't parse a wang id index!");
                return null;
            }
            wangID[i] = wangIDIndex.GetValueOrDefault(); 
        }

        return wangID;
    }

    protected int[] ParseTerrainIndex(Godot.Collections.Array terrainIndexArray) {
        var terrainIndex = new int[4];
        for (int i = 0; i < 4; ++i) {
            int? terrainIndexElement = ToInt(terrainIndexArray[i]);
            if (terrainIndexElement == null) {
                GD.PushError("Can't parse a terrain index element!");
                return null;
            }
            terrainIndex[i] = terrainIndexElement.GetValueOrDefault();
        }

        return terrainIndex;
    }
}
