using Godot;
using System;
using System.Linq;

public enum WangSetType {
    Corner, Edge, Mixed
}

public struct WangSet 
{
    public string name { get; private set; }
    public WangTile[] wangTiles { get; private set; }
    public WangColor[] colors { get; private set; }
    public int tileID { get; private set; }
    public Property[] properties { get; private set; }
    public WangSetType type { get; private set; }

    public WangSet(string name, WangTile[] wangTiles, WangColor[] colors, int tileID, Property[] properties, WangSetType type) {
        var requiredParameters = new object[] { 
            name,
            wangTiles,
            colors,
            tileID,
            properties
        };
        if (requiredParameters.Any(argument => argument == null)) {
            GD.PushError("Not all of the required tile parameters are initialized!");
        }
        this.name = name ?? "";
        this.wangTiles = wangTiles ?? new WangTile[0];
        this.colors = colors ?? new WangColor[0];
        this.tileID = tileID;
        this.properties = properties ?? new Property[0];
        this.type = type;
    }
}

