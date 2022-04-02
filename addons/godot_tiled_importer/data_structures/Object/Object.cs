using Godot;
using System;
using System.Linq;

public enum ObjectType {
    DefaultObject, PointObject, ShapeObject, TextObject
}

public struct ObjectInfo {
    public string name;
    public int? id;
    public Point coordinates;
    public double? width;
    public double? height;
    public double? rotation;
    public string template;
    public string type;
    public bool? visible;
    public ObjectType? objectType;
}

public class Object {
    public string name { get; private set; }
    public int id { get; private set; }
    public Point coordinates { get; private set; }
    public double width { get; private set; }
    public double height { get; private set; }
    public double rotation { get; private set; }
    public string template { get; private set; } // Reference to a template file, in case object is a template instance (optional).
    public string type { get; private set; }
    public bool visible { get; private set; }
    public ObjectType objectType { get; private set; }

    public Object(ObjectInfo objectInfo) {
        var requiredFields = new object[] {
            objectInfo.name,
            objectInfo.id,
            objectInfo.coordinates,
            objectInfo.width,
            objectInfo.height,
            objectInfo.rotation,
            objectInfo.type,
            objectInfo.visible,
            objectInfo.objectType
        };
        if (requiredFields.Any(field => field == null)) {
            GD.PushError("Not all of the required tile parameters are initialized!");
        }

        name = objectInfo.name ?? "";
        id = objectInfo.id ?? -1;
        coordinates = objectInfo.coordinates;
        width = objectInfo.width ?? 0;
        height = objectInfo.height ?? 0;
        rotation = objectInfo.rotation ?? 0;
        type = objectInfo.type ?? "";
        visible = objectInfo.visible ?? false;
        objectType = objectInfo.objectType ?? ObjectType.DefaultObject;

        template = objectInfo.template;
    }
}

