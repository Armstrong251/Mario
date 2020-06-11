using System.Drawing;
using Microsoft.Xna.Framework;

namespace Utils
{
    public static class RectangleExt
    {
        public static RectangleF OffsetConst(this RectangleF rectangle, Vector2 offset)
        {
            return new RectangleF((rectangle.Location.ToVec2() + offset).ToPF(), rectangle.Size);
        }
        public static Vector2 TL(this RectangleF rectangle) => rectangle.Location.ToVec2();
        public static Vector2 TR(this RectangleF rectangle) => new Vector2(rectangle.Right, rectangle.Top);
        public static Vector2 BL(this RectangleF rectangle) => new Vector2(rectangle.Left, rectangle.Bottom);
        public static Vector2 BR(this RectangleF rectangle) => new Vector2(rectangle.Right, rectangle.Bottom);
    }
}