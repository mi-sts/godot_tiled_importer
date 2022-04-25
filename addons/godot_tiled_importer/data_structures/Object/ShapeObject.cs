using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum ShapeObjectType
    {
        Ellipse, Rectangle, Point
    }

    public class ShapeObject : StandardObject
    {
        public ShapeObjectType shapeType { get; private set; }

        public ShapeObject(
            int id,
            Point coordinates,
            ObjectType type,
            StandardObjectInfo objectInfo,
            ShapeObjectType shapeType
            ) : base(id, coordinates, type, objectInfo)
        {
            this.shapeType = shapeType;
        }
    }
}
