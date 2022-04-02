using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GodotCollectionsExtensions;

public class JsonMapParser : Parser {
    // Parses a map data from the json string.
    public override Map Parse(string data) {
        JSONParseResult parseResult = JSON.Parse(data);
        var mapDictionary = parseResult.Result as Godot.Collections.Dictionary;
        if (mapDictionary == null) {
            GD.PushError("Incorrect map JSON file format!");
            return null;
        }
        var mapInfo = new MapInfo();
        mapInfo.width = ToInt(mapDictionary.TryGet("width"));
        mapInfo.height = ToInt(mapDictionary.TryGet("height"));
        mapInfo.mapOrientation = DetermineMapOrientation(mapDictionary.TryGet("orientation") as string);
        mapInfo.tileWidth = ToInt(mapDictionary.TryGet("tilewidth"));
        mapInfo.tileHeight = ToInt(mapDictionary.TryGet("tileheight"));
        mapInfo.infinite = ToBool(mapDictionary.TryGet("infinite"));
        mapInfo.tiledVersion = ToString(mapDictionary.TryGet("tiledversion"));

        var tileSetsArray = mapDictionary.TryGet("tilesets") as Godot.Collections.Array;
        if (tileSetsArray == null) {
            GD.PushError("Can't extract the array of tile sets in the map!");
            return null;
        }
        var tileSets = new List<TileSet>();
        foreach (object tileSetElement in tileSetsArray) {
            var tileSetDicitonary = tileSetElement as Godot.Collections.Dictionary;
            var tileSet = ParseTileSet(tileSetDicitonary);
            if (tileSet != null) {
                tileSets.Add(tileSet);
            }
        }
        mapInfo.tileSets = tileSets.ToArray();


        var parsedMapFields = new object[] { 
            mapInfo.width,
            mapInfo.height,
            mapInfo.mapOrientation,
            mapInfo.tileWidth,
            mapInfo.tileHeight,
            mapInfo.tileSets,
            mapInfo.infinite,
            mapInfo.tiledVersion
        };
        if (parsedMapFields.Any(field => field == null)) {
            GD.PushError("One of the parsed map fields is null!");
            return null;
        }

        switch (mapInfo.mapOrientation) {
            case MapOrientation.Hexagonal:
                if (!ParseHexagonalMapFields(mapDictionary, mapInfo)) {
                    GD.PushError("Can't parse required hexagonal map fields!");
                    return null;
                }
                break;
            case MapOrientation.Staggered:
                if (!ParseStaggeredMapFields(mapDictionary, mapInfo)) {
                    GD.PushError("Can't parse required staggered map fields!");
                    return null;
                }
                break;
        }

        var layersArray = mapDictionary.TryGet("layers") as Godot.Collections.Array;
        if (layersArray == null) {
            GD.PushError("Can't extract the array of layers in the map!");
            return null;
        }
        Layer[] layers = ParseLayersArray(layersArray);

        var propertiesArray = mapDictionary.TryGet("properties") as Godot.Collections.Array;
        if (layersArray == null) {
            GD.PushError("Can't extract the array of properties in the map!");
            return null;
        }
        mapInfo.properties = ParsePropertiesArray(propertiesArray);

        mapInfo.renderOrder = DetermineRenderOrder(ToString(mapDictionary.TryGet("renderorder")));
        mapInfo.parallaxOriginPoint = ParseParallaxOriginPoint(mapDictionary);
        string backgroundColorHexCode = ToString(mapDictionary.TryGet("backgroundcolor"));
        if (backgroundColorHexCode != null) {
            mapInfo.backgroundColor = ParseColor(backgroundColorHexCode);
        }

        return new Map(mapInfo);
    }

    private Point? ParseParallaxOriginPoint(Godot.Collections.Dictionary mapDictionary) {
        double? parallaxOriginX = ToDouble(mapDictionary.TryGet("parallaxoriginx"));
        double? parallaxOriginY = ToDouble(mapDictionary.TryGet("parallaxoriginy"));
        if (parallaxOriginX == null || parallaxOriginY == null)
            return Point.Zero;

        return new Point(parallaxOriginX ?? 0.0, parallaxOriginY ?? 0.0);
    }

    private Layer[] ParseLayersArray(Godot.Collections.Array layersArray) {
        var layers = new List<Layer>();
        foreach (object layerElement in layersArray) {
            var layerDictionary = layerElement as Godot.Collections.Dictionary;
            Layer layer = ParseLayer(layerDictionary);
            if (layer != null) {
                layers.Add(layer);
            }
        }

        return layers.ToArray();
    }

    // Parses fields that are specific for a hexagonal map into the given map info.
    private bool ParseHexagonalMapFields(Godot.Collections.Dictionary mapDictionary, MapInfo mapInfo) {
        StaggerAxis? staggerAxis = DetermineStaggerAxis(ToString(mapDictionary.TryGet("staggeraxis")));
        StaggerIndex? staggerIndex = DetermineStaggerIndex(ToString(mapDictionary.TryGet("staggerindex")));
        int? hexSideLength = ToInt(ToString(mapDictionary.TryGet("hexsidelength")));

        if (staggerAxis == null || staggerAxis == null || hexSideLength == null) {
            GD.PushError("One of the required hexagonal map fields is null!");
            return false;
        }
        mapInfo.staggerAxis = staggerAxis;
        mapInfo.staggerIndex = staggerIndex;
        mapInfo.hexSideLength = hexSideLength;
        return true;
    }

    // Parses fields that are specific for a staggered map into the given map info.
    private bool ParseStaggeredMapFields(Godot.Collections.Dictionary mapDictionary, MapInfo mapInfo) {
        StaggerAxis? staggerAxis = DetermineStaggerAxis(ToString(mapDictionary.TryGet("staggeraxis")));
        StaggerIndex? staggerIndex = DetermineStaggerIndex(ToString(mapDictionary.TryGet("staggerindex")));

        if (staggerAxis == null || staggerAxis == null) {
            GD.PushError("One of the required staggered map fields is null!");
            return false;
        }
        mapInfo.staggerAxis = staggerAxis;
        mapInfo.staggerIndex = staggerIndex;
        return true;
    }

    // Parses an array of the property data dictionaries.
    private Property[] ParsePropertiesArray(Godot.Collections.Array propertiesArray) {
        var properties = new List<Property>();
        foreach (object propertyElement in propertiesArray) {
            var propertyDictionary = propertyElement as Godot.Collections.Dictionary;
            var parsedProperty = ParseProperty(propertyDictionary);
            if (parsedProperty != null) {
                properties.Add(parsedProperty.GetValueOrDefault());
            }
        }

        return properties.ToArray();
    }

    // Parses a tile set data from the tile set dictionary.
    private TileSet ParseTileSet(Godot.Collections.Dictionary tileSetDicitonary) {
        if (tileSetDicitonary == null) {
            GD.PushError("Parsing dictionary that contains tile set fields is null!");
            return null;
        }
        TileSetInfo? tileSetInfo = ParseRequiredTileSetFields(tileSetDicitonary);
        if (tileSetInfo == null) {
            return null;
        }
        ParseOptionalTileSetFields(tileSetDicitonary, tileSetInfo.GetValueOrDefault());
        
        return new TileSet(tileSetInfo.GetValueOrDefault());
    }

    // Parses a required tileset fields and returns TileSetInfo with that data.
    private TileSetInfo? ParseRequiredTileSetFields(Godot.Collections.Dictionary tileSetDictionary) {
        TileSetInfo tileSetInfo = new TileSetInfo();
        tileSetInfo.name = ToString(tileSetDictionary.TryGet("name"));
        tileSetInfo.firstGID = ToInt(tileSetDictionary.TryGet("firstgid"));
        tileSetInfo.image = ToString(tileSetDictionary.TryGet("image"));
        tileSetInfo.imageHeight = ToInt(tileSetDictionary.TryGet("imageheight"));
        tileSetInfo.imageWidth = ToInt(tileSetDictionary.TryGet("imagewidth"));
        tileSetInfo.margin = ToInt(tileSetDictionary.TryGet("margin"));
        tileSetInfo.spacing = ToInt(tileSetDictionary.TryGet("spacing"));
        tileSetInfo.tileCount = ToInt(tileSetDictionary.TryGet("tilecount"));
        tileSetInfo.tileHeight = ToInt(tileSetDictionary.TryGet("tileheight"));
        tileSetInfo.tileWidth = ToInt(tileSetDictionary.TryGet("tilewidth"));

        var parsedRequiredTileSetField = new object[] {
            tileSetInfo.name,
            tileSetInfo.firstGID, 
            tileSetInfo.image,
            tileSetInfo.imageHeight,
            tileSetInfo.imageWidth, 
            tileSetInfo.margin,
            tileSetInfo.spacing,
            tileSetInfo.tileCount,
            tileSetInfo.tileHeight, 
            tileSetInfo.tileWidth
        };
        if (parsedRequiredTileSetField.Any(field => field == null)) {
            GD.PushError("One of the required tile set fields is null!");
            return null;
        }

        return tileSetInfo;
    }

    // Parses an optional tileset fields and puts that data into the given TileSetInfo.
    private void ParseOptionalTileSetFields(Godot.Collections.Dictionary tileSetDictionary, TileSetInfo tileSetInfo) {
        var tileOffsetDictionary = tileSetDictionary.TryGet("tileoffset") as Godot.Collections.Dictionary;
        if (tileOffsetDictionary != null) {
            tileSetInfo.tileOffset = ParseTileOffset(tileOffsetDictionary);
        }
        var propertiesArray = tileSetDictionary.TryGet("properties") as Godot.Collections.Array;
        if (propertiesArray != null) {
            tileSetInfo.properties = ParsePropertiesArray(propertiesArray);
        }

        var wangSetsArray = tileSetDictionary.TryGet("wangsets") as Godot.Collections.Array;
        if (wangSetsArray != null) {
            var wangSets = new List<WangSet>();
            foreach (object wangSetObject in wangSetsArray) {
                var wangSetDicitonary = wangSetObject as Godot.Collections.Dictionary;
                var wangSet = ParseWangSet(wangSetDicitonary);
                if (wangSet != null) {
                    wangSets.Add(wangSet.GetValueOrDefault());
                }
            }
            tileSetInfo.wangSets = wangSets.ToArray();
        }

        if (tileSetDictionary.Contains("objectalignment")) {
            tileSetInfo.objectsAlignment = DetermineTileObjectAlignment(ToString(tileSetDictionary.TryGet("objectalignment")));
        }
        if (tileSetDictionary.Contains("transparentcolor")) {
            tileSetInfo.transparentColor = ParseColor(ToString(tileSetDictionary.TryGet("transparentcolor")));
        }

        var terrainsArray = tileSetDictionary.TryGet("terrains") as Godot.Collections.Array;
        if (terrainsArray != null) {
            var terrains = new List<Terrain>();
            foreach (object terrainElement in terrainsArray) {
                var terrainDictionary = terrainElement as Godot.Collections.Dictionary;
                var parsedTerrain = ParseTerrain(terrainDictionary);
                if (parsedTerrain != null) {
                    terrains.Add(parsedTerrain.GetValueOrDefault());
                }
            }
            tileSetInfo.terrains = terrains.ToArray();
        }

        var transformationsDictionary = tileSetDictionary.TryGet("transformations") as Godot.Collections.Dictionary;
        if (transformationsDictionary != null) {
            tileSetInfo.transfromations = ParseTransformations(transformationsDictionary);
        }
        var tilesArray = tileSetDictionary.TryGet("tiles") as Godot.Collections.Array;
        if (tilesArray != null) {
            var tiles = new List<Tile>();
            foreach (object tileElement in tilesArray) {
                var tileDictionary = tileElement as Godot.Collections.Dictionary;
                var parsedTile = ParseTile(tileDictionary);
                if (parsedTile != null) {
                    tiles.Add(parsedTile.GetValueOrDefault());
                }
            }
            tileSetInfo.tiles = tiles.ToArray();
        }
    }

    private IntPoint? ParseTileOffset(Godot.Collections.Dictionary tileOffsetDictionary) {
        if (tileOffsetDictionary == null) {
            GD.PushError("Parsing dictionary that contains tile offset fields is null!");
            return null;
        }
        int? x = ToInt(tileOffsetDictionary.TryGet("x"));
        int? y = ToInt(tileOffsetDictionary.TryGet("y"));
        if (x == null || y == null) {
            GD.PushError("One of the required tile offset fields is null!");
            return null;
        }

        return new IntPoint(x ?? 0, y ?? 0);
    }
    
    // Parses a tile data from the tile dictionary.
    private Tile? ParseTile(Godot.Collections.Dictionary tileDictionary) {
        if (tileDictionary == null) {
            GD.PushError("Parsing dictionary that contains tile fields is null!");
            return null;
        }
        var tileInfo = new TileInfo();
        tileInfo.id = ToInt(tileDictionary.TryGet("id"));
        if (tileInfo.id == null) {
            GD.PushError("Parsed tile id is null!");
            return null;
        }
        tileInfo.imageWidth = ToInt(tileDictionary.TryGet("imagewidth"));
        tileInfo.imageHeight = ToInt(tileDictionary.TryGet("imageheight"));
        tileInfo.type = ToString(tileDictionary.TryGet("type"));
        tileInfo.image = ToString(tileDictionary.TryGet("image"));
        tileInfo.probability = ToDouble(tileDictionary.TryGet("probability"));
        var terrainArray = tileDictionary.TryGet("terrain") as Godot.Collections.Array;
        if (terrainArray != null) {
            tileInfo.terrainIndex = ParseTerrainIndex(terrainArray);
        }
        var animationArray = tileDictionary.TryGet("animation") as Godot.Collections.Array;
        if (animationArray != null) {
            var animation = new List<Frame>();
            foreach (object animationElement in animationArray) {
                var frameDictionary = animationElement as Godot.Collections.Dictionary;
                var frame = ParseFrame(frameDictionary);
                if (frame != null) {
                    animation.Add(frame.GetValueOrDefault());
                }
            }
            tileInfo.animation = animation.ToArray();
        }
        var propertiesArray = tileDictionary.TryGet("properties") as Godot.Collections.Array;
        if (propertiesArray != null) {
            tileInfo.properties = ParsePropertiesArray(propertiesArray);
        }
        var objectGroupDictionary = tileDictionary.TryGet("objectgroup") as Godot.Collections.Dictionary;
        if (objectGroupDictionary != null) {
            tileInfo.objectGroup = ParseLayer(objectGroupDictionary);
        }
        var gridDictionary = tileDictionary.TryGet("grid") as Godot.Collections.Dictionary;
        if (gridDictionary != null) {
            tileInfo.grid = ParseGrid(gridDictionary);
        }

        return new Tile(tileInfo);
    }

    // Parses a grid data from the grid dictionary.
    private Grid? ParseGrid(Godot.Collections.Dictionary gridDictionary) {
        GridOrientation? orientation = GridOrientation.Orthogonal;
        if (gridDictionary.Contains("orientation")) {
            orientation = DetermineGridOrientation(ToString(gridDictionary.TryGet("orientation")));
        }
        int? width = ToInt(gridDictionary.TryGet("width"));
        int? height = ToInt(gridDictionary.TryGet("height"));
        if (width == null || height == null) {
            GD.PushError("One of the required grid field is null!");
            return null;
        }

        return new Grid(width ?? 0, height ?? 0, orientation ?? GridOrientation.Orthogonal);
    }

    // Parses a terrain data from the terrain dictionary.
    private Terrain? ParseTerrain(Godot.Collections.Dictionary terrainDictionary) {
        if (terrainDictionary == null) {
            GD.PushError("Parsing dictionary that contains terrain fields is null!");
            return null;
        }
        string name = ToString(terrainDictionary.TryGet("name"));
        int? tileID = ToInt(terrainDictionary.TryGet("tile"));
        if (name == null || tileID == null) {
            GD.PushError("One of the required terrain fields is null!");
            return null;
        }

        var propertiesArray = terrainDictionary.TryGet("properties") as Godot.Collections.Array;
        Property[] properties = null;
        if (propertiesArray != null) {
            properties = ParsePropertiesArray(propertiesArray);
        }

        return new Terrain(name, tileID ?? -1, properties);
    }

    private Frame? ParseFrame(Godot.Collections.Dictionary frameDictionary) {
        if (frameDictionary == null) {
            GD.PushError("Parsing dictionary that contains frame fields is null!");
            return null;
        }
        int? duration = ToInt(frameDictionary.TryGet("duration"));
        int? tileID = ToInt(frameDictionary.TryGet("tileid"));
        if (duration == null || tileID == null) {
            GD.PushError("One of the required frame fields is null!");
            return null;
        }
        
        return new Frame(tileID ?? -1, duration ?? 0);
    }

    // Parses a transformations data from the transformations dicitonary.
    private Transfromations? ParseTransformations(Godot.Collections.Dictionary transformationsDictionary) {
        if (transformationsDictionary == null) {
            GD.PushError("Parsing dictionary that contains transformations fields is null!");
            return null;
        }
        bool? hflip = ToBool(transformationsDictionary.TryGet("hflip"));
        bool? vflip = ToBool(transformationsDictionary.TryGet("vflip"));
        bool? rotate = ToBool(transformationsDictionary.TryGet("rotate"));
        bool? preferUntransformed = ToBool(transformationsDictionary.TryGet("preferuntransformed"));

        if (hflip == null || vflip == null || rotate == null || preferUntransformed == null) {
            GD.PushError("One of the required transformations fields is null!");
            return null;
        }

        return new Transfromations(
            hflip ?? false, 
            vflip ?? false, 
            rotate ?? false, 
            preferUntransformed ?? false
        );
    }

    // Parses a wang set data from the wang set dictionary.
    private WangSet? ParseWangSet(Godot.Collections.Dictionary wangSetDictionary) {
        if (wangSetDictionary == null) {
            GD.PushError("Parsing dictionary that contains wang set fields is null!");
            return null;
        }
        var wangSetInfo = new WangSetInfo();

        var wangTilesArray = wangSetDictionary.TryGet("wangtiles") as Godot.Collections.Array;
        if (wangTilesArray == null) {
            GD.PushError("Wang tiles field of the wang set is null!");
            return null;
        }
        var wangTiles = new List<WangTile>();
        foreach (object wangTileObject in wangTilesArray) {
            var wangTileDicitonary = wangTileObject as Godot.Collections.Dictionary;
            var wangTile = ParseWangTile(wangTileDicitonary);
            if (wangTile != null) {
                wangTiles.Add(wangTile.GetValueOrDefault());
            }
        }
        wangSetInfo.wangTiles = wangTiles.ToArray();

        var propertiesArray = wangSetDictionary.TryGet("properties") as Godot.Collections.Array;
        if (propertiesArray == null) {
            GD.PushError("Properties field of the wang set is null!");
            return null;
        }
        wangSetInfo.properties = ParsePropertiesArray(propertiesArray);

        wangSetInfo.name = ToString(wangSetDictionary.TryGet("name"));
        wangSetInfo.tileID = ToInt(wangSetDictionary.TryGet("tileid"));

        var parsedRequiredWangSetFields = new object[] { 
            wangSetInfo.wangTiles,
            wangSetInfo.properties,
            wangSetInfo.name,
            wangSetInfo.tileID
        };
        if (parsedRequiredWangSetFields.Any(field => field == null)) {
            GD.PushError("One of the required wang set fields is null!");
            return null;
        }
        
        wangSetInfo.type = DetermineWangSetType(ToString(wangSetDictionary.TryGet("type")));
        var wangColorsArray = wangSetDictionary.TryGet("colors") as Godot.Collections.Array;
        if (wangColorsArray != null) {
            var wangColors = new List<WangColor>();
            foreach (object wangColorElement in wangColorsArray) {
                var colorDictionary = wangSetDictionary.TryGet("colors") as Godot.Collections.Dictionary;
                var color = ParseWangColor(colorDictionary);
                if (color != null) {
                    wangColors.Add(color.GetValueOrDefault());
                }
            }
            wangSetInfo.colors = wangColors.ToArray();
        }

        return new WangSet(wangSetInfo);
    }

    // Parses a wang tile data from the wang tile dicitonary.
    private WangTile? ParseWangTile(Godot.Collections.Dictionary wangTileDicitonary) {
        if (wangTileDicitonary == null) {
            GD.PushError("Parsing dictionary that contains wang tiles fields is null!");
            return null;
        }
        int? tileID = ToInt(wangTileDicitonary.TryGet("tileid"));
        var wangIDArray = wangTileDicitonary.TryGet("wangid") as Godot.Collections.Array;
        if (tileID == null || wangIDArray == null) {
            GD.PushError("One of the required wang tile fields is null!");
            return null;
        }
        ushort[] wangID = ParseWangID(wangIDArray);
        if (wangID == null) {
            GD.PushError("Parsed wang id is null!");
            return null;
        }
        
        return new WangTile(tileID ?? -1, wangID);
    }

    // Parses a wang color data from the wang color dicitonary.
    private WangColor? ParseWangColor(Godot.Collections.Dictionary wangColorDictionary) {
        if (wangColorDictionary == null) {
            GD.PushError("Parsing dictionary that contains wang color fields is null!");
            return null;
        }
        var wangColorInfo = new WangColorInfo();
        wangColorInfo.name = ToString(wangColorDictionary.TryGet("name"));
        wangColorInfo.color = ParseColor(ToString(wangColorDictionary.TryGet("color")));
        wangColorInfo.probability = ToDouble(wangColorDictionary.TryGet("probability"));
        wangColorInfo.tileID = ToInt(wangColorDictionary.TryGet("tile"));

        var parsedRequiredWangColorFields = new object[] { 
            wangColorInfo.name,
            wangColorInfo.color,
            wangColorInfo.probability,
            wangColorInfo.tileID
        };
        if (parsedRequiredWangColorFields.Any(field => field == null)) {
            GD.PushError("One of the required wang color fields is null!");
            return null;
        }

        var propertiesArray = wangColorDictionary.TryGet("properties") as Godot.Collections.Array;
        if (propertiesArray != null) {
            wangColorInfo.properties = ParsePropertiesArray(propertiesArray);
        }

        return new WangColor(wangColorInfo);
    }

    // Parses a layer data from the layer dictionary.
    private Layer ParseLayer(Godot.Collections.Dictionary layerDictionary, bool infinite = false) {
        if (layerDictionary == null) {
            GD.PushError("Parsing dictionary that contains layer fields is null!");
            return null;
        }
        LayerInfo? layerInfo = ParseRequiredLayerFields(layerDictionary);
        if (layerInfo == null) {
            return null;
        }
        LayerInfo _layerInfo = layerInfo.GetValueOrDefault();
        _layerInfo.infinite = infinite;

        return FillToSpecificLayer(layerDictionary, _layerInfo);
    }

    // Parses fields that are common to all of layer types.
    private LayerInfo? ParseRequiredLayerFields(Godot.Collections.Dictionary layerDictionary) {
        int? xTilesOffset = ToInt(layerDictionary.TryGet("x"));
        int? yTilesOffset = ToInt(layerDictionary.TryGet("y"));
        LayerInfo layerInfo = new LayerInfo();
        layerInfo.tilesOffset = new IntPoint(xTilesOffset ?? 0, yTilesOffset ?? 0);
        layerInfo.visible = ToBool(layerDictionary.TryGet("visible"));
        layerInfo.type = DetermineLayerType(ToString(layerDictionary.TryGet("type")));
        layerInfo.opacity = ToDouble(layerDictionary.TryGet("opacity"));

        var parsedRequiredLayerFields = new object[] { 
            xTilesOffset,
            yTilesOffset,
            layerInfo.tilesOffset,
            layerInfo.visible,
            layerInfo.type,
            layerInfo.opacity
        };
        if (parsedRequiredLayerFields.Any(field => field == null)) {
            GD.PushError("One of the required layer fields is null!");
            return null;
        }

        var propertiesArray = layerDictionary.TryGet("properties") as Godot.Collections.Array;
        if (propertiesArray != null) {
            layerInfo.properties = ParsePropertiesArray(propertiesArray);
        }

        return layerInfo;
    }

    // Fills fields that specific for this layer type and creates a corresponding layer.
    private Layer FillToSpecificLayer(Godot.Collections.Dictionary layerDictionary, LayerInfo layerInfo) {
        switch (layerInfo.type) {
            case LayerType.ImageLayer:
                return FillToImageLayer(layerDictionary, layerInfo);
            case LayerType.TileLayer:
                return FillToTileLayer(layerDictionary, layerInfo);
            case LayerType.Group:
                return FillToGroupLayer(layerDictionary, layerInfo);
            case LayerType.ObjectGroup:
                return FillToObjectGroupLayer(layerDictionary, layerInfo);
            default:
                GD.PushError("Layer type is null!");
                return null;
        }
    }

    // Parses fields that are specific for an image layer.
    private ImageLayer FillToImageLayer(Godot.Collections.Dictionary layerDictionary, LayerInfo layerInfo) {
        string image = layerDictionary.TryGet("image") as string;
        bool? repeatX = ToBool(layerDictionary.TryGet("repeatx"));
        bool? repeatY = ToBool(layerDictionary.TryGet("repeaty"));
        if (image == null || repeatX == null || repeatY == null) {
            GD.PushError("One of the required image layer fields is null!");
            return null;
        }
        return new ImageLayer(layerInfo, image, repeatX ?? false, repeatY ?? false);
    }

    // Fills fields that are specific for a not infinite tile layer and creates a TileLayerObject.
    private TileLayer FillToNotInfiniteTileLayer(Godot.Collections.Dictionary layerDictionary, LayerInfo layerInfo) {
        Compression? compression = Compression.None;
        if (layerDictionary.Contains("compression")) {
            compression = DetermineCompression(layerDictionary.TryGet("compression") as string);
        }
        Encoding? encoding = Encoding.CSV;
        if (layerDictionary.Contains("encoding")) {
            encoding = DetermineEncoding(layerDictionary.TryGet("encoding") as string);
        }
        var encodedLayerData = layerDictionary.TryGet("data");
        if (encodedLayerData == null) {
            GD.PushError("Data field of the layer is null");
            return null;
        }
        TileLayerData layerData = ParseLayerData(
            ToString(encodedLayerData),
            layerInfo.width ?? 0,
            layerInfo.height ?? 0,
            encoding ?? Encoding.CSV,
            compression ?? Compression.None
            );
        if (layerData == null) {
            GD.PushError("Parsed layer data is null!");
            return null;
        }
        return new TileLayer(layerInfo, layerData);
    }

    // Parses chunk fields from the infinite tile layer.
    private Chunk? ParseChunk(Godot.Collections.Dictionary chunkDictionary) {
        if (chunkDictionary == null) {
            GD.PushError("Parsing dictionary that contains chunk fields is null!");
            return null;
        }
        var chunkWidth = ToInt(chunkDictionary.TryGet("width"));
        var chunkHeight = ToInt(chunkDictionary.TryGet("height"));
        var chunkCoordinates = new IntPoint(ToInt(chunkDictionary.TryGet("x")) ?? 0, ToInt(chunkDictionary.TryGet("y")) ?? 0);
        var encodedChunkData = ToString(chunkDictionary.TryGet("data"));
        TileLayerData chunkData = ParseLayerData(ToString(encodedChunkData), chunkWidth ?? 0, chunkHeight ?? 0, Encoding.CSV);
        
        var parsedChunkFields = new object[] { chunkWidth, chunkHeight, chunkCoordinates, encodedChunkData };
        if (parsedChunkFields.Any(field => field == null)) {
            GD.PushError("One of the parsed chunk fields is null!");
            return null;
        }
        return new Chunk(chunkData, chunkWidth ?? 0, chunkHeight ?? 0, chunkCoordinates);
    }

    // Fills fields that are specific for a infinte tile layer (chunks) and creates a TileLayerObject.
    private TileLayer FillToInfiniteTileLayer(Godot.Collections.Dictionary layerDictionary, LayerInfo layerInfo) {
        var chunksArray = layerDictionary.TryGet("chunks") as Godot.Collections.Array;
        if (chunksArray == null) {
            GD.PushError("Chunks field of the infinite map layer is null!");
            return null;
        }
        var chunks = new List<Chunk>();
        foreach (object chunkElement in chunksArray) {
            var chunkDictionary = chunkElement as Godot.Collections.Dictionary;
            var parsedChunk = ParseChunk(chunkDictionary);
            if (parsedChunk != null) {
                chunks.Add(parsedChunk.GetValueOrDefault());
            }
        }
        return new TileLayer(layerInfo, chunks.ToArray());
    }

    // Fills fields that are specific for a tile layer and creates a TileLayerObject.
    private TileLayer FillToTileLayer(Godot.Collections.Dictionary layerDictionary, LayerInfo layerInfo) {
        if (layerInfo.infinite == null) {
            GD.PushError("Infinite field of the layer is null!");
            return null;
        }
        bool? isLayerInfinite = layerInfo.infinite;
        layerInfo.width = ToInt(layerDictionary.TryGet("width"));
        layerInfo.height = ToInt(layerDictionary.TryGet("height"));
        if (layerInfo.width == 0 || layerInfo.height == 0) {
            GD.PushError("One of the tile layer fields is null!");
            return null;
        }
        if (layerInfo.infinite ?? false) {
            return FillToNotInfiniteTileLayer(layerDictionary, layerInfo);
        } else {
            return FillToInfiniteTileLayer(layerDictionary, layerInfo);
        }
    }

    // Fills fields that are specific for a group layer and creates a GroupLayerObject.
    private GroupLayer FillToGroupLayer(Godot.Collections.Dictionary layerDictionary, LayerInfo layerInfo) {
        var groupLayersArray = layerDictionary.TryGet("layers") as Godot.Collections.Array;
        if (groupLayersArray == null) {
            GD.PushError("Can't extract the array of layers in the group layer!");
            return null;
        }
        var groupLayers = new List<Layer>();
        foreach (object groupLayerElement in groupLayersArray) {
            var groupLayerDictionary = groupLayerElement as Godot.Collections.Dictionary;
            var groupLayer = ParseLayer(layerDictionary, layerInfo.infinite ?? false);
            if (groupLayer != null) {
                groupLayers.Add(groupLayer);
            }
        }
        return new GroupLayer(layerInfo, groupLayers.ToArray());
    }

    // Fills fields that are specific for an object group layer and creates an ObjectGroupLayer.
    private ObjectGroupLayer FillToObjectGroupLayer(Godot.Collections.Dictionary layerDictionary, LayerInfo layerInfo) {
        var drawOrder = DrawOrder.TopDown;
        if (layerDictionary.Contains("draworder")) {
            drawOrder = DetermineDrawOrder(ToString(layerDictionary.TryGet("draworder"))) ?? DrawOrder.TopDown;
        }
        var groupObjectsArray = layerDictionary.TryGet("objects") as Godot.Collections.Array;
        if (groupObjectsArray == null) {
            GD.PushError("Can't extract the array of objects in the object layer!");
            return null;
        }

        var objects = new List<Object>();
        foreach (object groupObjectElement in groupObjectsArray) {
            var groupObjectDictionary = groupObjectElement as Godot.Collections.Dictionary;
            Object groupObject = ParseObject(groupObjectDictionary);
            if (groupObject != null) {
                objects.Add(groupObject);
            }
        }

        return new ObjectGroupLayer(layerInfo, objects.ToArray(), drawOrder);
    }

    // Parses a data field of the tile layer.
    private TileLayerData ParseLayerData(string data, int layerWidth, int layerHeight, Encoding encoding, Compression compression = Compression.None) {
        switch (encoding) {
            case Encoding.CSV:
                if (compression != Compression.None) {
                    GD.PushError("CSV format can't be compressed!");
                }
                return new CSVDecoder().Decode(data, layerWidth, layerHeight);
            default:
                return new Base64Decoder().Decode(data, layerWidth, layerHeight, compression);
        }
    }

    private ObjectType DetermineObjectType(Godot.Collections.Dictionary objectDictionary) {
        if (ToBool(objectDictionary.TryGet("ellipse")) == true || 
            ToBool(objectDictionary.TryGet("point")) == true) {
            return ObjectType.ShapeObject;
        } else if (ToBool(objectDictionary.TryGet("polygon")) == true ||
            ToBool(objectDictionary.TryGet("polyline")) == true) {
            return ObjectType.PointObject;
        } else if (ToBool(objectDictionary.TryGet("text")) == true) {
            return ObjectType.TextObject;
        } else if (objectDictionary.Contains("gid")) {
            return ObjectType.DefaultObject;
        } else {
            return ObjectType.ShapeObject;
        }
    }

    // Parses a object data from the object dictionary.
    private Object ParseObject(Godot.Collections.Dictionary objectDictionary) {
        if (objectDictionary == null) {
            GD.PushError("Parsing dictionary that contains object fields is null!");
            return null;
        }
        ObjectInfo? objectInfo = ParseRequiredObjectFields(objectDictionary);
        if (objectInfo == null) {
            return null;
        }

        return FillToSpecificObject(objectDictionary, objectInfo.GetValueOrDefault());
    }

    // Parses fields that are common to all of object types.
    private ObjectInfo? ParseRequiredObjectFields(Godot.Collections.Dictionary objectDictionary) {
        var objectInfo = new ObjectInfo();
        objectInfo.name = ToString(objectDictionary.TryGet("name"));
        objectInfo.id = ToInt(objectDictionary.TryGet("id"));
        double? x = ToDouble(objectDictionary.TryGet("x"));
        double? y = ToDouble(objectDictionary.TryGet("y"));
        objectInfo.coordinates = new Point(x ?? 0.0, y ?? 0.0);
        objectInfo.width = ToDouble(objectDictionary.TryGet("width"));
        objectInfo.height = ToDouble(objectDictionary.TryGet("height"));
        objectInfo.rotation = ToDouble(objectDictionary.TryGet("rotation"));
        objectInfo.type = ToString(objectDictionary.TryGet("type"));
        objectInfo.visible = ToBool(objectDictionary.TryGet("visible"));
        objectInfo.template = ToString(objectDictionary.TryGet("template"));

        var parsedObjectRequiredFields = new object[] { 
            objectInfo.name,
            objectInfo.id,
            x,
            y,
            objectInfo.width,
            objectInfo.height,
            objectInfo.rotation,
            objectInfo.type,
            objectInfo.visible,
            objectInfo.template
        };
        if (parsedObjectRequiredFields.Any(field => field == null)) {
            GD.PushError("One of the parsed object fields is null!");
            return null;
        }
        objectInfo.objectType = DetermineObjectType(objectDictionary);
        return objectInfo;
    }

    // Fills fields that specific for this object type and creates a corresponding object.
    private Object FillToSpecificObject(Godot.Collections.Dictionary objectDictionary, ObjectInfo objectInfo) {
        switch (objectInfo.objectType) {
            case ObjectType.DefaultObject:
                return FillToDefaultObject(objectDictionary, objectInfo);
            case ObjectType.PointObject:
                return FillToPointObject(objectDictionary, objectInfo);
            case ObjectType.ShapeObject:
                return FillToShapeObject(objectDictionary, objectInfo);
            default:
                return FillToTextObject(objectDictionary, objectInfo);
        }
    }

    // Fills fields that are specific for a default object and creates a DefaultObject.
    private DefaultObject FillToDefaultObject(Godot.Collections.Dictionary objectDictionary, ObjectInfo objectInfo) {
        uint? GID = ToUInt(objectDictionary.TryGet("gid"));
        var propertiesArray = objectDictionary.TryGet("properties") as Godot.Collections.Array;
        if (propertiesArray == null) {
            GD.PushError("Properties field of the default object is null!");
            return null;
        }
        Property[] properties = ParsePropertiesArray(propertiesArray);

        return new DefaultObject(objectInfo, GID ?? 0u, properties);
    }

    // Parses a property data from the property dictionary.
    private Property? ParseProperty(Godot.Collections.Dictionary propertyDictionary) {
        if (propertyDictionary == null) {
            GD.PushError("Parsing dictionary that contains property fields is null!");
            return null;
        }
        string name = ToString(propertyDictionary.TryGet("name"));
        PropertyType? propertyType = DeterminePropertyType(ToString(propertyDictionary.TryGet("type")));
        object value = propertyDictionary.TryGet("value");
        if (name == null || propertyType == null || value == null) {
            GD.PushError("One of the parsed property fields is null!");
            return null;
        }

        return new Property(name, value, propertyType ?? PropertyType.String);
    }

    // Fills fields that are specific for a point object (polyline and polygon) and creates a PointObject.
    private PointObject FillToPointObject(Godot.Collections.Dictionary objectDictionary, ObjectInfo objectInfo) {
        PointObjectType? pointObjectType = DeterminePointObjectType(objectDictionary);
        if (pointObjectType == null) {
            GD.PushError("Point object type is not determined!");
            return null;
        }

        Godot.Collections.Array pointsArray = null;
        switch (pointObjectType) {
            case PointObjectType.Polygon:
                pointsArray = objectDictionary.TryGet("polygon") as Godot.Collections.Array;
                break;
            case PointObjectType.Polyline:
                pointsArray = objectDictionary.TryGet("polyline") as Godot.Collections.Array;
                break;
        }
        if (pointsArray == null) {
            GD.PushError("Can't extract the array of points in the point object!");
            return null;
        }

        var points = new List<Point>();
        foreach (object pointElement in pointsArray) {
            var pointDictionary = pointElement as Godot.Collections.Dictionary;
            var parsedPoint = ParsePoint(pointDictionary);
            if (parsedPoint != null) {
                points.Add(parsedPoint.GetValueOrDefault());
            }
        }

        return new PointObject(objectInfo, pointObjectType.GetValueOrDefault(), points.ToArray());
    }

    private PointObjectType? DeterminePointObjectType(Godot.Collections.Dictionary objectDictionary) {
        if (objectDictionary.Contains("polyline")) {
            return PointObjectType.Polyline;
        } else if (objectDictionary.Contains("polygon")) {
            return PointObjectType.Polygon;
        }
        GD.PushError("Can't determine a point object type!");
        return null;
    }

    // Parses a point data from the point dictionary.
    private Point? ParsePoint(Godot.Collections.Dictionary pointDictionary) {
        double? x = ToDouble(pointDictionary.TryGet("x"));
        double? y = ToDouble(pointDictionary.TryGet("y"));
        if (x == null || y == null) {
            GD.PushError("Can't extract one of the point coordinates");
            return null;
        }
        
        return new Point(x ?? 0.0, y ?? 0.0);
    }

    // Fills fields that are specific for a shape object (ellipse, rectangle and point) and creates a ShapeObject.
    private ShapeObject FillToShapeObject(Godot.Collections.Dictionary objectDictionary, ObjectInfo objectInfo) {
        ShapeObjectType? shapeObjectType = DetermineShapeObjectType(objectDictionary);
        if (shapeObjectType == null) {
            GD.PushError("Shape object type is not determined!");
            return null;
        }

        return new ShapeObject(objectInfo, shapeObjectType.GetValueOrDefault());
    }

    private ShapeObjectType? DetermineShapeObjectType(Godot.Collections.Dictionary objectDictionary) {
        if (ToBool(objectDictionary.TryGet("ellipse")) == true) {
            return ShapeObjectType.Ellipse;
        } else if (ToBool(objectDictionary.TryGet("rectangle")) == true) {
            return ShapeObjectType.Rectangle;
        } else if (ToBool(objectDictionary.TryGet("point")) == true) {
            return ShapeObjectType.Point;
        } else {
            GD.PushError("Can't determine a shape object type!");
            return null;
        }
    }

    // Fills fields that are specific for a text object and creates a TextObject.
    private TextObject FillToTextObject(Godot.Collections.Dictionary objectDictionary, ObjectInfo objectInfo) {
        var textDictionary = objectDictionary.TryGet("text") as Godot.Collections.Dictionary;
        if (textDictionary == null) {
            GD.PushError("Can't extract the text dictionary in the text object!");
            return null;
        }
        Text? text = ParseText(textDictionary);
        if (text == null) {
            GD.PushError("Parsed text field in the text object is null!");
            return null;
        }

        return new TextObject(objectInfo, text.GetValueOrDefault());
    }

    // Parses a text data from the text dictionary.
    private Text? ParseText(Godot.Collections.Dictionary textDictionary) {
        var textInfo = new TextInfo();
        textInfo.text = ToString(textDictionary.TryGet("text"));
        if (textInfo.text == null) {
            GD.PushError("Text field of the text object is null!");
            return null;
        }
        textInfo.pixelSize = ToInt(textDictionary.TryGet("pixelsize"));
        textInfo.bold = ToBool(textDictionary.TryGet("bold"));
        textInfo.italic = ToBool(textDictionary.TryGet("italic"));
        textInfo.fontFamily = ToString(textDictionary.TryGet("fontfamily"));
        textInfo.halign = DetermineHorizontalAlignment(ToString(textDictionary.TryGet("halign")));
        textInfo.valign = DetermineVerticalAlignment(ToString(textDictionary.TryGet("valign")));
        textInfo.kerning = ToBool(textDictionary.TryGet("kerning"));
        textInfo.strikeout = ToBool(textDictionary.TryGet("underline"));
        textInfo.wrap = ToBool(textDictionary.TryGet("wrap"));

        return new Text(textInfo);
    }
}
