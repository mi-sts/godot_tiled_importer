using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class WangTileJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "tileid", ElementaryType.Int },
                { "wangid", ElementaryType.Object },
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
            int tileID = (int)requiredElementaryTypeFields["tileid"];

            var wangIDArray = requiredElementaryTypeFields["wangid"] as Godot.Collections.Array;
            if (wangIDArray == null)
            {
                GD.PushError("Converted wang id array of the wang tile is null!");
                return null;
            }
            ushort[] wangID = ParserUtils.ParseWangID(wangIDArray);
            if (wangID == null)
            {
                GD.PushError("Parsed wang id is null!");
                return null;
            }

            return new WangTile(tileID, wangID);
        }
    }
}
