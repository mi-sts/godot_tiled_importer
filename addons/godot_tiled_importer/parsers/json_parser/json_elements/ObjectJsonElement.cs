using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;

public class ObjectJsonElement  : JsonElement
{   
    protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames { 
        get 
        { 
            return new Dictionary<string, ElementaryType>() {
                { "name", ElementaryType.String },
                { "id", ElementaryType.Int },
                { "x", ElementaryType.Double },
                { "y", ElementaryType.Double },
                { "width", ElementaryType.Double },
                { "height", ElementaryType.Double },
                { "rotation", ElementaryType.Double },
                { "type", ElementaryType.String },
                { "visible", ElementaryType.Bool }            
            }; 
        }
    }

    protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames { 
        get 
        { 
            return new Dictionary<string, ElementaryType>() {
                { "template", ElementaryType.String },

                // Shape object fields.
                { "ellipse", ElementaryType.Bool },
                { "point", ElementaryType.Bool },

                // Default object fields.
                { "gid", ElementaryType.UInt }
            }; 
        }
    }

    protected override Dictionary<string, DataStructure> OptionalFieldsNames {
        get 
        {
            return new Dictionary<string, DataStructure>() {
                //Text object fields.
                { "text", DataStructure.Text }
            };   
        }
    }

    protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames {
        get 
        {
            return new Dictionary<string, DataStructure>() {
                // Shape object fields.
                { "polygon", DataStructure.Point },
                { "polyline", DataStructure.Point },

                // Default object fields.
                { "properties", DataStructure.Property }
            };
        }
    }

    public override object Parse(Godot.Collections.Dictionary elementDictionary) {
        var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
        if (requiredElementaryTypeFields == null) {
            GD.PushError("Dictionary of the required elementary type fields is null!");
            return null;
        }
        var objectInfo = new ObjectInfo();
        objectInfo.name = (string)requiredElementaryTypeFields["name"];
        objectInfo.id = (int)requiredElementaryTypeFields["id"];
        double xCoordinate = (double)requiredElementaryTypeFields["x"];
        double yCoordinate = (double)requiredElementaryTypeFields["y"];
        objectInfo.coordinates = new Point(xCoordinate, yCoordinate);
        objectInfo.width = (double)requiredElementaryTypeFields["width"];
        objectInfo.height = (double)requiredElementaryTypeFields["height"];
        objectInfo.rotation = (double)requiredElementaryTypeFields["rotation"];
        objectInfo.type = (string)requiredElementaryTypeFields["type"];
        objectInfo.visible = (bool)requiredElementaryTypeFields["visible"];

        var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
        if (optionalElementaryTypeFields == null) {
            GD.PushError("Dictionary of the optional elementary type fields is null!");
            return null;
        }
        var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
        if (optionalArrayFields == null) {
            GD.PushError("Dictionary of the optional array fields is null!");
            return null;
        }
        var optionalFields = ParseOptionalFields(elementDictionary);
        if (optionalFields == null) {
            GD.PushError("Dictionary of the optional fields is null!");
            return null;
        }
        objectInfo.template = (string)optionalElementaryTypeFields["template"];
        if (optionalElementaryTypeFields["ellipse"] as bool? == true || 
            optionalElementaryTypeFields["point"] as bool? == true) {
            objectInfo.objectType = ObjectType.ShapeObject;
        } else if (optionalArrayFields["polygon"] != null || optionalArrayFields["polyline"] != null) {
            objectInfo.objectType = ObjectType.PointObject;
        } else if (optionalFields["text"] != null) {
            objectInfo.objectType = ObjectType.DefaultObject;
        } else {
            objectInfo.objectType = ObjectType.ShapeObject;
        }

        switch (objectInfo.objectType) {
            case ObjectType.DefaultObject:
                return FillToDefaultObject(objectInfo, optionalElementaryTypeFields, optionalArrayFields);
            case ObjectType.ShapeObject:
                return FillToShapeObject(objectInfo, elementDictionary);
            case ObjectType.PointObject:
                return FillToPointObject(objectInfo, optionalArrayFields, elementDictionary);
            case ObjectType.TextObject:
                return FillToTextObject(objectInfo, optionalFields);
            default:
                GD.PushError("Not determined object type!");
                return null;
        }
    }

    private PointObject FillToPointObject(
        ObjectInfo objectInfo,
        Dictionary<string, object[]> optionalArrayFields,
        Godot.Collections.Dictionary elementDictionary
        ) {
        PointObjectType? pointObjectType = DeterminePointObjectType(elementDictionary);
        if (pointObjectType == null) {
            GD.PushError("Point object type is not determined!");
            return null;
        }

        object[] boxedPoints = null;
        switch (pointObjectType) {
            case PointObjectType.Polygon:
                boxedPoints = optionalArrayFields["polygon"];
                if (boxedPoints == null) {
                    GD.PushError("Parsed polygon array of the point object is null!");
                    return null;
                }
                break;
            case PointObjectType.Polyline:
                boxedPoints = optionalArrayFields["polyline"];
                if (boxedPoints == null) {
                    GD.PushError("Parsed polyline array of the point object is null!");
                    return null;
                }
                break;
        }

        if (boxedPoints == null) {
            GD.PushError("Parsed points of the point object is null!");
            return null;
        }
        var points = Array.ConvertAll(boxedPoints, point => (Point)point);
        
        return new PointObject(objectInfo, pointObjectType.GetValueOrDefault(), points);
    }

    private PointObjectType? DeterminePointObjectType(Godot.Collections.Dictionary elementDictionary) {
        if (elementDictionary.Contains("polyline")) {
            return PointObjectType.Polyline;
        } else if (elementDictionary.Contains("polygon")) {
            return PointObjectType.Polygon;
        }
        GD.PushError("Not determined a point object type!");
        return null;
    }

    private ShapeObject FillToShapeObject(ObjectInfo objectInfo, Godot.Collections.Dictionary elementDictionary) {
        ShapeObjectType? shapeObjectType = DetermineShapeObjectType(elementDictionary);
        if (shapeObjectType == null) {
            GD.PushError("Shape object type is not determined!");
            return null;
        }

        return new ShapeObject(objectInfo, shapeObjectType.GetValueOrDefault());
    }

    private ShapeObjectType? DetermineShapeObjectType(Godot.Collections.Dictionary elementDictionary) {
        if (ParserUtils.ToBool(elementDictionary.TryGet("ellipse")) == true) {
            return ShapeObjectType.Ellipse;
        } else if (ParserUtils.ToBool(elementDictionary.TryGet("point")) == true) {
            return ShapeObjectType.Point;
        } else {
            return ShapeObjectType.Rectangle;
        }
    }

    private DefaultObject FillToDefaultObject(
        ObjectInfo objectInfo, 
        Dictionary<string, object> optionalElementaryTypeFields,
        Dictionary<string, object[]> optionalArrayFields
        ) {
        uint? gID = (uint?)optionalElementaryTypeFields["gid"];
        if (gID == null) {
            GD.PushError("Parsed gid of the default object is null!");
            return null;
        }
        
        Property[] properties = (Property[])optionalArrayFields["properties"];
        if (properties == null) {
            GD.PushError("Parsed properties array of the default object is null!");
            return null;
        }

        return new DefaultObject(objectInfo, gID ?? 0u, properties);
    }

    private TextObject FillToTextObject(
        ObjectInfo objectInfo,
        Dictionary<string, object> optionalFields
    ) {
        Text? text = (Text?)optionalFields["text"];
        if (text == null) {
            GD.PushError("Parsed text of the text object is null!");
            return null;
        }

        return new TextObject(objectInfo, text.GetValueOrDefault());
    }
}
