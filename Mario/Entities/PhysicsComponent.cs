using System.Drawing;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

namespace Mario.Entities
{
    public class PhysicsComponent
    {
        VectorPoint Position { get; set; }
        VectorPoint Velocity { get; set; }
    }
    public struct VectorPoint
    {
        public float X 
        {
            get => vector.X;
            set => this.vector.X = value;
        }
        public float Y 
        {
            get => vector.Y;
            set => this.vector.Y = value;
        }
        Vector2 vector;
        public VectorPoint(Vector2 vec)
        {
            vector = vec;
        }
        public static VectorPoint operator +(VectorPoint vp, VectorPoint other)
        {
            return new VectorPoint(vp.vector + other.vector);
        }
        public static VectorPoint operator -(VectorPoint vp, VectorPoint other)
        {
            return new VectorPoint(vp.vector - other.vector);
        }
        public static VectorPoint operator +(VectorPoint vp, Vector2 other)
        {
            return new VectorPoint(vp.vector + other);
        }
        public static VectorPoint operator -(VectorPoint vp, Vector2 other)
        {
            return new VectorPoint(vp.vector - other);
        }
        public static VectorPoint operator +(VectorPoint vp, SizeF other)
        {
            return new VectorPoint(new Vector2(vp.X + other.Width, vp.Y + other.Height));
        }
        public static VectorPoint operator -(VectorPoint vp, SizeF other)
        {
            return new VectorPoint(new Vector2(vp.X - other.Width, vp.Y - other.Height));
        }
        public static VectorPoint operator +(VectorPoint vp, Point other)
        {
            return new VectorPoint(vp.vector + other.ToVector2());
        }
        public static VectorPoint operator -(VectorPoint vp, Point other)
        {
            return new VectorPoint(vp.vector - other.ToVector2());
        }
        public static VectorPoint operator *(VectorPoint vectorPoint, float scale)
        {
            return new VectorPoint(vectorPoint.vector * scale);
        }
        public static VectorPoint operator *(VectorPoint vectorPoint, Point point)
        {
            return new VectorPoint(new Vector2(vectorPoint.vector.X * point.X, vectorPoint.vector.Y * point.Y));
        }
        public static implicit operator Vector2(VectorPoint point) => point.vector;
        public static implicit operator Point(VectorPoint point) => point.vector.ToPoint();
        public static implicit operator VectorPoint(Vector2 vector) => new VectorPoint(vector);
        public static implicit operator VectorPoint(Point p) => new VectorPoint(p.ToVector2());
        public static implicit operator VectorPoint(PointF point) => new VectorPoint(new Vector2(point.X, point.Y));
        public static implicit operator PointF(VectorPoint point) => new PointF(point.X, point.Y);
        public static implicit operator SizeF(VectorPoint point) => new SizeF(point.X, point.Y);
        public static bool operator ==(VectorPoint one, VectorPoint two) => one.Equals(two);
        public static bool operator !=(VectorPoint one, VectorPoint two) => !one.Equals(two);
        public override bool Equals(object obj)
        {
            return obj is VectorPoint point &&
                   vector.Equals(point.vector);
        }

        public override int GetHashCode()
        {
            var hashCode = 1358520213;
            hashCode = hashCode * -1521134295 + vector.GetHashCode();
            return hashCode;
        }
    }
}