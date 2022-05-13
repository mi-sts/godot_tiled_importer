using Godot;
using System;
using TiledImporter.MapBuilder;

public class qqqqqqqqqqqqq : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var sourceFilePath = "grassland_test.tmj";
        var saveFilePath = "q.tscn";
        GD.Print(sourceFilePath);

        var tiledMapFile = new Godot.File();
        tiledMapFile.Open(sourceFilePath, File.ModeFlags.Read);
        var tiledMapData = tiledMapFile.GetAsText();
        tiledMapFile.Close();

        var tiledMapJsonParser = new TiledImporter.Parsers.JsonParser();
        TiledImporter.Structures.Map map = tiledMapJsonParser.Parse(tiledMapData);

        var tileMapBuilder = new TileMapBuilder();
        PackedScene mapScene = tileMapBuilder.GenerateTileMapScene("qqq", map);

        ResourceSaver.Save($"{saveFilePath}", mapScene);
    
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
