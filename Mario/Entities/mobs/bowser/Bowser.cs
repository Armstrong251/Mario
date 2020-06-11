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
    public class Bowser : MonsterEntity
    {
        public bool AI { get; set; } = false;
        public int HP { get; set; } = 10;
        public override bool SolidTo(IEntity other, Point dir) => base.SolidTo(other, dir) || other is MarioContext && dir.Y == -1;
        public Bowser(Level l, Point position) : base(l, position, NullSprite.Instance, NullSprite.Instance) {
            BoundingBox = new Bounds(this, Color.Red, new Vector2(3, 3), new Vector2(29, 29));
            HasGravity = true;
        }
        public void Damage()
        {
            if (--HP == 0)
            {
                this.Kill(DeathType.Fall);
            }
        }
        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is MonsterEntity && direction == Point.Zero)
            {
                Position += new Point(Math.Sign(Position.X - other.Position.X), 0);
            }
            if (SolidTo(other, direction) && direction.Y == 0)
            {
                Flipped = (direction.X == -1);
            }
            if (other is MarioContext mario)
            {
                if (mario.StarState.Active)
                {
                    Kill(DeathType.Fall);
                }
                else if (direction.Y == -1)
                {
                    Damage();
                }
            }
            else if (other is Fireball || (other is BlockEntity && other.Commands.IsNext<Move>()))
            {
                Damage();
            }
        }

    }
    public class BowserWeak : Bowser
    {
        public override Sprite SpriteOverride => States.State.Sprite;
        public StateList<BowserState, BowserEnum> States { get; set; }

        public BowserWeak(Level l, Point position) : base(l,position)
        {
        }
        public override void Update()
        {
            States.Update();
            base.Update();
        }

    }
    public class AIBowser : Bowser
    {
        public override Sprite SpriteOverride => States.State.Sprite;

        public StateList<BowserState, BowserEnum> States { get; set; }

        public AIBowser(Level l, Point position) : base(l, position) {
            AI = true;
        }
        public override void Update()
        {
            States.Update();
            base.Update();
        }
       
    }

}

