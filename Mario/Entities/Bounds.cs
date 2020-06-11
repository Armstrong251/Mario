using System;
using System.Drawing;
using Mario.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utils;
using Color = Microsoft.Xna.Framework.Color;

namespace Mario.Entities
{
    public class Bounds
    {
        public Color Color { get; set; }
        private Vector2 offset;
        private SizeF size;
        public bool Active { get; set; } = true;
        public RectangleF Rectangle
        {
            get
            {
                return new RectangleF(entity.Position + Offset, Size);
            }
        }
        public RectangleF PrevRect
        {
            get
            {
                return new RectangleF(entity.PrevPosition + Offset, Size);
            }
        }

        public RectangleF SweptRect
        {
            get
            {
                RectangleF prev = PrevRect, rect = Rectangle;
                return new RectangleF(PointExt.Min(prev.TL(), rect.TL()).ToPF(), new SizeF(PointExt.Max(prev.BR() - rect.TL(), rect.BR() - prev.TL()).ToPF()));
            }
        }
        public SizeF Size { get => size; set => size = value; }
        public Vector2 Offset { get => offset; set => offset = value; }

#pragma warning disable CA2211 // Non-constant fields should not be visible
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static Texture2D Pixel;
#pragma warning restore CA2211 // Non-constant fields should not be visible
        private Entity entity;


        public Bounds(Entity e, Color color, Vector2 offset, Vector2 size)
        {
            entity = e;
            this.Color = color;
            this.Offset = offset;
            this.Size = new SizeF(size.X, size.Y);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (entity.Level.BoundsVisible)
            {
                spriteBatch.Draw(Pixel, Rectangle.Location.ToVec2(), null, Color * 0.4f, 0, new Vector2(), Rectangle.Size.ToVec2(), SpriteEffects.None, 1f);
            }
        }
        //public void Update()
        //{
        //}
        public bool Intersects(Bounds bounds)
        {
            return SweptRect.IntersectsWith(bounds.SweptRect);
        }
    }
}