using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities
{
    public class Points : Entity
    {
        private int timer = 40;
        public Points(Level l, Point position, Sprite sprite, PointValue value) : base(l, position, sprite) 
        {
            if(value == PointValue.p1up)
            {
                Level.Game.Lives++;
            }
            else
            {
                Level.Game.Points += 100*(int)value;
            }
        }

        public override bool SolidTo(IEntity other, Point dir) => false;
        public override void Update()
        {
            Position += new Point(0, -1);
            if(--timer == 0) Destroy();
        }
    }
}