using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class TileJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "id", ElementaryType.Int }
            };
            }
        }

        protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "imageheight", ElementaryType.Int },
                { "imagewidth", ElementaryType.Int },
                { "type", ElementaryType.String },
                { "image", ElementaryType.String },
                { "probability", ElementaryType.Double }
            };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                { "objectgroup", DataStructure.Layer },
                { "grid", DataStructure.Grid },
                { "terrain", DataStructure.Terrain}
            };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                { "properties", DataStructure.Property },
                { "animation", DataStructure.Frame }
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
            var tileInfo = new TileInfo();
            tileInfo.id = (int)requiredElementaryTypeFields["id"];


            var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
            if (optionalElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the optional elementary type fields is null!");
                return null;
            }
            tileInfo.imageWidth = (int?)optionalElementaryTypeFields["imagewidth"];
            tileInfo.imageHeight = (int?)optionalElementaryTypeFields["imageheight"];
            tileInfo.type = (string)optionalElementaryTypeFields["type"];
            tileInfo.image = (string)optionalElementaryTypeFields["image"];
            tileInfo.probability = (double?)optionalElementaryTypeFields["probability"];


            var optionalFields = ParseOptionalFields(elementDictionary);
            if (optionalFields == null)
            {
                GD.PushError("Dictionary of the optional fields is null!");
                return null;
            }
            tileInfo.objectGroup = (Layer)optionalFields["objectgroup"];
            tileInfo.grid = (Grid?)optionalFields["grid"];
            var terrainIndexArray = optionalFields["terrain"] as Godot.Collections.Array;
            if (terrainIndexArray != null)
                tileInfo.terrainIndex = ParserUtils.ParseTerrainIndex(terrainIndexArray);


            var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
            if (optionalArrayFields == null)
            {
                GD.PushError("Dictionary of the optional array fields is null!");
                return null;
            }
            object[] boxedProperties = optionalArrayFields["properties"];
            object[] boxedAnimation = optionalArrayFields["animation"];
            if (boxedProperties != null)
                tileInfo.properties = Array.ConvertAll(boxedProperties, property => (Property)property);
            if (boxedAnimation != null)
            {
                tileInfo.animation = Array.ConvertAll(boxedAnimation, frame => (Frame)frame);
            }

            return new Tile(tileInfo);
        }
    }
}
