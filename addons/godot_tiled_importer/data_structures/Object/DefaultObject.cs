using Godot;
using System;

namespace TiledImporter.Structures
{
    public class TileObject : DefaultObject
    {
        public TileData objectTileData { get; private set; }
        public Property[] properties { get; private set; }

        public TileObject(
            int id,
            Point position,
            ObjectType type,
            DefaultObjectInfo objectInfo,
            uint gID,
            Property[] properties
            ) : base(id, position, type, objectInfo)
        {
            this.objectTileData = Decoders.Decoder.DecodeGID(gID);
            this.properties = properties ?? new Property[0];
        }
    }
}
