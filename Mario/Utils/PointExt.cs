using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

namespace Utils
{
    public static class PointExt
    {
        public static Point Mult(this Point point, int i) => new Point(point.X * i, point.Y * i);
        public static Point Mult(this Point point, float i) => new Point((int)(point.X * i), (int)(point.Y * i));
        public static Point Mult(this Point point, Point other) => new Point(point.X * other.X, point.Y * other.Y);
        public static Point Flip(this Point point) => new Point(point.Y, point.X);
        public static Vector2 Flip(this Vector2 point) => new Vector2(point.Y, point.X);
        public static Point Min(Point one, Point two) => new Point(MathHelper.Min(one.X, two.X), MathHelper.Min(one.Y, two.Y));
        public static Point Max(Point one, Point two) => new Point(MathHelper.Max(one.X, two.X), MathHelper.Max(one.Y, two.Y));
        public static Vector2 Min(Vector2 one, Vector2 two) => new Vector2(MathHelper.Min(one.X, two.X), MathHelper.Min(one.Y, two.Y));
        public static Vector2 Max(Vector2 one, Vector2 two) => new Vector2(MathHelper.Max(one.X, two.X), MathHelper.Max(one.Y, two.Y));
        public static Point Sign(this Point point) => new Point(Math.Sign(point.X), Math.Sign(point.Y));
        public static Point Sign(this Vector2 point) => new Point(Math.Sign(point.X), Math.Sign(point.Y));
        public static PointF ToPF(this Vector2 vector) => new PointF(vector.X, vector.Y);
        public static Vector2 ToVec2(this PointF point) => new Vector2(point.X, point.Y);
        public static Vector2 ToVec2(this SizeF size) => new Vector2(size.Width, size.Height);
    }
}