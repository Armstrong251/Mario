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
    class FakeBill : SuperBill
    {
        private bool close = false;
        public FakeBill(Level l, Point position, Sprite sprite) : base(l, position, sprite )
        {
            Stompable = true;
            Damaging = false;
        }

        public override void Update()
        {
            if (Math.Abs(Level.Mario.Position.X - this.Position.X) <= 5 && Math.Abs(Level.Mario.Position.Y - this.Position.Y) <= 5)
            {
                this.Velocity = Vector2.Zero;
                this.Velocity -= new Vector2(0, 3);
                close = true;
                float angle = (float)Math.Acos(Vector2.Dot(Velocity, Velocity) / (((Vector2)Velocity).Length() * ((Vector2)Velocity).Length()));
                if (angle < MathHelper.PiOver4)
                {
                    Sprite.Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + (Flipped ? 0 : MathHelper.Pi);
                }
            }
            else
            {
                if(!close) base.Update();
            }
        }
        public override void OnCollision(IEntity other, Point direction)
        {
            if(other is BlockEntity)
            {
                this.Destroy();
            }
        }

    }
}
