using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Mario.Entities.mobs
{
    class BulletBill : MonsterEntity
    {
        public BulletBill(Level l, Point position, Sprite sprite) : base(l, position, sprite, NullSprite.Instance)
        {
            BoundingBox = new Bounds(this, Color.Red, new Vector2(2, 2), new Vector2(12, 12));
            this.HasGravity = false;
            Stompable = true;
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            if(other is BlockEntity)
            {
                this.Destroy();
            }
            if(other is MarioContext)
            {
                Kill(DeathType.Fall);
            }
        }
        public override bool SolidTo(IEntity other, Point dir)
        {
            if(other is BlockEntity)
            {
                return true;
            }
            return false;
        }
        
    }
}
