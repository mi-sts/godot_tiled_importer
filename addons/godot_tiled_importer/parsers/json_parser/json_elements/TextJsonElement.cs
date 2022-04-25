using Godot;
using System;
using System.Collections.Generic;
using GodotCollectionsExtensions;
using TiledImporter.Structures;

namespace TiledImporter.Parsers
{
    public class TextJsonElement : JsonElement
    {
        protected override Dictionary<string, ElementaryType> RequiredElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "text", ElementaryType.String }
            };
            }
        }

        protected override Dictionary<string, ElementaryType> OptionalElementaryTypeFieldsNames
        {
            get
            {
                return new Dictionary<string, ElementaryType>() {
                { "pixelsize", ElementaryType.Int },
                { "bold", ElementaryType.Bool },
                { "italic", ElementaryType.Bool },
                { "fontfamily", ElementaryType.String },
                { "halign", ElementaryType.HorizontalAlignment },
                { "valign", ElementaryType.VerticalAlignment },
                { "kerning", ElementaryType.Bool },
                { "underline", ElementaryType.Bool },
                { "wrap", ElementaryType.Bool }
            };
            }
        }

        public override object Parse(Godot.Collections.Dictionary elementDictionary)
        {
            var requiredElementaryTypeFields = ParseRequiredElementaryTypeFields(elementDictionary);
            if (requiredElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the required elementary type fields is null!");
                return null;
            }
            var textInfo = new TextInfo();
            textInfo.text = (string)requiredElementaryTypeFields["text"];


            var optionalElementaryTypeFields = ParseOptionalElementaryTypeFields(elementDictionary);
            if (optionalElementaryTypeFields == null)
            {
                GD.PushError("Dictionary of the optional elementary type fields is null!");
                return null;
            }
            textInfo.pixelSize = (int)optionalElementaryTypeFields["pixelsize"];
            textInfo.bold = (bool)optionalElementaryTypeFields["bold"];
            textInfo.italic = (bool)optionalElementaryTypeFields["italic"];
            textInfo.fontFamily = (string)optionalElementaryTypeFields["fontfamily"];
            textInfo.halign = (HorizontalAlignment)optionalElementaryTypeFields["halign"];
            textInfo.valign = (VerticalAlignment)optionalElementaryTypeFields["valign"];
            textInfo.kerning = (bool)optionalElementaryTypeFields["kerning"];
            textInfo.underline = (bool)optionalElementaryTypeFields["underline"];
            textInfo.wrap = (bool)optionalElementaryTypeFields["wrap"];

            return new Text(textInfo);
        }
    }
}
