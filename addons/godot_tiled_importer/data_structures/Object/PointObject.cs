using Godot;
using System;

public enum PointObjectType {
    Polygon, Polyline
}

public class PointObject : Object
{
    public Point[] points { get; private set; }
    public PointObjectType pointObjectType { get; private set; }

    public PointObject (ObjectInfo objectInfo, Point[] points, PointObjectType pointObjectType) : base(objectInfo) {
        if (points == null) {
            GD.PushError("Points of the point object are not initialized!");
            return;
        }
        this.points = points;
        this.pointObjectType = pointObjectType;
    }
}
