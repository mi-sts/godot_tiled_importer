using Godot;
using System;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public enum Compression
    {
        None, ZLib, GZip, ZStd
    }
    public enum Encoding
    {
        CSV, Base64,
    }

    public enum DataStructure
    {
        Layer,
        Object,
        Chunk,
        Frame,
        Grid,
        Map,
        Property,
        Terrain,
        Text,
        Tile,
        TileSet,
        Transfomations,
        WangColor,
        WangSet,
        WangTile,
        Point,
        IntPoint
    }

    public enum ElementaryType
    {
        Int,
        UInt,
        Bool,
        UShort,
        Double,
        String,
        Object,
        MapOrientation,
        LayerType,
        Compression,
        Encoding,
        DrawOrder,
        PropertyType,
        HorizontalAlignment,
        VerticalAlignment,
        StaggerAxis,
        StaggerIndex,
        RenderOrder,
        WangSetType,
        GridOrientation,
        TileObjectsAlignment,
        Color,
        WangID,
        TerrainIndex
    }

    public abstract class Parser
    {
        public abstract Map Parse(string data);
    }
}
