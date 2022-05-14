using Godot;
using System;
using System.Collections;
using System.Linq;

namespace TiledImporter.Structures
{
    public struct DefaultObjectInfo
    {
        public string name;
        public double? width;
        public double? height;
        public double? rotation;
        public string template;
        public string type;
        public bool? visible;
    }

    public abstract class DefaultObject : Object
    {
        public string name { get; private set; }
        public double width { get; private set; }
        public double height { get; private set; }
        public double rotation { get; private set; }
        public string type { get; private set; }
        public bool visible { get; private set; }

        public DefaultObject(
            int id,
            Point coordinates,
            ObjectType objectType,
            DefaultObjectInfo objectInfo
            ) : base(id, coordinates, objectType)
        {
            var requiredFields = new object[] {
            objectInfo.name,
            objectInfo.width,
            objectInfo.height,
            objectInfo.rotation,
            objectInfo.type,
            objectInfo.visible
        };
            if (requiredFields.Any(field => field == null))
            {
                GD.PushError("Not all of the required standard object fields are initialized!");
            }

            name = objectInfo.name ?? "";
            width = objectInfo.width ?? 0;
            height = objectInfo.height ?? 0;
            rotation = objectInfo.rotation ?? 0;
            type = objectInfo.type ?? "";
            visible = objectInfo.visible ?? false;
        }
    }
}
