using Godot;
using System;
using System.Linq;

namespace TiledImporter.Structures
{
    public enum MapOrientation { Orthogonal, Isometric, Staggered, Hexagonal }

    public enum RenderOrder { RightDown, RightUp, LeftDown, LeftUp }

    public enum StaggerAxis { X, Y }

    public enum StaggerIndex { Odd, Even }

    public struct MapInfo
    {
        public Layer[] layers;
        public int? width;
        public int? height;
        public MapOrientation? mapOrientation;
        public Property[] properties;
        public int? tileWidth;
        public int? tileHeight;
        public TileSet[] tileSets;
        public int? hexSideLength;
        public Color? backgroundColor;
        public bool? infinite;
        public Point? parallaxOriginPoint;
        public RenderOrder? renderOrder;
        public StaggerAxis? staggerAxis;
        public StaggerIndex? staggerIndex;
        public string tiledVersion;
        public string type;
    }

    public class Map
    {
        public Layer[] layers { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public MapOrientation mapOrientation { get; private set; }
        public Property[] properties { get; private set; } // optional.
        public int tileWidth { get; private set; }
        public int tileHeight { get; private set; }
        public TileSet[] tileSets { get; private set; }
        public int? hexSideLength { get; private set; } // Length of the side of a hex tile in pixels (hexagonal maps only).
        public Color? backgroundColor { get; private set; } // (optional).
        public bool infinite { get; private set; }
        public Point parallaxOriginPoint { get; private set; } // Parallax origin point (default: (0, 0)).
        public RenderOrder renderOrder { get; private set; } // (default: right-down).
        public StaggerAxis? staggerAxis { get; private set; } // Staggered / hexagonal maps only.
        public StaggerIndex? staggerIndex { get; private set; } // Staggered / hexagonal maps only.
        public string tiledVersion { get; private set; }
        public string type { get; private set; } // map (since 1.0).

        public Map(MapInfo mapInfo)
        {
            var requiredFields = new object[] {
            mapInfo.layers,
            mapInfo.width,
            mapInfo.height,
            mapInfo.mapOrientation,
            mapInfo.tileWidth,
            mapInfo.tileHeight,
            mapInfo.tileSets,
            mapInfo.infinite,
            mapInfo.tiledVersion
        };
            if (requiredFields.Any(field => field == null))
            {
                GD.PushError("Not all of the required map parameters are initialized!");
            }

            layers = mapInfo.layers ?? new Layer[0];
            width = mapInfo.width ?? 0;
            height = mapInfo.height ?? 0;
            mapOrientation = mapInfo.mapOrientation ?? MapOrientation.Orthogonal;
            tileWidth = mapInfo.tileWidth ?? 0;
            tileHeight = mapInfo.tileHeight ?? 0;
            tileSets = mapInfo.tileSets ?? new TileSet[0];
            infinite = mapInfo.infinite ?? false;
            tiledVersion = mapInfo.tiledVersion ?? "";

            hexSideLength = mapInfo.hexSideLength;
            backgroundColor = mapInfo.backgroundColor;
            parallaxOriginPoint = mapInfo.parallaxOriginPoint ?? new Point(0, 0);
            renderOrder = mapInfo.renderOrder ?? RenderOrder.RightDown;
            staggerAxis = mapInfo.staggerAxis;
            staggerIndex = mapInfo.staggerIndex;
            properties = mapInfo.properties ?? new Property[0];
        }
    }
}
