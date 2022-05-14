using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class TransformationsJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "hflip", ElementaryType.Bool },
                { "vflip", ElementaryType.Bool },
                { "rotate", ElementaryType.Bool },
                { "preferuntransformed", ElementaryType.Bool }
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
            bool hflip = (bool)requiredElementaryTypeFields["hflip"];
            bool vflip = (bool)requiredElementaryTypeFields["vflip"];
            bool rotate = (bool)requiredElementaryTypeFields["rotate"];
            bool preferUntransformed = (bool)requiredElementaryTypeFields["preferuntransformed"];

            return new Transfromations(hflip, vflip, rotate, preferUntransformed);
        }
    }
}
