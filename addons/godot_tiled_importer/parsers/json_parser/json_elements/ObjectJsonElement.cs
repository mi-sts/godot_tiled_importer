using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class ObjectJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "id", ElementaryType.Int },
                { "x", ElementaryType.Double },
                { "y", ElementaryType.Double }
            };
            }
        }

        protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                // Default object fields.
                { "name", ElementaryType.String },
                { "width", ElementaryType.Double },
                { "height", ElementaryType.Double },
                { "rotation", ElementaryType.Double },
                { "type", ElementaryType.String },
                { "visible", ElementaryType.Bool },  

                // Template object fields.
                { "template", ElementaryType.String },

                // Shape object fields.
                { "ellipse", ElementaryType.Bool },
                { "point", ElementaryType.Bool },

                // Tile object fields.
                { "gid", ElementaryType.Object }
            };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                //Text object fields.
                { "text", DataStructure.Text }
            };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames
        {
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

        public override object Parse(Godot.Collections.Dictionary elementDictionary)
        {
            var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
            if (requiredElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the required elementary type fields is null!");
                return null;
            }
            int id = (int)requiredElementaryTypeFields["id"];
            double xCoordinate = (double)requiredElementaryTypeFields["x"];
            double yCoordinate = (double)requiredElementaryTypeFields["y"];
            Point position = new Point(xCoordinate, yCoordinate);

            var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
            if (optionalElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the optional elementary type fields is null!");
                return null;
            }
            var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
            if (optionalArrayFields == null)
            {
                GD.PushError("Dictionary of the optional array fields is null!");
                return null;
            }
            var optionalFields = ParseOptionalFields(elementDictionary);
            if (optionalFields == null)
            {
                GD.PushError("Dictionary of the optional fields is null!");
                return null;
            }

            if (optionalElementaryTypeFields["template"] != null)
            {
                return FillToTemplateObject(id, position, optionalElementaryTypeFields);
            }
            else
            {
                return FillToStandardObject(
                    id,
                    position,
                    optionalElementaryTypeFields,
                    optionalFields,
                    optionalArrayFields,
                    elementDictionary
                );
            }
        }

        private TemplateObject FillToTemplateObject(
            int id,
            Point position,
            Dictionary<string, object> optionalElementaryTypeFields
            )
        {
            string template = ParserUtils.ToString(optionalElementaryTypeFields["template"]);
            if (template == null)
            {
                GD.PushError("Template field of the template object is null!");
                return null;
            }
            return new TemplateObject(id, position, ObjectType.TemplateObject, template);
        }

        private DefaultObject FillToStandardObject(
            int id,
            Point position,
            Dictionary<string, object> optionalElementaryTypeFields,
            Dictionary<string, object> optionalFields,
            Dictionary<string, object[]> optionalArrayFields,
            Godot.Collections.Dictionary elementDictionary
        )
        {
            var objectInfo = new DefaultObjectInfo();
            objectInfo.name = (string)optionalElementaryTypeFields["name"];
            objectInfo.width = (double)optionalElementaryTypeFields["width"];
            objectInfo.height = (double)optionalElementaryTypeFields["height"];
            objectInfo.rotation = (double)optionalElementaryTypeFields["rotation"];
            objectInfo.type = (string)optionalElementaryTypeFields["type"];
            objectInfo.visible = (bool)optionalElementaryTypeFields["visible"];
            objectInfo.template = (string)optionalElementaryTypeFields["template"];
            var requiredFields = new object[] {
            objectInfo.name,
            objectInfo.width,
            objectInfo.height,
            objectInfo.rotation,
            objectInfo.type,
            objectInfo.visible,
        };
            if (requiredFields.Any(field => field == null))
            {
                GD.PushError("One of the required standard object fields is null!");
                return null;
            }
            ObjectType objectType = ObjectType.ShapeObject;
            if (optionalElementaryTypeFields["ellipse"] as bool? == true ||
                optionalElementaryTypeFields["point"] as bool? == true)
            {
                objectType = ObjectType.ShapeObject;
            }
            else if (optionalArrayFields["polygon"] != null || optionalArrayFields["polyline"] != null)
            {
                objectType = ObjectType.PointObject;
            }
            else if (optionalFields["text"] != null)
            {
                objectType = ObjectType.TextObject;
            }
            else if (optionalElementaryTypeFields["gid"] != null)
            {
                objectType = ObjectType.DefaultObject;
            }
            else
            {
                objectType = ObjectType.ShapeObject;
            }

            switch (objectType)
            {
                case ObjectType.DefaultObject:
                    return FillToDefaultObject(
                        id,
                        position,
                        objectType,
                        objectInfo,
                        optionalElementaryTypeFields,
                        optionalArrayFields
                        );
                case ObjectType.ShapeObject:
                    return FillToShapeObject(
                        id,
                        position,
                        objectType,
                        objectInfo,
                        elementDictionary
                        );
                case ObjectType.PointObject:
                    return FillToPointObject(
                        id,
                        position,
                        objectType,
                        objectInfo,
                        optionalArrayFields,
                        elementDictionary
                        );
                case ObjectType.TextObject:
                    return FillToTextObject(
                        id,
                        position,
                        objectType,
                        objectInfo,
                        optionalFields
                        );
                default:
                    GD.PushError("Not determined object type!");
                    return null;
            }
        }

        private PointObject FillToPointObject(
            int id,
            Point position,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            Dictionary<string, object[]> optionalArrayFields,
            Godot.Collections.Dictionary elementDictionary
            )
        {
            PointObjectType? pointObjectType = DeterminePointObjectType(elementDictionary);
            if (pointObjectType == null)
            {
                GD.PushError("Point object type is not determined!");
                return null;
            }

            object[] boxedPoints = null;
            switch (pointObjectType)
            {
                case PointObjectType.Polygon:
                    boxedPoints = optionalArrayFields["polygon"];
                    if (boxedPoints == null)
                    {
                        GD.PushError("Parsed polygon array of the point object is null!");
                        return null;
                    }
                    break;
                case PointObjectType.Polyline:
                    boxedPoints = optionalArrayFields["polyline"];
                    if (boxedPoints == null)
                    {
                        GD.PushError("Parsed polyline array of the point object is null!");
                        return null;
                    }
                    break;
            }

            if (boxedPoints == null)
            {
                GD.PushError("Parsed points of the point object is null!");
                return null;
            }
            var points = Array.ConvertAll(boxedPoints, point => (Point)point);

            return new PointObject(
                id,
                position,
                type,
                objectInfo,
                pointObjectType.GetValueOrDefault(),
                points);
        }

        private PointObjectType? DeterminePointObjectType(Godot.Collections.Dictionary elementDictionary)
        {
            if (elementDictionary.Contains("polyline"))
            {
                return PointObjectType.Polyline;
            }
            else if (elementDictionary.Contains("polygon"))
            {
                return PointObjectType.Polygon;
            }
            GD.PushError("Not determined a point object type!");
            return null;
        }

        private ShapeObject FillToShapeObject(
            int id,
            Point position,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            Godot.Collections.Dictionary elementDictionary)
        {
            ShapeObjectType? shapeObjectType = DetermineShapeObjectType(elementDictionary);
            if (shapeObjectType == null)
            {
                GD.PushError("Shape object type is not determined!");
                return null;
            }

            return new ShapeObject(
                id,
                position,
                type,
                objectInfo,
                shapeObjectType.GetValueOrDefault());
        }

        private ShapeObjectType? DetermineShapeObjectType(Godot.Collections.Dictionary elementDictionary)
        {
            if (ParserUtils.ToBool(elementDictionary.TryGet("ellipse")) == true)
            {
                return ShapeObjectType.Ellipse;
            }
            else if (ParserUtils.ToBool(elementDictionary.TryGet("point")) == true)
            {
                return ShapeObjectType.Point;
            }
            else
            {
                return ShapeObjectType.Rectangle;
            }
        }

        private TileObject FillToDefaultObject(
            int id,
            Point position,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            Dictionary<string, object> optionalElementaryTypeFields,
            Dictionary<string, object[]> optionalArrayFields
            )
        {
            /*uint? gID = (uint?)optionalElementaryTypeFields["gid"];
            if (gID == null)
            {
                GD.PushError("Parsed gid of the default object is null!");
                return null;
            }*/

            Property[] properties = null;
            object[] boxedProperties = optionalArrayFields["properties"];
            if (boxedProperties != null)
                properties = Array.ConvertAll(optionalArrayFields["properties"], property => (Property)property);

            return new TileObject(
                id,
                position,
                type,
                objectInfo,
                Convert.ToUInt32(optionalElementaryTypeFields["gid"]),
                properties);
        }

        private TextObject FillToTextObject(
            int id,
            Point position,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            Dictionary<string, object> optionalFields
        )
        {
            Text? text = (Text?)optionalFields["text"];
            if (text == null)
            {
                GD.PushError("Parsed text of the text object is null!");
                return null;
            }

            return new TextObject(
                id,
                position,
                type,
                objectInfo,
                text.GetValueOrDefault());
        }
    }
}
