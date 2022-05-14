using Godot;
using System;

namespace TiledImporter.Structures
{
    public struct WangTile
    {
        public static WangTile NullWangTile = new WangTile() { tileID = -1, wangID = new ushort[0] };
        public int tileID { get; private set; }
        public ushort[] wangID { get; private set; }

        public WangTile(int tileID, ushort[] wangID)
        {
            if (wangID == null)
            {
                GD.PushError("Indexes of the wang tile not initialized!");
            }
            this.tileID = tileID;
            this.wangID = wangID ?? new ushort[0];
        }
    }
}
