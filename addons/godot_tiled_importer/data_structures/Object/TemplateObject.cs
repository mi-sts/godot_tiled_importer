using Godot;
using System;
using System.Collections;
using System.Linq;

namespace TiledImporter.Structures
{
    public class TemplateObject : Object
    {
        public string template { get; private set; }

        public TemplateObject(int id, Point coordinates, ObjectType type, string template) : base(id, coordinates, type)
        {
            if (template == null)
            {
                GD.PushError("Template field of the template object is not initialized!");
            }
            this.template = template;
        }
    }
}
