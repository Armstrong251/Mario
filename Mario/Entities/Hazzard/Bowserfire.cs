using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Mario.Entities
{
    class Bowserfire : HazzardEntity
    {
        public Bowserfire(Level l, Point position, Sprite sprite) : base(l, position, sprite, NullSprite.Instance) {
            BoundingBox = new Bounds(this, Color.Pink, new Vector2(2, 2), new Vector2(10, 10));
            HasGravity = false;
        }
        public override void OnCollision(IEntity other, Point direction)
        {
            
            if (other is BlockEntity)
            {
                this.Destroy();
            }
            
        }
    }
}
