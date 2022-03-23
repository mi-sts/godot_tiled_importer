using Godot;
using System;

public enum PropertyType {
    String, Int, Float, Bool,  Color, File, Object, Class
}

public struct Property 
{
    public string name;
    public PropertyType propertyType;
    public dynamic value;
}

