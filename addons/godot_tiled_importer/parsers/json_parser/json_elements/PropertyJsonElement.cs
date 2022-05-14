using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class PropertyJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "name", ElementaryType.String },
                { "type", ElementaryType.PropertyType },
                { "value", ElementaryType.Object }
            };
            }
        }
        public override object Parse(Godot.Collections.Dictionary elementDictionary)
        {
            var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
            if (requiredElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the required elementary type fields is null!");
                return null;
            }
            string name = (string)requiredElementaryTypeFields["name"];
            object value = requiredElementaryTypeFields["value"];
            PropertyType type = (PropertyType)requiredElementaryTypeFields["type"];

            return new Property(name, value, type);
        }
    }
}
