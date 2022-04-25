using Godot;
using System;

namespace TiledImporter.Structures
{
    public enum PropertyType
    {
        String, Int, Float, Bool, Color, File, Object, Class, Null
    }

    public class Property
    {
        public string name { get; private set; }
        public object value { get; private set; }
        public PropertyType propertyType { get; private set; }

        private static Type ConvertProperyTypeToSystemType(PropertyType propertyType)
        {
            // Some properties represented as a string.
            switch (propertyType)
            {
                case PropertyType.String:
                    return typeof(string);
                case PropertyType.Int:
                    return typeof(int);
                case PropertyType.Float:
                    return typeof(float);
                case PropertyType.Bool:
                    return typeof(bool);
                case PropertyType.Color:
                    return typeof(string);
                case PropertyType.File:
                    return typeof(string);
                case PropertyType.Object:
                    return typeof(string);
                default:
                    return null;
            }
        }

        public Property(string name, object value, PropertyType propertyType)
        {
            if (name == null)
            {
                GD.PushError("Name of the property is not initialized!");
                return;
            }
            if (value == null)
            {
                GD.PushError("Value of the property is null!");
                return;
            }
            this.name = name ?? "";
            this.propertyType = propertyType;
            this.value = 0;

            Type expectedType = ConvertProperyTypeToSystemType(propertyType);
            if (expectedType == null)
            {
                return;
            }
            try
            {
                value = Convert.ChangeType(value, ConvertProperyTypeToSystemType(propertyType));
            }
            catch (InvalidCastException)
            {
                GD.PushError($"Can't cast the property value to the declared {expectedType.ToString()} type!");
            }
            catch (FormatException)
            {
                GD.PushError($"Property value type is not in a format recognized by {expectedType.ToString()} type!");
            }
            catch (OverflowException)
            {
                GD.PushError($"Property value represents a number that is out of range of {expectedType.ToString()} type!");
            }
        }
    }
}
