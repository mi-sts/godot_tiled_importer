using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class MapJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "width", ElementaryType.Int },
                { "height", ElementaryType.Int },
                { "orientation", ElementaryType.MapOrientation },
                { "tilewidth", ElementaryType.Int },
                { "tileheight", ElementaryType.Int },
                { "infinite", ElementaryType.Bool },
                { "tiledversion", ElementaryType.String }
            };
            }
        }

        protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "renderorder", ElementaryType.RenderOrder },
                { "backgroundcolor", ElementaryType.Color },

                // Parallax origin point coordinates.
                { "parallaxoriginx", ElementaryType.Double },
                { "parallaxoriginy", ElementaryType.Double },
        
                // Hexagonal and staggered map fields.
                { "staggeraxis", ElementaryType.StaggerAxis },
                { "staggerindex", ElementaryType.StaggerIndex },

                // Hexagonal map fields.
                { "hexsidelength", ElementaryType.Int }
            };
            }
        }

        protected override Dictionary<string, DataStructure> RequiredArrayFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
                { "layers", DataStructure.Layer },
                { "tilesets", DataStructure.TileSet }
            };
            }
        }

        protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames
        {
            get
            {
                return new Dictionary<string, DataStructure>() {
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
            var mapInfo = new MapInfo();
            mapInfo.width = (int)requiredElementaryTypeFields["width"];
            mapInfo.height = (int)requiredElementaryTypeFields["height"];
            mapInfo.mapOrientation = (MapOrientation)requiredElementaryTypeFields["orientation"];
            mapInfo.tileWidth = (int)requiredElementaryTypeFields["tilewidth"];
            mapInfo.tileHeight = (int)requiredElementaryTypeFields["tileheight"];
            mapInfo.infinite = (bool)requiredElementaryTypeFields["infinite"];
            mapInfo.tiledVersion = (string)requiredElementaryTypeFields["tiledversion"];


            var requiredArrayFields = ParseRequiredArrayFields(elementDictionary);
            if (requiredArrayFields == null)
            {
                GD.PushError("Dictionary of the required array fields is null!");
                return null;
            }
            mapInfo.layers = Array.ConvertAll(requiredArrayFields["layers"], layer => (Layer)layer);
            mapInfo.tileSets = Array.ConvertAll(requiredArrayFields["tilesets"], tileSet => (TiledImporter.Structures.TileSet)tileSet);


            var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
            if (optionalArrayFields == null)
            {
                GD.PushError("Dictionary of the optional array fields is null!");
                return null;
            }
            object[] boxedProperties = optionalArrayFields["properties"];
            if (boxedProperties != null)
            {
                mapInfo.properties = Array.ConvertAll(boxedProperties, property => (Property)property);
            }


            var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
            if (optionalElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the optional elementary type fields is null!");
                return null;
            }
            mapInfo.renderOrder = (RenderOrder?)optionalElementaryTypeFields["renderorder"];
            mapInfo.backgroundColor = (Color?)optionalElementaryTypeFields["backgroundcolor"];
            var parallaxOriginPointX = (double?)optionalElementaryTypeFields["parallaxoriginx"];
            var parallaxOriginPointY = (double?)optionalElementaryTypeFields["parallaxoriginy"];
            if (parallaxOriginPointX == null || parallaxOriginPointY == null)
                mapInfo.parallaxOriginPoint = Point.Zero;
            else
                mapInfo.parallaxOriginPoint = new Point(parallaxOriginPointX ?? 0.0, parallaxOriginPointY ?? 0.0);
            mapInfo.staggerAxis = (StaggerAxis?)optionalElementaryTypeFields["staggeraxis"];
            mapInfo.staggerIndex = (StaggerIndex?)optionalElementaryTypeFields["staggerindex"];
            mapInfo.hexSideLength = (int?)optionalElementaryTypeFields["hexsidelength"];

            return new Map(mapInfo);
        }
    }
}
