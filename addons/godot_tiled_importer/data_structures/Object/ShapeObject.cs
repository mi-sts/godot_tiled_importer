using Godot;
using System;

public enum ShapeObjectType {
    Ellipse, Rectangle, Point
}

public class ShapeObject : Object {
    public ShapeObjectType shapeType { get; private set; }

    public ShapeObject(ObjectInfo objectInfo, ShapeObjectType shapeType) : base(objectInfo) {
        this.shapeType = shapeType;
    }
}
