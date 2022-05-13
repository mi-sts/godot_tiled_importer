using Godot;
using Godot.Collections;
using System;
using TiledImporter.Parsers;
using TiledImporter.MapBuilder;
using TiledImporter.Structures;

public class EditorTiledMapFormatImportPlugin : EditorImportPlugin
{
    public override string GetImporterName() => "mi-sts.godot_tiled_importer";

    public override string GetVisibleName() => "Tiled Map";

    public override Godot.Collections.Array GetRecognizedExtensions() => 
        new Godot.Collections.Array(new string[] {"tmj"});

    public override string GetResourceType() => "TileMap";

    public override int Import(
        string sourceFile, 
        string savePath, 
        Dictionary options, 
        Godot.Collections.Array platformVariants, 
        Godot.Collections.Array genFiles
        )
    {
        var tiledMapFile = new Godot.File();
        tiledMapFile.Open(sourceFile, File.ModeFlags.Read);
        var tiledMapData = tiledMapFile.GetAsText();
        tiledMapFile.Close();

        var tiledMapJsonParser = new TiledImporter.Parsers.JsonParser();
        TiledImporter.Structures.Map map = tiledMapJsonParser.Parse(tiledMapData);

        var tileMapBuilder = new TileMapBuilder();
        PackedScene mapScene = tileMapBuilder.GenerateTileMapScene(map);

        return (int)ResourceSaver.Save(savePath, mapScene);
    }
}
