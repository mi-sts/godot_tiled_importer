using Godot;
using System;
using System.Linq;

namespace TiledImporter.Structures
{
    public enum WangSetType
    {
        Corner, Edge, Mixed
    }

    public struct WangSetInfo
    {
        public string name;
        public WangTile[] wangTiles;
        public int? tileID;
        public Property[] properties;
        public WangSetType? type;
        public WangColor[] colors;

    }

    public class WangSet
    {
        public string name { get; private set; }
        public WangTile[] wangTiles { get; private set; }
        public int tileID { get; private set; }
        public Property[] properties { get; private set; }
        public WangSetType? type { get; private set; } // (optional).
        public WangColor[] colors { get; private set; } // (optional).

        public WangSet(WangSetInfo wangSetInfo)
        {
            var requiredFields = new object[] {
            wangSetInfo.name,
            wangSetInfo.wangTiles,
            wangSetInfo.tileID,
        };
            if (requiredFields.Any(field => field == null))
            {
                GD.PushError("Not all of the required tile parameters are initialized!");
            }

            name = wangSetInfo.name ?? "";
            wangTiles = wangSetInfo.wangTiles ?? new WangTile[0];
            tileID = wangSetInfo.tileID ?? -1;
            properties = wangSetInfo.properties ?? new Property[0];

            type = wangSetInfo.type;
            colors = wangSetInfo.colors;
        }
    }
}
