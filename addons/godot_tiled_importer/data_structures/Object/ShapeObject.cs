using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum ShapeObjectType
    {
        Ellipse, Rectangle, Point
    }

    public class ShapeObject : DefaultObject
    {
        public ShapeObjectType shapeType { get; private set; }

        public ShapeObject(
            int id,
            Point coordinates,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            ShapeObjectType shapeType
            ) : base(id, coordinates, type, objectInfo)
        {
            this.shapeType = shapeType;
        }
    }
}
