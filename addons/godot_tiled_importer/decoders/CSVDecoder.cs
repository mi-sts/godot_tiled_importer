using System;
using System.Linq;
using Godot;
using TiledImporter.Structures;

namespace TiledImporter.Decoders
{
    public class CSVDecoder : Decoder
    {
        public TileLayerData Decode(uint[] tileIDs, int layerWidth, int layerHeight)
        {
            if (tileIDs == null)
            {
                GD.PushError("Tile GIDs is null!");
                return null;
            }

            bool[][] flipFlags = DecodeFlipFlagsAndClear(ref tileIDs);

            return CreateLayerData(tileIDs, flipFlags, layerWidth, layerHeight);
        }
    }
}
