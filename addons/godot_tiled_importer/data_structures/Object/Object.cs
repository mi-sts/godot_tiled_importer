using Godot;
using System;
using System.Linq;

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
}

public class Object 
{
    public string name { get; private set; }
    public int? id { get; private set; } 
    public Point coordinates { get; private set; }
    public double width { get; private set; }
    public double height { get; private set; }
    public double rotation { get; private set; }
    public string template { get; private set; }
    public string type { get; private set; }
    public bool visible { get; private set; }

    public Object(ObjectInfo objectInfo) {
        var requiredParameters = new object[] { 
            objectInfo.name, 
            objectInfo.id,
            objectInfo.coordinates, 
            objectInfo.width, 
            objectInfo.height,
            objectInfo.rotation,
            objectInfo.template,
            objectInfo.type,
            objectInfo.visible
        };
        if (requiredParameters.Any(argument => argument == null)) {
            GD.PushError("Not all of the required tile parameters are initialized!");
            return;
        }

        name = objectInfo.name;
        id = objectInfo.id;
        coordinates = objectInfo.coordinates;
        width = objectInfo.width ?? 0;
        height = objectInfo.height ?? 0;
        rotation = objectInfo.rotation ?? 0;
        template = objectInfo.template ?? "";
        type = objectInfo.type ?? "";
        visible = objectInfo.visible ?? false;
    }
}

