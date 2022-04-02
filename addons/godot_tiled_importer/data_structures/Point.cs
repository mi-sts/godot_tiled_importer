using Godot;
using System;

public struct Point {
    public static Point Zero = new Point(0.0, 0.0);

    public double x { get; private set; }
    public double y { get; private set; }

    public Point(double x, double y) {
        this.x = x;
        this.y = y;
    }
}

public struct IntPoint {
    public static IntPoint Zero = new IntPoint(0, 0);

    public int x { get; private set; }
    public int y { get; private set; }

    public IntPoint(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

