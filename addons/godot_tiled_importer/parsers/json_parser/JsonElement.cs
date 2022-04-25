using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public abstract class JsonElement
    {
        // Fields that represent other JsonElement objects.

        protected virtual Dictionary<string, DataStructure> RequiredFieldsNames { get; private set; }
        protected virtual Dictionary<string, DataStructure> OptionalFieldsNames { get; set; }

        // Fields that represent arrays of other JsonElement object.
        protected virtual Dictionary<string, DataStructure> RequiredArrayFieldsNames { get; set; }
        protected virtual Dictionary<string, DataStructure> OptionalArrayFieldsNames { get; set; }


        // Fields that represent system type variables.
        protected virtual Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames { get; private set; }
        protected virtual Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames { get; private set; }

        public JsonElement()
        {
            if (RequiredFieldsNames == null)
                RequiredFieldsNames = new Dictionary<string, DataStructure>();
            if (OptionalFieldsNames == null)
                OptionalFieldsNames = new Dictionary<string, DataStructure>();

            if (RequiredElementaryTypeFieldsNames == null)
                RequiredElementaryTypeFieldsNames = new Dictionary<string, ElementaryType>();
            if (OptionalElementaryTypeFieldsNames == null)
                OptionalElementaryTypeFieldsNames = new Dictionary<string, ElementaryType>();

            if (RequiredArrayFieldsNames == null)
                RequiredArrayFieldsNames = new Dictionary<string, DataStructure>();
            if (OptionalArrayFieldsNames == null)
                OptionalArrayFieldsNames = new Dictionary<string, DataStructure>();
        }

        private static JsonElement CreateJsonElementByDataStructure(DataStructure dataStructure)
        {
            switch (dataStructure)
            {
                case DataStructure.Layer:
                    return new LayerJsonElement();
                case DataStructure.Object:
                    return new ObjectJsonElement();
                case DataStructure.Frame:
                    return new FrameJsonElement();
                case DataStructure.Grid:
                    return new GridJsonElement();
                case DataStructure.Map:
                    return new MapJsonElement();
                case DataStructure.Property:
                    return new PropertyJsonElement();
                case DataStructure.Terrain:
                    return new TerrainJsonElement();
                case DataStructure.Text:
                    return new TextJsonElement();
                case DataStructure.Tile:
                    return new TileJsonElement();
                case DataStructure.TileSet:
                    return new TileSetJsonElement();
                case DataStructure.Transfomations:
                    return new TransformationsJsonElement();
                case DataStructure.WangColor:
                    return new WangColorJsonElement();
                case DataStructure.WangSet:
                    return new WangSetJsonElement();
                case DataStructure.WangTile:
                    return new WangTileJsonElement();
                case DataStructure.Point:
                    return new PointJsonElement();
                case DataStructure.IntPoint:
                    return new IntPointJsonElement();
                default:
                    GD.PushError("Can't determine a data stucture type!");
                    return null;
            }
        }

        // Parses a data from dictionary and returns a filled data class corresponding to it's type.
        public abstract object Parse(Godot.Collections.Dictionary elementDictionary);

        // Parses fields that must be present in this json dictionary. Returns array of JsonElement.
        protected Dictionary<string, object> ParseRequiredFields(Godot.Collections.Dictionary elementDictionary)
        {
            if (elementDictionary is null)
            {
                GD.PushError("Parsing dictionary is null!");
                return null;
            }

            var parsedFields = new Dictionary<string, object>();
            foreach (string name in RequiredFieldsNames.Keys)
            {
                object requiredFieldObject = elementDictionary.TryGet(name);
                var requiredFieldDictionary = requiredFieldObject as Godot.Collections.Dictionary;
                if (requiredFieldDictionary == null)
                {
                    GD.PushError("Required field dictionary is null!");
                    return null;
                }

                JsonElement jsonElement = CreateJsonElementByDataStructure(RequiredFieldsNames[name]);
                object parsedField = jsonElement.Parse(requiredFieldDictionary);
                if (parsedField == null)
                {
                    GD.PushError("Parsed required field is null!");
                    return null;
                }

                parsedFields[name] = parsedField;
            }

            return parsedFields;
        }

        // Parses fields that are optional for this json dictionary. Returns array of JsonElement.
        protected Dictionary<string, object> ParseOptionalFields(Godot.Collections.Dictionary elementDictionary)
        {
            if (elementDictionary is null)
            {
                GD.PushError("Parsing dictionary is null!");
                return null;
            }

            var parsedFields = new Dictionary<string, object>();
            foreach (string name in OptionalFieldsNames.Keys)
            {
                object optionalFieldObject = elementDictionary.TryGet(name);
                var optionalFieldDictionary = optionalFieldObject as Godot.Collections.Dictionary;
                if (optionalFieldDictionary != null)
                {
                    JsonElement jsonElement = CreateJsonElementByDataStructure(OptionalFieldsNames[name]);
                    object parsedField = jsonElement.Parse(optionalFieldDictionary);
                    if (parsedField == null)
                    {
                        GD.PushError("Parsed optional field is null!");
                        return null;
                    }

                    parsedFields[name] = parsedField;
                }
                else
                {
                    parsedFields[name] = null;
                }
            }

            return parsedFields;
        }

        protected object ParseElementaryTypeField(object fieldObject, ElementaryType fieldType)
        {
            switch (fieldType)
            {
                case ElementaryType.Int:
                    return ParserUtils.ToInt(fieldObject);
                case ElementaryType.Bool:
                    return ParserUtils.ToBool(fieldObject);
                case ElementaryType.Double:
                    return ParserUtils.ToDouble(fieldObject);
                case ElementaryType.UInt:
                    return ParserUtils.ToUInt(fieldObject);
                case ElementaryType.String:
                    return ParserUtils.ToString(fieldObject);
                case ElementaryType.UShort:
                    return ParserUtils.ToUShort(fieldObject);
                case ElementaryType.Object:
                    return fieldObject;
                case ElementaryType.MapOrientation:
                    return ParserUtils.DetermineMapOrientation(ParserUtils.ToString(fieldObject));
                case ElementaryType.LayerType:
                    return ParserUtils.DetermineLayerType(ParserUtils.ToString(fieldObject));
                case ElementaryType.Compression:
                    return ParserUtils.DetermineCompression(ParserUtils.ToString(fieldObject));
                case ElementaryType.Encoding:
                    return ParserUtils.DetermineEncoding(ParserUtils.ToString(fieldObject));
                case ElementaryType.DrawOrder:
                    return ParserUtils.DetermineDrawOrder(ParserUtils.ToString(fieldObject));
                case ElementaryType.PropertyType:
                    return ParserUtils.DeterminePropertyType(ParserUtils.ToString(fieldObject));
                case ElementaryType.HorizontalAlignment:
                    return ParserUtils.DetermineHorizontalAlignment(ParserUtils.ToString(fieldObject));
                case ElementaryType.VerticalAlignment:
                    return ParserUtils.DetermineVerticalAlignment(ParserUtils.ToString(fieldObject));
                case ElementaryType.StaggerAxis:
                    return ParserUtils.DetermineStaggerAxis(ParserUtils.ToString(fieldObject));
                case ElementaryType.StaggerIndex:
                    return ParserUtils.DetermineStaggerIndex(ParserUtils.ToString(fieldObject));
                case ElementaryType.RenderOrder:
                    return ParserUtils.DetermineRenderOrder(ParserUtils.ToString(fieldObject));
                case ElementaryType.WangSetType:
                    return ParserUtils.DetermineWangSetType(ParserUtils.ToString(fieldObject));
                case ElementaryType.GridOrientation:
                    return ParserUtils.DetermineGridOrientation(ParserUtils.ToString(fieldObject));
                case ElementaryType.TileObjectsAlignment:
                    return ParserUtils.DetermineTileObjectsAlignment(ParserUtils.ToString(fieldObject));
                case ElementaryType.Color:
                    return ParserUtils.ParseColor(ParserUtils.ToString(fieldObject));
                default:
                    GD.PushError("Can't determine an elementary field type!");
                    return null;
            }
        }

        // Parses elementary fields that must be present in this json dictionary. Returns an array of elementary type values.
        protected Dictionary<string, object> ParseRequiredElementaryTypeFields(Godot.Collections.Dictionary elementDictionary)
        {
            if (elementDictionary is null)
            {
                GD.PushError("Parsing dictionary is null!");
                return null;
            }

            var parsedFields = new Dictionary<string, object>();
            foreach (string name in RequiredElementaryTypeFieldsNames.Keys)
            {
                object requiredElementaryTypeFieldObject = elementDictionary.TryGet(name);
                if (requiredElementaryTypeFieldObject == null)
                {
                    GD.PushError("Value of the required elementary type field  is null!");
                    return null;
                }

                object parsedField = ParseElementaryTypeField(requiredElementaryTypeFieldObject, RequiredElementaryTypeFieldsNames[name]);
                if (parsedField == null)
                {
                    GD.PushError("Parsed required elementary type field is null!");
                    return null;
                }

                parsedFields[name] = parsedField;
            }

            return parsedFields;
        }

        // Parses elementary fields that are optional for this json dictionary. Returns an array of elementary type values.

        protected Dictionary<string, object> ParseOptionalElementaryTypeFields(Godot.Collections.Dictionary elementDictionary)
        {
            if (elementDictionary is null)
            {
                GD.PushError("Parsing dictionary is null!");
                return null;
            }

            var parsedFields = new Dictionary<string, object>();
            foreach (string name in OptionalElementaryTypeFieldsNames.Keys)
            {
                object optionalElementaryTypeFieldObject = elementDictionary.TryGet(name);
                if (optionalElementaryTypeFieldObject != null)
                {
                    object parsedField = ParseElementaryTypeField(optionalElementaryTypeFieldObject, OptionalElementaryTypeFieldsNames[name]);
                    if (parsedField == null)
                    {
                        GD.PushError("Parsed optional elementary type field is null!");
                        return null;
                    }

                    parsedFields[name] = parsedField;
                }
                else
                {
                    parsedFields[name] = null;
                }
            }

            return parsedFields;
        }

        protected object[] ParseArray(Godot.Collections.Array array, JsonElement fieldJsonElement)
        {
            if (array == null)
            {
                GD.PushError("Parsing array is null!");
                return null;
            }

            var parsedArrayElements = new List<object>();
            foreach (object arrayElement in array)
            {
                var fieldDicitonary = arrayElement as Godot.Collections.Dictionary;
                if (fieldDicitonary == null)
                {
                    GD.PushError("Value of the array element dictionary is null!");
                    return null;
                }

                object parsedField = fieldJsonElement.Parse(fieldDicitonary);
                if (parsedField == null)
                {
                    GD.PushError("Parsed array element field is null!");
                    return null;
                }
                parsedArrayElements.Add(parsedField);
            }

            return parsedArrayElements.ToArray();
        }

        // Parses array fields that must be present in this json dictionary. Returns an array of JsonElement.
        protected Dictionary<string, object[]> ParseRequiredArrayFields(Godot.Collections.Dictionary elementDicitonary)
        {
            if (elementDicitonary == null)
            {
                GD.PushError("Parsing dictionary is null!");
                return null;
            }

            var parsedRequiredArrayFields = new Dictionary<string, object[]>();
            foreach (string name in RequiredArrayFieldsNames.Keys)
            {
                object requiredFieldObject = elementDicitonary.TryGet(name);
                var requiredArrayField = requiredFieldObject as Godot.Collections.Array;
                if (requiredArrayField == null)
                {
                    GD.PushError("Required array field is null!");
                    return null;
                }

                JsonElement jsonElement = CreateJsonElementByDataStructure(RequiredArrayFieldsNames[name]);
                object[] parsedRequiredArrayField = ParseArray(requiredArrayField, jsonElement);
                parsedRequiredArrayFields[name] = parsedRequiredArrayField;
            }

            return parsedRequiredArrayFields;
        }

        // Parses array fields that are optional for this json dictionary. Returns an array of JsonElement.
        protected Dictionary<string, object[]> ParseOptionalArrayFields(Godot.Collections.Dictionary elementDictionary)
        {
            if (elementDictionary == null)
            {
                GD.PushError("Parsing dictionary is null!");
                return null;
            }

            var parsedOptionalArrayFields = new Dictionary<string, object[]>();
            foreach (string name in OptionalArrayFieldsNames.Keys)
            {
                object optionalFieldObject = elementDictionary.TryGet(name);
                var optionalArrayField = optionalFieldObject as Godot.Collections.Array;
                if (optionalArrayField != null)
                {
                    JsonElement jsonElement = CreateJsonElementByDataStructure(OptionalArrayFieldsNames[name]);
                    object[] parsedOptionalArrayField = ParseArray(optionalArrayField, jsonElement);
                    parsedOptionalArrayFields[name] = parsedOptionalArrayField;
                }
                else
                {
                    parsedOptionalArrayFields[name] = null;
                }
            }

            return parsedOptionalArrayFields;
        }
    }
}
