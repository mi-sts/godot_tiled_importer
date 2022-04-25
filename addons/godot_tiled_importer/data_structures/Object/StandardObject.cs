using Godot;
using System;
using System.Collections;
using System.Linq;

namespace TiledImporter.Structures
{
    public struct StandardObjectInfo
    {
        public string name;
        public double? width;
        public double? height;
        public double? rotation;
        public string template;
        public string type;
        public bool? visible;
    }

    public abstract class StandardObject : Object
    {
        public string name { get; private set; } // (for non-template objects).
        public double width { get; private set; } // (for non-template objects).
        public double height { get; private set; } // (for non-template objects).
        public double rotation { get; private set; } // (for non-template objects).
        public string type { get; private set; } // (for non-template objects).
        public bool visible { get; private set; } // (for non-template objects).

        public StandardObject(
            int id,
            Point coordinates,
            ObjectType objectType,
            StandardObjectInfo objectInfo
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
