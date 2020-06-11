using Mario.Entities.Mario;
using Mario.Levels;
using Microsoft.Xna.Framework;

namespace Mario.Entities.Hazzards
{
    public class Hammer : HazzardEntity
    {
        public override bool SolidTo(IEntity other, Point dir) => false;
        public Hammer(Level l, Point position, Sprite sprite, Sprite deathSprite) : base(l, position, sprite, deathSprite)
        {
            Sprite.Delay = 4;
            HasGravity = true;
            BoundingBox = new Bounds(this, Color.Pink, new Vector2(2, 2), new Vector2(12, 12));
        }
    }
}