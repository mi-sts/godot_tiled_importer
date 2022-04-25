using Godot;
using System;
using TiledImporter.Structures;
using TiledImporter.Parsers;

namespace TiledImporter.Parsers {
public class JsonParser : Parser
{
    public override Map Parse(string data)
    {
        JSONParseResult parseResult = JSON.Parse(data);
        var mapDictionary = parseResult.Result as Godot.Collections.Dictionary;
        if (mapDictionary == null)
        {
            GD.PushError("Incorrect map JSON file format!");
            return null;
        }

        var mapJsonElement = new MapJsonElement();
        return (Map)mapJsonElement.Parse(mapDictionary);
    }
}
}
