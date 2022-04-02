using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;

public class test : Node {
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        var testData = new File();
        testData.Open("res://addons/godot_tiled_importer/parsers/island.tmj", File.ModeFlags.Read);
        var data = testData.GetAsText();
        testData.Close();
        var parsed = JSON.Parse(data);
        //GD.Print(parsed.Result);
        var res = ((parsed.Result as Godot.Collections.Dictionary)["layers"]) as Godot.Collections.Array;
        foreach (object i in res) {
            GD.Print(i as Godot.Collections.Dictionary);
        }

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
