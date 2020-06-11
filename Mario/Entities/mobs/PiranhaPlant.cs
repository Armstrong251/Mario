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
    class PiranhaPlant : MonsterEntity
    {
        private bool toggle = true;
        private int timer = 100;
        private VectorPoint originalposition;
        public override bool Stompable => false;
        public PiranhaPlant(Level l, Point position, Sprite sprite) : base(l, position, sprite, NullSprite.Instance)
        {
            BoundingBox = new Bounds(this, Color.Red, new Vector2(2, 2), new Vector2(12, 22));

            HasGravity = false;
            this.Position -= new Point(-7, 8);
            this.PrevPosition = this.Position;
            this.Sprite.Layer = 0f;
            originalposition = position;
            this.Velocity = Vector2.Zero;

        }

        public override void Update()
        {
            if (!Dead)
            {
                if (Math.Abs(Level.Mario.Position.X - Position.X) > 32 || Position.Y < originalposition.Y - 16)
                {
                    if (toggle)
                    {
                        timer--;
                        this.Commands += new Move(0, -.25f);
                    }
                    else
                    {
                        timer--;
                        this.Commands += new Move(0, .25f);
                    }
                    if (timer == 0)
                    {
                        timer = 100;
                        toggle = !toggle;
                    }
                }
                this.Commands += new Move(0, 0);
            }
        }
        public override bool SolidTo(IEntity other, Point dir) => other is MonsterEntity;

        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is HazzardEntity || (other is MarioContext mario && mario.StarState.Active))
            {
                this.Kill(DeathType.Fall);
            }
        }
        public override void Kill(DeathType type, bool spawnPoints = true)
        {
            BoundingBox.Active = false;
            if (spawnPoints) Level.Spawn(typeof(Points), Position, PointValue.p200);
            Velocity = new Vector2(Velocity.X, +4);
            Sprite.SpriteEffects |= SpriteEffects.FlipVertically;
            Dead = true;
        }





    }
}
