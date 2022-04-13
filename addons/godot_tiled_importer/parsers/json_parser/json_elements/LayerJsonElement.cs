using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;

public class LayerJsonElement : JsonElement
{   
    protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames { 
        get 
        { 
            return new Dictionary<string, ElementaryType>() {
                { "x", ElementaryType.Int },
                { "y", ElementaryType.Int },
                { "name", ElementaryType.String },
                { "id", ElementaryType.Int },
                { "visible", ElementaryType.Bool },
                { "type", ElementaryType.LayerType },
                { "opacity", ElementaryType.Double }
            }; 
        }
    }

    protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames { 
        get 
        { 
            return new Dictionary<string, ElementaryType>() {
                // Image layer fields.
                { "image", ElementaryType.String },
                { "repeatx", ElementaryType.Bool },
                { "repeaty", ElementaryType.Bool },
                { "transparentcolor", ElementaryType.Color },

                // Tile layer fields.
                { "width", ElementaryType.Int },
                { "height", ElementaryType.Int },

                // Not infinite tile layer fields.
                { "compression", ElementaryType.Compression },
                { "encoding", ElementaryType.Encoding },
                { "data", ElementaryType.String },

                // Object group layer fields.
                { "draworder", ElementaryType.DrawOrder }
            }; 
        }
    }

    protected override Dictionary<string, DataStructure> OptionalArrayFieldsNames {
        get 
        {
            return new Dictionary<string, DataStructure>() {
                { "properties", DataStructure.Property },
                
                // Infinite tile layer fields.
                { "chunks", DataStructure.Chunk },

                // Group layer fields.
                { "layers", DataStructure.Layer },

                // Object group layer fields.
                { "objects", DataStructure.Object }
            };
        }
    }

    public override object Parse(Godot.Collections.Dictionary elementDictionary) {
        var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
        if (requiredElementaryTypeFields == null) {
            GD.PushError("Dictionary of the required elementary type fields is null!");
            return null;
        }
        var layerInfo = new LayerInfo();
        int xTilesOffset = (int)requiredElementaryTypeFields["x"];
        int yTilesOffset = (int)requiredElementaryTypeFields["y"];
        layerInfo.tilesOffset = new IntPoint(xTilesOffset, yTilesOffset);
        layerInfo.name = (string)requiredElementaryTypeFields["name"];
        layerInfo.id = (int)requiredElementaryTypeFields["id"];
        layerInfo.visible = (bool)requiredElementaryTypeFields["visible"];
        layerInfo.type = (LayerType)requiredElementaryTypeFields["type"];
        layerInfo.opacity = (double)requiredElementaryTypeFields["opacity"];
        layerInfo.infinite = false;


        var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
        if (optionalArrayFields == null) {
            GD.PushError("Dictionary of the optional array fields is null!");
            return null;
        }
        layerInfo.properties = (Property[])optionalArrayFields["properties"];

        var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
        if (optionalElementaryTypeFields == null) {
            GD.PushError("Dictionary of the optional elementary type fields is null!");
            return null;
        }
        switch (layerInfo.type) {
            case LayerType.ImageLayer: 
                return FillToImageLayer(layerInfo, optionalElementaryTypeFields);
            case LayerType.TileLayer:
                return FillToTileLayer(
                    layerInfo,
                    optionalElementaryTypeFields,
                    optionalArrayFields,
                    elementDictionary
                );
            case LayerType.ObjectGroup:
                return FillToObjectGroupLayer(
                    layerInfo,
                    optionalElementaryTypeFields,
                    optionalArrayFields
                );
            default:
                return FillToGroupLayer(layerInfo, optionalArrayFields);
        }
    }

    private ImageLayer FillToImageLayer(LayerInfo layerInfo, Dictionary<string, object> optionalElementaryTypeFields) {
        string image = (string)optionalElementaryTypeFields["image"];
        bool? repeatX = (bool?)optionalElementaryTypeFields["repeatx"];
        bool? repeatY = (bool?)optionalElementaryTypeFields["repeaty"];
        if (image == null || repeatX == null || repeatY == null) {
            GD.PushError("One of the parsed required image layer fields is null!");
            return null;
        }
        Color? transparentColor = (Color?)optionalElementaryTypeFields["transparentcolor"];

        return new ImageLayer(layerInfo, image, repeatX ?? false, repeatY ?? false, transparentColor);
    }

    private TileLayer FillToTileLayer(
        LayerInfo layerInfo, 
        Dictionary<string, object> optionalElementaryTypeFields,
        Dictionary<string, object[]> optionalArrayFields,
        Godot.Collections.Dictionary elementDictionary
        ) {
        int? width = (int?)optionalElementaryTypeFields["width"];
        int? height = (int?)optionalElementaryTypeFields["height"];
        if (width == null || height == null) {
            GD.PushError("One of the tile layer size fields is null!");
            return null;
        }

        if (optionalArrayFields["chunks"] != null) {
            layerInfo.infinite = true;
            Chunk[] chunks = (Chunk[])optionalArrayFields["chunks"];
            if (chunks == null) {
                GD.PushError("Parsed chunks array of the infinite tile layer is null!");
                return null;
            }

            return new TileLayer(layerInfo, width ?? 0, height ?? 0, chunks);
        } else if (optionalElementaryTypeFields["data"] != null) {
            layerInfo.infinite = false;
            Compression compression = (Compression?)optionalElementaryTypeFields["compression"] ?? Compression.None;
            Encoding encoding = (Encoding?)optionalElementaryTypeFields["encoding"] ?? Encoding.CSV;
            string data = ParserUtils.ToString(elementDictionary.TryGet("data"));
            if (data == null) {
                GD.PushError("Parsed data array of the not infinite tile layer is null!");
                return null;
            }
            TileLayerData layerData = ParserUtils.ParseLayerData(
                data,
                width ?? 0,
                height ?? 0,
                encoding,
                compression
            );
            if (layerData == null) {
                GD.PushError("Parsed layer data is null!");
                return null;
            }

            return new TileLayer(layerInfo, width ?? 0, height ?? 0, layerData);
        } else {
            GD.PushError("Not determined type of tile layer (infinite or not)");
            return null;
        }
    }

    private ObjectGroupLayer FillToObjectGroupLayer(
        LayerInfo layerInfo,
        Dictionary<string, object> optionalElementaryTypeFields, 
        Dictionary<string, object[]> optionalArrayFields
        ) {
        DrawOrder drawOrder = (DrawOrder?)optionalElementaryTypeFields["draworder"] ?? DrawOrder.TopDown;
        var objects = Array.ConvertAll(optionalArrayFields["objects"], groupObject => (Object)groupObject);
        if (objects == null) {
            GD.PushError("Parsed objects array of the object group layer is null!");
            return null;     
        }
                
        return new ObjectGroupLayer(layerInfo, objects, drawOrder);
    }

    private GroupLayer FillToGroupLayer(
        LayerInfo layerInfo, 
        Dictionary<string, object[]> optionalArrayFields
        ) {
        Layer[] layers = (Layer[])optionalArrayFields["layers"];
        if (layers == null) {
            GD.PushError("Parsed layers array of the group layer is null!");
            return null;
        }
                
        return new GroupLayer(layerInfo, layers);
    }
}
