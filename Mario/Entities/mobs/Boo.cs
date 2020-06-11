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
    class Boo : MonsterEntity
    {


        IBooState currentstate;
        ChaseState chaseState;
        HideState hiddenState;
        bool flipped = false;
        public Boo(Level l, Point position, Sprite hideState, Sprite chaseState) : base(l, position, hideState, null)
        {
            Stompable = false;
            BoundingBox = new Bounds(this, Color.Red, new Vector2(3, 3), new Vector2(13, 13));
            this.chaseState = new ChaseState(this, chaseState);
            this.hiddenState = new HideState(this, hideState);
            currentstate = hiddenState;
            Speed = 1;
        }
        public override bool SolidTo(IEntity other, Point dir) {
            return false;
        }


        public override void Draw(SpriteBatch batch)
        {
            BoundingBox?.Draw(batch);
            Sprite.Draw(batch, Position, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            currentstate.OnCollision(other, direction);
        }

        public override void Update()
        {
            currentstate.Update();
            if (Level.Mario.Position.X > Position.X)
            {
                flipped = true;
            }
            else
            {
                flipped = false;

            }
        }
        abstract class BooState : IBooState
        {
            protected Boo Context;
            protected bool Visible;
            protected BooState(Boo boo)
            {
                this.Context = boo;
                if (boo.Level.Mario.Flipped && boo.Position.X - boo.Level.Mario.Position.X > 0)
                {
                    Visible = true;
                }
            }

            public virtual void Update()
            {
                Visible = Context.Level.Mario.Flipped == (Context.Position.X - Context.Level.Mario.Position.X > 0);
            }

            public virtual void OnCollision(IEntity other, Point direction)
            {
                if(other is MarioContext)
                {
                    if (((MarioContext)other).StarState.Active)
                    {
                        Context.Kill(DeathType.Fall);
                        Context.Destroy();
                    }
                }
            }

            public virtual void Enter()
            {
                Context.currentstate = this;
            }
        }
        interface IBooState
        {
            void Update();

            void Enter();

            void OnCollision(IEntity other, Point direction);
        }
        class ChaseState : BooState
        {
            public Sprite sprite;
            public ChaseState(Boo boo, Sprite chaseSprite) : base(boo)
            {
                this.sprite = chaseSprite;
            }

            public override void Update()
            {
                Context.Damaging = true;
                Context.Sprite = sprite;
                if (Visible) Context.currentstate = Context.hiddenState;
                else
                {
                    Vector2 diff = base.Context.Level.Mario.Position - base.Context.Position;
                    Vector2 newVelocity = diff / (diff.Length());
                    base.Context.Velocity = newVelocity;
                    sprite.Rotation = (float)Math.Atan2(Context.Velocity.Y, Context.Velocity.X) + (Context.Level.Mario.Flipped ? 0 : MathHelper.Pi);
                }
                base.Update();
            }


        }

        class HideState : BooState
        {
            public Sprite sprite;
            public HideState(Boo boo, Sprite sprite) : base(boo)
            {
                this.sprite = sprite;
            }

            public override void Update()
            {
                Context.Damaging = false;
                Context.Sprite = sprite;
                if (!Visible) Context.currentstate = Context.chaseState;
                else
                {
                    base.Context.Velocity = Vector2.Zero;
                }
                base.Update();
            }
        }
    }
}
