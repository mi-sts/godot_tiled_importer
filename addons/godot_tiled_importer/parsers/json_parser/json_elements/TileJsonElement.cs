using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;

public class TileJsonElement : JsonElement
{
    protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames { 
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
    
    protected override Dictionary<string, DataStructure> OptionalFieldsNames { 
        get 
        { 
            return new Dictionary<string, DataStructure>() {
                { "objectgroup", DataStructure.Layer },
                { "grid", DataStructure.Grid },
                { "terrain", DataStructure.Object }
            };
        }
    }

    protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames {
        get 
        {
            return new Dictionary<string, DataStructure>() {
                { "properties", DataStructure.Property },
                { "animation", DataStructure.Frame }
            };
        }
    }

    public override object Parse(Godot.Collections.Dictionary elementDictionary) {
        var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
        if (requiredElementaryTypeFields == null) {
            GD.PushError("Dictionary of the required elementary type fields is null!");
            return null;
        }
        var tileInfo = new TileInfo();
        tileInfo.imageWidth = (int)requiredElementaryTypeFields["imagewidth"];
        tileInfo.imageHeight = (int)requiredElementaryTypeFields["imageheight"];
        tileInfo.type = (string)requiredElementaryTypeFields["type"];
        tileInfo.image = (string)requiredElementaryTypeFields["image"];
        tileInfo.probability = (double)requiredElementaryTypeFields["probability"];


        var optionalFields = ParseOptionalFields(elementDictionary);
        if (optionalFields == null) {
            GD.PushError("Dictionary of the optional fields is null!");
            return null;
        }
        tileInfo.objectGroup = (Layer)optionalFields["objectgroup"];
        tileInfo.grid = (Grid?)optionalFields["grid"];
        var terrainIndexArray = optionalFields["terrain"] as Godot.Collections.Array;
        if (terrainIndexArray != null)
            tileInfo.terrainIndex = ParserUtils.ParseTerrainIndex(terrainIndexArray);


        var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
        if (optionalArrayFields == null) {
            GD.PushError("Dictionary of the optional array fields is null!");
            return null;
        }
        tileInfo.properties = (Property[])optionalArrayFields["properties"];
        object[] boxedAnimation = optionalArrayFields["animation"];
        if (boxedAnimation != null) {
            tileInfo.animation = Array.ConvertAll(boxedAnimation, frame => (Frame)frame);
        }

        return new Tile(tileInfo);
    }
}
