using Godot;
using System;
using System.Collections;
using System.Linq;

namespace TiledImporter.Structures
{
    public enum ObjectType
    {
        TemplateObject, DefaultObject, PointObject, ShapeObject, TextObject
    }

    public abstract class Object
    {
        public int id { get; private set; }
        public Point coordinates { get; private set; }
        public ObjectType objectType { get; private set; }

        public Object(int id, Point coordinates, ObjectType objectType)
        {
            this.id = id;
            this.coordinates = coordinates;
            this.objectType = objectType;
        }
    }
}
