using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class GridJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "width", ElementaryType.Int },
                { "height", ElementaryType.Int }
            };
            }
        }

        protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "orientation", ElementaryType.GridOrientation }
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
            int width = (int)requiredElementaryTypeFields["width"];
            int height = (int)requiredElementaryTypeFields["height"];


            var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
            if (optionalElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the optional elementary type fields is null!");
                return null;
            }
            GridOrientation orientation = (GridOrientation?)optionalElementaryTypeFields["orientation"] ?? GridOrientation.Orthogonal;

            return new Grid(width, height, orientation);
        }
    }
}
