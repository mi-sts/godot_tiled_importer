# Godot Tiled importer (Mono version).
Importer of tile maps from the Tiled editor to the Godot game engine.

# What types of maps does the plugin support?
The plugin supports all types of maps available in Tiled, namely:
- Orthogonal maps
- Hexagonal cards
- Isometric maps
- Staggerd isometric maps
- Object group maps (partially)

# What are the advantages of this importer?
- An atlas is used to draw orthogonal and isometric maps, which is very efficient in terms of performance.
- Import time is almost instantaneous even for huge maps.
- Ability to continue editing the map in Tiled editor after import, the plugin will automatically load the changes.

# Getting started
## Import plugin to Godot
- First you need to download the plugin repository and move the addons folder with the plugin to the root folder of your project in Godot (since the plugin uses C#, it will only work in the Mono version of the engine). Next, be sure to build the project, and then enable the plugin in Project/Project Settings/Plugins.

![Alt Text](https://github.com/mi-sts/godot_tiled_importer/blob/main/gifs/plugin_import.gif)

## Export tilemap from Tiled
- Then open your map in Tiled and export it in JSON format which has tmj extension (the plugin only supports this format at the moment) to any folder inside your project. Don't forget to move images containing tiles or any other graphic assets used in your map to the same folder as the previously exported file, or to any subfolder within it.

![Alt Text](https://github.com/mi-sts/godot_tiled_importer/blob/main/gifs/map_export.gif)

## Paths to files
- Be sure to make sure that the path to your images is in the correct format, otherwise change it to the correct one. You can do this by opening the exported map file in any text editor (for example, notepad) and entering the name of your tile image.

- The image path format must be written relative to the root folder of your Godot project. 

- For example, if you created a folder my_map inside your project and placed the map file map.tmj and the graphic file with tiles map_tileset.png there, then the path to the image will be: "my_map/map_tileset.png".
- You may also want to put the graphic file in the tileset subfolder, then the path to the image becomes: "my_map/tilset/map_tilset.png".

![Alt Text](https://github.com/mi-sts/godot_tiled_importer/blob/main/gifs/tileset_import.gif)

## Finally
If everything is done correctly, then when you click on the map file, a scene will open in front of you with what you need.

![Alt Text](https://github.com/mi-sts/godot_tiled_importer/blob/main/gifs/result.gif)
