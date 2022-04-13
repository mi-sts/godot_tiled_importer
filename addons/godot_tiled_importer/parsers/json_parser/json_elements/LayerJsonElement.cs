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
                { "width", ElementaryType.Int },
                { "height", ElementaryType.Int },
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
                { "encdoding", ElementaryType.Encoding },

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
        layerInfo.width = (int)requiredElementaryTypeFields["width"];
        layerInfo.height = (int)requiredElementaryTypeFields["height"];
        layerInfo.visible = (bool)requiredElementaryTypeFields["visible"];
        layerInfo.type = (LayerType)requiredElementaryTypeFields["type"];
        layerInfo.opacity = (double)requiredElementaryTypeFields["opacity"];


        var optionalArrayFields = ParseOptionalArrayFields(elementDictionary);
        if (optionalArrayFields == null) {
            GD.PushError("Dictionary of the optional array fields is null!");
            return null;
        }
        layerInfo.properties = (Property[])optionalArrayFields["propertires"];


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
        if (optionalArrayFields["chunks"] != null) {
            Chunk[] chunks = (Chunk[])optionalArrayFields["chunks"];
            if (chunks == null) {
                GD.PushError("Parsed chunks array of the infinite tile layer is null!");
                return null;
            }

            return new TileLayer(layerInfo, chunks);
        } else if (optionalArrayFields["layers"] != null) {
            Compression compression = (Compression?)optionalElementaryTypeFields["compression"] ?? Compression.None;
            Encoding encoding = (Encoding?)optionalElementaryTypeFields["encoding"] ?? Encoding.CSV;
            string data = ParserUtils.ToString(elementDictionary.TryGet("data"));
            if (data == null) {
                GD.PushError("Parsed data array of the not infinite tile layer is null!");
                return null;
            }
            TileLayerData layerData = ParseLayerData(
                data,
                layerInfo.width ?? 0,
                layerInfo.height ?? 0,
                encoding,
                compression
            );
            if (layerData == null) {
                GD.PushError("Parsed layer data is null!");
                return null;
            }

            return new TileLayer(layerInfo, layerData);
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
        Object[] objects = (Object[])optionalArrayFields["objects"];
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
}
