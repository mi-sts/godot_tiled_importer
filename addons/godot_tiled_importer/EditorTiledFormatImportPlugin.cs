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

    public override string GetResourceType() => "PackedScene";

    public override string GetSaveExtension() => "tscn";

    public override int GetPresetCount() => 0;

    public override Godot.Collections.Array GetImportOptions(int preset) => new Godot.Collections.Array();

    public override int Import(
        string sourceFilePath, 
        string saveFilePath, 
        Dictionary options, 
        Godot.Collections.Array platformVariants, 
        Godot.Collections.Array genFiles
        )
    {
        sourceFilePath = GodotProjectPathToRelative(sourceFilePath);
        saveFilePath = GodotProjectPathToRelative(saveFilePath);

        GD.Print(sourceFilePath);

        var tiledMapFile = new Godot.File();
        tiledMapFile.Open(sourceFilePath, File.ModeFlags.Read);
        var tiledMapData = tiledMapFile.GetAsText();
        tiledMapFile.Close();

        var tiledMapJsonParser = new TiledImporter.Parsers.JsonParser();
        TiledImporter.Structures.Map map = tiledMapJsonParser.Parse(tiledMapData);

        var tileMapBuilder = new TileMapBuilder();
        string mapName = GetFileNameFromPath(sourceFilePath);
        PackedScene mapScene = tileMapBuilder.GenerateTileMapScene(mapName, map);

        return (int)ResourceSaver.Save($"{saveFilePath}.{GetSaveExtension()}", mapScene);
    }

    private string GodotProjectPathToRelative(string godotProjectPath) {
        if (godotProjectPath.Length >= 6 && godotProjectPath.Substring(0, 6) == "res://") 
            return godotProjectPath.Substring(6);
        else 
            return null;
    }

    private string GetFileNameFromPath(string filePath) {
        string[] directories = filePath.Split("/");
        string fileWithExtension = directories[directories.Length - 1];
        return fileWithExtension.Split(".")[0];
    }
}