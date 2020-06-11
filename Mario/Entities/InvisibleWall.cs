using System.Drawing;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Utils;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace Mario.Entities
{
    public class InvisibleWall : Entity
    {
        public RectangleF Area 
        {
            get
            {
                return new RectangleF(Position, Position + BoundingBox.Size);
            }
            
            set
            {
                Position = value.Location;
                BoundingBox = new Bounds(this, Color.Purple, Vector2.Zero, value.Size.ToVec2());
            }
        }
        public InvisibleWall(Level l) : base(l, Point.Zero)
        {
            Solid = true;
            Sprite = NullSprite.Instance;
        }
        public override void Update() { }
    }
}
