using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum PointObjectType
    {
        Polygon, Polyline
    }

    public class PointObject : DefaultObject
    {
        public Point[] points { get; private set; }
        public PointObjectType pointObjectType { get; private set; }

        public PointObject(
            int id,
            Point position,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            PointObjectType pointObjectType,
            Point[] points
            ) : base(id, position, type, objectInfo)
        {
            if (points == null)
            {
                GD.PushError("Points of the point object are not initialized!");
            }
            this.points = points ?? new Point[0];
            this.pointObjectType = pointObjectType;
        }
    }
}
