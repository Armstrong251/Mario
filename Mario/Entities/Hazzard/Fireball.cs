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
    class Fireball : HazzardEntity
    {
        private int timer;
        public Hazards Type { get; }

        public override bool SolidTo(IEntity other, Point dir) => base.SolidTo(other, dir) && Type != Hazards.FirebarFireball;
        public override bool Damages(IEntity other) => (other is MarioContext && Type != Hazards.MarioFireball) || (other is MonsterEntity && Type == Hazards.MarioFireball);

        public Fireball(Level l, Point position, Sprite sprite, Sprite deathsprite, Hazards type) : base(l, position, sprite,deathsprite )
        {
            timer = 70;
            Type = type;
            if(Type == Hazards.FirebarFireball)
            {
                BoundingBox = new Bounds(this, Color.Pink, new Vector2(2, 2), new Vector2(4, 4));
                HasGravity = false;
                Velocity = Vector2.Zero;
            }
            else
            {
                BoundingBox = new Bounds(this, Color.Pink, new Vector2(-1, -1), new Vector2(10, 10));
                HasGravity = true;
                Velocity = new Vector2((l.Mario.Flipped ? 1 : -1) * 3, 0);
                Velocity += Level.Mario.Position - Level.Mario.PrevPosition;
                Flipped = l.Mario.Flipped;
            }
        }

        public override void Update()
        {
            if(Type != Hazards.FirebarFireball)
            {
                timer -= 1;
                if (timer == 0) this.Kill();
            }
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            if(Type != Hazards.FirebarFireball)
            {
                if (other is BlockEntity)
                {
                    if (direction == Point.Zero) Kill();
                    this.Position = this.PrevPosition;
                    if (direction.Y == 1)
                    {
                        this.Velocity = new Vector2(Velocity.X, -4);
                    }
                    if (direction.X != 0)
                    {
                        if ((direction.X == -1) != Flipped)
                        {
                            Velocity *= new Point(-1, 1);
                        }
                        Flipped = direction.X == -1;
                    }

                }
                if (Damages(other))
                {
                    this.Kill();
                }
            }
            base.OnCollision(other, direction);
        }
    }
}
