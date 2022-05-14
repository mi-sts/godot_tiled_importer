using Godot;
using System;

namespace TiledImporter.Structures
{
    public struct Point
    {
        public static Point Zero = new Point(0.0, 0.0);

        public double x { get; private set; }
        public double y { get; private set; }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator -(Point point)
            => new Point(-point.x, -point.y);

        public static Point operator +(Point point)
            => point;

        public static Point operator +(Point firstPoint, Point secondPoint)
            => new Point(firstPoint.x + secondPoint.x, firstPoint.y + secondPoint.y);

        public static Point operator -(Point firstPoint, Point secondPoint)
            => firstPoint - secondPoint;
    }

    public struct IntPoint
    {
        public static IntPoint Zero = new IntPoint(0, 0);

        public int x { get; private set; }
        public int y { get; private set; }

        public IntPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static IntPoint operator -(IntPoint point)
            => new IntPoint(-point.x, -point.y);

        public static IntPoint operator +(IntPoint point)
            => point;

        public static IntPoint operator +(IntPoint firstPoint, IntPoint secondPoint)
            => new IntPoint(firstPoint.x + secondPoint.x, firstPoint.y + secondPoint.y);

        public static IntPoint operator -(IntPoint firstPoint, IntPoint secondPoint)
            => firstPoint - secondPoint;
    }
}
