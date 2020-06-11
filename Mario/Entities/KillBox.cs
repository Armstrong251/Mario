using System.Drawing;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Utils;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace Mario.Entities
{
    class KillBox : Entity, IEventObserver
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
        public KillBox(Level l) : base(l, Point.Zero)
        {
            Sprite = NullSprite.Instance;
        }
        public override void OnCollision(IEntity other, Point direction)
        {

        }

        public void OnEventTriggered(GameEvent e)
        {

        }

        public override void Update()
        {

        }
    }
}
