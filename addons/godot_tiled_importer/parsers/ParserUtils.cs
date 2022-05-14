using System;
using System.Linq;
using Godot;
using TiledImporter.Structures;
using TiledImporter.Decoders;

namespace TiledImporter.Parsers
{
    public static class ParserUtils
    {
        public static int? ToInt(object data)
        {
            int? converted = null;
            try
            {
                converted = Convert.ToInt32(data);
            }
            catch (Exception exception)
            {
                if (exception is FormatException || exception is InvalidCastException)
                {
                    GD.PushError("Can't convert to int!");
                }
                else if (exception is OverflowException)
                {
                    GD.PushError("Parsing value is out of the range of int!");
                }
            }

            return converted;
        }

        public static uint? ToUInt(object data)
        {
            uint? converted = null;
            try
            {
                converted = Convert.ToUInt32(data);
            }
            catch (Exception exception)
            {
                if (exception is FormatException || exception is InvalidCastException)
                {
                    GD.PushError("Can't convert to uint!");
                }
                else if (exception is OverflowException)
                {
                    GD.PushError("Parsing value is out of the range of uint!");
                }
            }

            return converted;
        }

        public static double? ToDouble(object data)
        {
            double? converted = null;
            try
            {
                converted = Convert.ToDouble(data);
            }
            catch (Exception exception)
            {
                if (exception is FormatException || exception is InvalidCastException)
                {
                    GD.PushError("Can't convert to dpuble!");
                }
                else if (exception is OverflowException)
                {
                    GD.PushError("Parsing value is out of the range of double!");
                }
            }

            return converted;
        }

        public static ushort? ToUShort(object data)
        {
            ushort? converted = null;
            try
            {
                converted = Convert.ToUInt16(data);
            }
            catch (Exception exception)
            {
                if (exception is FormatException || exception is InvalidCastException)
                {
                    GD.PushError("Can't convert to ushort!");
                }
                else if (exception is OverflowException)
                {
                    GD.PushError("Parsing value is out of the range of ushort!");
                }
            }

            return converted;
        }

        public static bool? ToBool(object data)
        {
            bool? converted = null;
            try
            {
                converted = Convert.ToBoolean(data);
            }
            catch (Exception exception)
            {
                if (exception is FormatException || exception is InvalidCastException)
                {
                    GD.PushError("Can't convert to bool!");
                }
            }

            return converted;
        }

        public static string ToString(object data)
        {
            return Convert.ToString(data);
        }

        public static MapOrientation? DetermineMapOrientation(string orientation)
        {
            switch (orientation)
            {
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

        public static LayerType? DetermineLayerType(string type)
        {
            switch (type)
            {
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

        public static Compression? DetermineCompression(string compression)
        {
            switch (compression)
            {
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

        public static Encoding? DetermineEncoding(string encoding)
        {
            switch (encoding)
            {
                case "csv":
                    return Encoding.CSV;
                case "base64":
                    return Encoding.Base64;
                default:
                    GD.PushError("Can't determine an encoding type of the level data!");
                    return null;
            }
        }

        public static DrawOrder? DetermineDrawOrder(string drawOrder)
        {
            switch (drawOrder)
            {
                case "topdown":
                    return DrawOrder.TopDown;
                case "index":
                    return DrawOrder.Index;
                default:
                    GD.PushError("Can't determine an draw order of the object layer!");
                    return null;
            }
        }

        public static PropertyType? DeterminePropertyType(string type)
        {
            switch (type)
            {
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

        public static HorizontalAlignment? DetermineHorizontalAlignment(string alignment)
        {
            switch (alignment)
            {
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

        public static VerticalAlignment? DetermineVerticalAlignment(string alignment)
        {
            switch (alignment)
            {
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

        public static StaggerAxis? DetermineStaggerAxis(string staggerAxis)
        {
            switch (staggerAxis)
            {
                case "x":
                    return StaggerAxis.X;
                case "y":
                    return StaggerAxis.Y;
                default:
                    GD.PushError("Can't detemine a stagger axis type!");
                    return null;
            }
        }

        public static StaggerIndex? DetermineStaggerIndex(string staggerIndex)
        {
            switch (staggerIndex)
            {
                case "odd":
                    return StaggerIndex.Odd;
                case "even":
                    return StaggerIndex.Even;
                default:
                    GD.PushError("Can't determine a stagger index type!");
                    return null;
            }
        }

        public static RenderOrder? DetermineRenderOrder(string renderOrder)
        {
            switch (renderOrder)
            {
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

        public static WangSetType? DetermineWangSetType(string type)
        {
            switch (type)
            {
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

        public static GridOrientation? DetermineGridOrientation(string orientation)
        {
            switch (orientation)
            {
                case "orthogonal":
                    return GridOrientation.Orthogonal;
                case "isometric":
                    return GridOrientation.Isometric;
                default:
                    GD.PushError("Can't determine a grid orientation type!");
                    return null;
            }
        }

        public static TileObjectsAlignment? DetermineTileObjectsAlignment(string alignment)
        {
            switch (alignment)
            {
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

        public static Color? ParseColor(string colorHexCode)
        {
            if (colorHexCode == null)
            {
                GD.PushError("Parsing color hex code string is null!");
                return null;
            }

            return new Color(colorHexCode);
        }

        public static ushort[] ParseWangID(Godot.Collections.Array wangIDArray)
        {
            var wangID = new ushort[8];
            for (int i = 0; i < 8; ++i)
            {
                ushort? wangIDIndex = ToUShort(wangIDArray[i]);
                if (wangIDIndex == null)
                {
                    GD.PushError("Can't parse a wang id index!");
                    return null;
                }
                wangID[i] = wangIDIndex.GetValueOrDefault();
            }

            return wangID;
        }

        public static int[] ParseTerrainIndex(Godot.Collections.Array terrainIndexArray)
        {
            var terrainIndex = new int[4];
            for (int i = 0; i < 4; ++i)
            {
                int? terrainIndexElement = ToInt(terrainIndexArray[i]);
                if (terrainIndexElement == null)
                {
                    GD.PushError("Can't parse a terrain index element!");
                    return null;
                }
                terrainIndex[i] = terrainIndexElement.GetValueOrDefault();
            }

            return terrainIndex;
        }

        public static TileLayerData ParseLayerData(object data, int layerWidth, int layerHeight, Encoding encoding, Compression compression = Compression.None)
        {
            switch (encoding)
            {
                case Encoding.CSV:
                    if (compression != Compression.None)
                    {
                        GD.PushError("CSV format can't be compressed!");
                    }
                    object[] dataArrayElements = (data as Godot.Collections.Array).Cast<object>().ToArray();
                    uint[] tileIDs = Array.ConvertAll(dataArrayElements, element => ParserUtils.ToUInt(element) ?? 0u);
                    return new CSVDecoder().Decode(tileIDs, layerWidth, layerHeight);
                case Encoding.Base64:
                    return new Base64Decoder().Decode(ParserUtils.ToString(data), layerWidth, layerHeight, compression);
                default:
                    GD.PushError("Not determined encoding!");
                    return null;
            }
        }
    }
}
