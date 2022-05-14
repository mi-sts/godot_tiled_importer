using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class WangSetJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "name", ElementaryType.String },
                { "tile", ElementaryType.Int },
            };
            }
        }

        protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "type", ElementaryType.WangSetType }
            };
            }
        }


        protected override Dictionary<string, DataStructure> RequiredArrayFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                { "wangtiles", DataStructure.WangTile }
            };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                { "properties", DataStructure.Property },
                { "colors", DataStructure.WangColor }
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
            var wangSetInfo = new WangSetInfo();
            wangSetInfo.name = (string)requiredElementaryTypeFields["name"];
            wangSetInfo.tileID = (int)requiredElementaryTypeFields["tile"];


            var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
            if (requiredElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the optional elementary type fields is null!");
                return null;
            }
            wangSetInfo.type = (WangSetType?)optionalElementaryTypeFields["type"];

            var requiredArrayFields = ParseRequiredArrayFields(elementDictionary);
            if (requiredArrayFields == null)
            {
                GD.PushError("Dictionary of the required array fields is null!");
                return null;
            }
            wangSetInfo.wangTiles = Array.ConvertAll(requiredArrayFields["wangtiles"], wangTile => (WangTile)wangTile);


            var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
            if (optionalArrayFields == null)
            {
                GD.PushError("Dictionary of the optional array fields is null!");
                return null;
            }
            object[] boxedProperties = optionalArrayFields["properties"];
            object[] boxedColors = optionalArrayFields["colors"];
            if (boxedProperties != null)
                wangSetInfo.properties = Array.ConvertAll(boxedProperties, property => (Property)property);
            if (boxedColors != null)
                wangSetInfo.colors = Array.ConvertAll(optionalArrayFields["colors"], color => (WangColor)color);

            return new WangSet(wangSetInfo);
        }
    }
}
