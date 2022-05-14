using Godot;
using System;
using System.Linq;

namespace TiledImporter.Structures
{
    public struct WangColorInfo
    {
        public string name;
        public Color? color;
        public double? probability;
        public int? tileID;
        public Property[] properties;
    }

    public struct WangColor
    {
        public string name { get; private set; }
        public Color color { get; private set; }
        public double probability { get; private set; }
        public int tileID { get; private set; }
        public Property[] properties { get; private set; } // (optional).

        public WangColor(WangColorInfo wangColorInfo)
        {
            var requiredFields = new object[] {
            wangColorInfo.name,
            wangColorInfo.color,
            wangColorInfo.probability,
            wangColorInfo.tileID
        };
            if (requiredFields.Any(field => field == null))
            {
                GD.PushError("Not all of the required tile parameters are initialized!");
            }
            name = wangColorInfo.name ?? "";
            color = wangColorInfo.color ?? new Color();
            probability = wangColorInfo.probability ?? 0.0;
            tileID = wangColorInfo.tileID ?? -1;

            this.properties = wangColorInfo.properties ?? new Property[0];
        }
    }
}
