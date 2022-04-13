using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;

public class TileSetJsonElement : JsonElement
{
    protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames { 
        get
        { 
            return new Dictionary<string, ElementaryType>() {
                { "name", ElementaryType.String },
                { "firstgid", ElementaryType.Int },
                { "image", ElementaryType.String },
                { "imageheight", ElementaryType.Int },
                { "imagewidth", ElementaryType.Int },
                { "margin", ElementaryType.Int },
                { "spacing", ElementaryType.Int },
                { "tilecount", ElementaryType.Int },
                { "tileheight", ElementaryType.Int },
                { "tilewidth", ElementaryType.Int }
            }; 
        }
    }

    protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames { 
        get
        { 
            return new Dictionary<string, ElementaryType>() {
                { "objectalignment", ElementaryType.TileObjectsAlignment },
                { "transparentcolor", ElementaryType.Color }
            }; 
        }
    }

    protected override Dictionary<string, DataStructure> OptionalFieldsNames { 
        get 
        { 
            return new Dictionary<string, DataStructure>() {
                { "tileoffset", DataStructure.IntPoint },
                { "transformations", DataStructure.Transfomations },
            }; 
        }
    }

    protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames {
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

    public override object Parse(Godot.Collections.Dictionary elementDictionary) {
        var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
        if (requiredElementaryTypeFields == null) {
            GD.PushError("Dictionary of the required elementary type fields is null!");
            return null;
        }
        var tileSetInfo = new TileSetInfo();
        tileSetInfo.name = (string)requiredElementaryTypeFields["name"];
        tileSetInfo.firstGID = (int)requiredElementaryTypeFields["firstgid"];
        tileSetInfo.image = (string)requiredElementaryTypeFields["image"];
        tileSetInfo.imageHeight = (int)requiredElementaryTypeFields["imageheight"];
        tileSetInfo.imageWidth = (int)requiredElementaryTypeFields["imagewidth"];
        tileSetInfo.margin = (int)requiredElementaryTypeFields["margin"];
        tileSetInfo.spacing = (int)requiredElementaryTypeFields["spacing"];
        tileSetInfo.tileCount = (int)requiredElementaryTypeFields["tilecount"];
        tileSetInfo.tileHeight = (int)requiredElementaryTypeFields["tileheight"];
        tileSetInfo.tileWidth = (int)requiredElementaryTypeFields["tilewidth"];

        var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
        if (optionalElementaryTypeFields == null) {
            GD.PushError("Dictionary of the optional elementary type fields is null!");
            return null;
        }
        tileSetInfo.objectsAlignment = (TileObjectsAlignment?)optionalElementaryTypeFields["objectalignment"];
        tileSetInfo.transparentColor = (Color?)optionalElementaryTypeFields["transparentcolor"];


        var optionalFields = ParseOptionalFields(elementDictionary);
        if (optionalFields == null) {
            GD.PushError("Dictionary of the optional fields is null!");
            return null;
        }
        tileSetInfo.tileOffset = (IntPoint?)optionalFields["tileoffset"];
        tileSetInfo.transfromations = (Transfromations?)optionalFields["transformations"];


        var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
        if (optionalArrayFields == null) {
            GD.PushError("Dictionary of the optional array fields is null!");
            return null;
        }
        tileSetInfo.properties = (Property[])optionalArrayFields["properties"];
        tileSetInfo.wangSets = (WangSet[])optionalArrayFields["wangsets"];
        tileSetInfo.terrains = (Terrain[])optionalArrayFields["terrains"];
        object[] boxedTiles = optionalArrayFields["tiles"];
        if (boxedTiles != null) {
            tileSetInfo.tiles = Array.ConvertAll(boxedTiles, tile => (Tile)tile);
        }

        return new TileSet(tileSetInfo);
    }
}