using Godot;
using System;

public class DefaultObject : Object
{
    public int gID { get; private set; }
    public Property[] properties { get; private set; }

    public DefaultObject (ObjectInfo objectInfo, int gID, Property[] properties) : base(objectInfo) {
        if (properties == null) {
            GD.PushError("Properties of the default object are not initialized!");
        }
        this.gID = gID;
        this.properties = properties ?? new Property[0];
    }
}
