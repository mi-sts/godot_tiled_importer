using Godot;
using System;

namespace TiledImporter.Structures
{
    public class DefaultObject : StandardObject
    {
        public uint gID { get; private set; }
        public Property[] properties { get; private set; }

        public DefaultObject(
            int id,
            Point position,
            ObjectType type,
            StandardObjectInfo objectInfo,
            uint gID,
            Property[] properties
            ) : base(id, position, type, objectInfo)
        {
            if (properties == null)
            {
                GD.PushError("Properties of the default object are not initialized!");
            }
            this.gID = gID;
            this.properties = properties ?? new Property[0];
        }
    }
}
