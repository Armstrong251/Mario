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
    class TurboBill : MonsterEntity
    {
        private int delay = 0;
        public TurboBill(Level l, Point position, Sprite sprite) : base(l, position, sprite, NullSprite.Instance)
        {
            BoundingBox = new Bounds(this, Color.Red, new Vector2(3, 3), new Vector2(13, 13));
            Velocity = Vector2.Zero;
            Damaging = true;
        }
        public override void Update()
        {
            if (delay == 0)
            {
                Vector2 diff = Level.Mario.Position - this.Position;
                Vector2 newVelocity = diff / (diff.Length() * 2);
                Velocity = newVelocity;
                Sprite.Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + (Flipped ? 0 : MathHelper.Pi);
            }
            else
            {
                delay--;
            }
            }
        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is BlockEntity) this.Destroy();
            if (other is MarioContext)
            {
                if (((MarioContext)other).StarState.Active)
                {
                    this.Destroy();

                }
                else {
                    if (direction.Y == -1)
                    {
                        delay = 40;
                        Position += new Vector2(0, 8);
                    }
                }
            }
        }
    }
}
