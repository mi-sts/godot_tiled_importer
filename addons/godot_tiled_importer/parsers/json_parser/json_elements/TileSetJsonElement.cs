using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class TileSetJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "name", ElementaryType.String },
                { "firstgid", ElementaryType.UInt },
                { "margin", ElementaryType.Int },
                { "spacing", ElementaryType.Int },
                { "tilecount", ElementaryType.Int },
                { "tileheight", ElementaryType.Int },
                { "tilewidth", ElementaryType.Int },
                { "columns", ElementaryType.Int }
            };
            }
        }

        protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "image", ElementaryType.String },
                { "imageheight", ElementaryType.Int },
                { "imagewidth", ElementaryType.Int },
                { "objectalignment", ElementaryType.TileObjectsAlignment },
                { "transparentcolor", ElementaryType.Color }
                };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                { "tileoffset", DataStructure.IntPoint },
                { "transformations", DataStructure.Transfomations },
            };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                { "properties", DataStructure.Property },
                { "wangsets", DataStructure.WangSet },
                { "terrains", DataStructure.Terrain },
                { "tiles", DataStructure.Tile }
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
            var tileSetInfo = new TileSetInfo();
            tileSetInfo.name = (string)requiredElementaryTypeFields["name"];
            tileSetInfo.firstGID = (uint)requiredElementaryTypeFields["firstgid"];
            tileSetInfo.margin = (int)requiredElementaryTypeFields["margin"];
            tileSetInfo.spacing = (int)requiredElementaryTypeFields["spacing"];
            tileSetInfo.tileCount = (int)requiredElementaryTypeFields["tilecount"];
            tileSetInfo.tileHeight = (int)requiredElementaryTypeFields["tileheight"];
            tileSetInfo.tileWidth = (int)requiredElementaryTypeFields["tilewidth"];
            tileSetInfo.columns = (int)requiredElementaryTypeFields["columns"];

            var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
            if (optionalElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the optional elementary type fields is null!");
                return null;
            }
            tileSetInfo.objectsAlignment = (TileObjectsAlignment?)optionalElementaryTypeFields["objectalignment"];
            tileSetInfo.transparentColor = (Color?)optionalElementaryTypeFields["transparentcolor"];


            var optionalFields = ParseOptionalFields(elementDictionary);
            if (optionalFields == null)
            {
                GD.PushError("Dictionary of the optional fields is null!");
                return null;
            }
            tileSetInfo.tileOffset = (IntPoint?)optionalFields["tileoffset"];
            tileSetInfo.transfromations = (Transfromations?)optionalFields["transformations"];


            var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
            if (optionalArrayFields == null)
            {
                GD.PushError("Dictionary of the optional array fields is null!");
                return null;
            }
            if (optionalElementaryTypeFields["image"] != null)
            {
                tileSetInfo.type = TileSetType.SingleImageTileSet;
                tileSetInfo.imageHeight = (int?)optionalElementaryTypeFields["imageheight"];
                tileSetInfo.imageWidth = (int?)optionalElementaryTypeFields["imagewidth"];
                tileSetInfo.image = (string)optionalElementaryTypeFields["image"];
                var requiredSingleImageTileSetFields = new object[] {
                tileSetInfo.imageWidth,
                tileSetInfo.imageHeight,
                tileSetInfo.image
            };
                if (requiredSingleImageTileSetFields.Any(field => field == null))
                {
                    GD.PushError("One of the required image tile set fields is null!");
                    return null;
                }
            }
            else
            {
                tileSetInfo.type = TileSetType.MultupleImagesTileSet;
                tileSetInfo.tiles = Array.ConvertAll(optionalArrayFields["tiles"], tile => (Tile)tile);
            }
            object[] boxedProperties = optionalArrayFields["properties"];
            object[] boxedWangSets = optionalArrayFields["wangsets"];
            object[] boxedTerrains = optionalArrayFields["terrains"];
            if (boxedProperties != null)
                tileSetInfo.properties = Array.ConvertAll(boxedProperties, property => (Property)property);
            if (boxedWangSets != null)
                tileSetInfo.wangSets = Array.ConvertAll(boxedWangSets, wangSet => (WangSet)wangSet);
            if (boxedTerrains != null)
            {
                tileSetInfo.terrains = Array.ConvertAll(boxedTerrains, terrain => (Terrain)terrain);
            }

            return new TiledImporter.Structures.TileSet(tileSetInfo);
        }
    }
}
