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
    class SuperBill : BulletBill
    {
        public float ForceTowardsMario = 100;
        public SuperBill(Level l, Point position, Sprite sprite) : base(l, position, sprite)
        {
            Stompable = true;
        }
        public override void Update()
        {
            var dist = (Vector2)(Level.Mario.Position - Position);
            Vector2 accel = dist * ForceTowardsMario / (dist.LengthSquared() * dist.Length());
            float angle = (float)Math.Acos(Vector2.Dot(accel, Velocity) / (accel.Length() * ((Vector2)Velocity).Length()));
            if(angle < MathHelper.PiOver4)
            {
                Velocity += accel;
                Sprite.Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + (Flipped ? 0 : MathHelper.Pi);
                base.Update();
            }
        }
    }
   
}
