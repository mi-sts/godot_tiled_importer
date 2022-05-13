using Godot;
using System;

namespace TiledImporter.Structures
{
    public class TextObject : DefaultObject
    {
        public Text text { get; private set; }

        public TextObject(
            int id,
            Point coordinates,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            Text text) : base(id, coordinates, type, objectInfo)
        {
            this.text = text;
        }
    }
}
