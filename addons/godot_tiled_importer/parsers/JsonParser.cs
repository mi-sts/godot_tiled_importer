using Godot;
using System;
using TiledImporter.Structures;
using TiledImporter.Parsers;
using System.Text.RegularExpressions;

namespace TiledImporter.Parsers
{
    public class JsonParser : Parser
    {
        public override Map Parse(string data)
        {
            ChangeGIDFieldsType(ref data);
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

        private void ChangeGIDFieldsType(ref string data)
        { // Changes gid fields to string type to avoid data loss during parsing.
            string gIDPattern = @"""gid"":(\d+)";
            data = Regex.Replace(data, gIDPattern, @"""gid"":""$1""");

        }
    }
}
