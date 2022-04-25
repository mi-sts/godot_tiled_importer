using Godot;
using System;

namespace GodotCollectionsExtensions
{
    public static class GodotDictionaryExtenstion
    {
        public static object TryGet(this Godot.Collections.Dictionary dictionary, object key)
        {
            if (dictionary.Contains(key))
                return dictionary[key];

            return null;
        }
    }
}
