using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Mario.Levels;
using Mario.Entities.Block;
using Mario.Entities.Mario;

namespace Mario.Entities.mobs
{
    class Spiny : Goomba
    {

        interface ISpinyState
        {
            void OnCollision(IEntity other, Point direction);

            void Enter();
        }

        abstract class SpinyState : ISpinyState
        {
            protected Spiny spiny;

            protected SpinyState(Spiny spiny)
            {
                this.spiny = spiny;
            }

            public virtual void OnCollision(IEntity other, Point direction)
            {
                if (other is Fireball || other is KillBox || (other is MarioContext mario) && mario.StarState.Active)
                {
                    spiny.Kill(DeathType.Fall);
                }
            }

            public virtual void Enter()
            {
                spiny.currentState = this;
            }
        }

        class EggState : SpinyState
        {

            WalkingState walkingState;

            public EggState(Spiny spiny, WalkingState walkingState) : base(spiny)
            {
                this.walkingState = walkingState;
            }
            
            public override void OnCollision(IEntity other, Point direction)
            {
                base.OnCollision(other, direction);
                if (other is BlockEntity)
                {
                    walkingState.Enter();
                }
            }
        }

        class WalkingState : SpinyState
        {

            Sprite walkingSprite;
            VectorPoint walkSpeed;

            public WalkingState(Spiny spiny, Sprite walkingSprite, VectorPoint walkSpeed) : base(spiny) {
                this.walkingSprite = walkingSprite;
                this.walkSpeed = walkSpeed;
            }

            public override void OnCollision(IEntity other, Point direction)
            {
                base.OnCollision(other, direction);
                if (spiny.SolidTo(other, direction) && direction.Y == 0)
                {
                    spiny.Flipped = (direction.X == -1);
                }
            }

            public override void Enter()
            {
                base.Enter();
                spiny.Sprite = walkingSprite;
                spiny.Velocity = walkSpeed;

                if (spiny.Level.Mario.Position.X > spiny.Position.X)
                {
                    spiny.Flipped = true;
                }
            }

        }

        ISpinyState currentState;



        public override bool Stompable => false;

        public Spiny(Level l, Point position, Sprite eggSprite, Sprite walkingSprite) : base(l, position, eggSprite, NullSprite.Instance)
        {
            
            
            //Spinies are thrown.
            WalkingState walkingState = new WalkingState(this, walkingSprite, Velocity);
            Velocity = new Vector2(0, -5.0f);
            (new EggState(this, walkingState)).Enter();
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            currentState.OnCollision(other, direction);
        }
    }
}
