using Godot;
using System;

public class TextObject : Object {
    public Text text { get; private set; }

    public TextObject(ObjectInfo objectInfo, Text text) : base(objectInfo) {
        this.text = text;
    }
}
