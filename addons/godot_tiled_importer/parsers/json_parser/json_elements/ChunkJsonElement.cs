using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class ChunkInfoJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "width", ElementaryType.Int },
                { "height", ElementaryType.Int },
                { "x", ElementaryType.Int },
                { "y", ElementaryType.Int },
                { "data", ElementaryType.String }
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
            int xCoordinate = (int)requiredElementaryTypeFields["x"];
            int yCoordinate = (int)requiredElementaryTypeFields["y"];
            var position = new IntPoint(xCoordinate, yCoordinate);
            string data = (string)requiredElementaryTypeFields["data"];
            TileLayerData parsedData = ParserUtils.ParseLayerData(data, width, height, Encoding.CSV);
            if (parsedData == null)
            {
                GD.PushError("Parsed chunk data is null!");
                return null;
            }

            return new Chunk(parsedData, width, height, position);
        }
    }
}