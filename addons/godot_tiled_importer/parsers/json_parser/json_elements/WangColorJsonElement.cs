using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;

public class WangColorJsonElement : JsonElement
{
    protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames { 
        get 
        { 
            return new Dictionary<string, ElementaryType>() {
                { "name", ElementaryType.String },
                { "color", ElementaryType.Color },
                { "probability", ElementaryType.Double },
                { "tile", ElementaryType.Int }
            }; 
        }
    }

    protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames {
        get 
        {
            return new Dictionary<string, DataStructure>() {
                { "properties", DataStructure.Property },
            };
        }
    }

    public override object Parse(Godot.Collections.Dictionary elementDictionary) {
        var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
        if (requiredElementaryTypeFields == null) {
            GD.PushError("Dictionary of the required elementary type fields is null!");
            return null;
        }
        var wangColorInfo = new WangColorInfo();
        wangColorInfo.name = (string)requiredElementaryTypeFields["name"];
        wangColorInfo.color = (Color)requiredElementaryTypeFields["color"];
        wangColorInfo.probability = (double)requiredElementaryTypeFields["probability"];
        wangColorInfo.tileID = (int)requiredElementaryTypeFields["tile"];


        var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
        if (optionalArrayFields == null) {
            GD.PushError("Dictionary of the optional array fields is null!");
            return null;
        }
        wangColorInfo.properties = (Property[])optionalArrayFields["propertires"];
    
        return new WangColor(wangColorInfo);
    }
}
