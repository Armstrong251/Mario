using System;
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Hazzards;
using Mario.Input;
using Microsoft.Xna.Framework;

namespace Mario.Entities.mobs
{
    public enum BroEnum { WALKING, JUMPING, THROWING, SelfState }
    public abstract class BroState : IState<BroEnum>
    {
        public virtual bool SolidTo(IEntity other, Point dir) => other is BlockEntity;
        public BroEnum SelfEnum => BroEnum.SelfState;

        public Sprite Sprite { get; }
        public HammerBro Context { get; }

        protected BroState(Sprite sprite, HammerBro context)
        {
            Sprite = sprite;
            Context = context;
        }
        public virtual void EnterState(BroEnum previousState)
        {
        }
        public virtual void ExitState(BroEnum nextState)
        {
        }
        public virtual BroEnum HandleCollision(IEntity other, Point direction)
        {
            return SelfEnum;
        }
        public virtual BroEnum HandleEvent(GameEvent e)
        {
            return SelfEnum;
        }
        public virtual BroEnum Update()
        {
            Context.Flipped = Context.Level.Mario.Position.X > Context.Position.X;
            return SelfEnum;
        }
    }
    public class BroWalkingState : BroState
    {
        private int timer;

        public BroWalkingState(Sprite sprite, HammerBro context) : base(sprite, context)
        {
        }

        public override void EnterState(BroEnum previousState)
        {
            Context.Velocity = new Vector2(Context.Velocity.X == 0? 0.25f : -Context.Velocity.X, 0);
            timer = 64;
        }
        public override BroEnum Update()
        {
            if(Context.Dead) return SelfEnum;
            timer--;
            if(timer % 32 == 0 && HammerBro.Rand.Next() % 2 != 0)
            {
                Hammer h = (Hammer)Context.Level.Spawn(typeof(HazzardEntity), Context.Position, Hazards.Hammer);
                h.BoundingBox.Active = false;
                Context.Commands += new Command[]
                {
                    new MethodCall<IEntity>(e => h.Position = e.Position + new Vector2(2, -8)).Repeat(16),
                    new MethodCall<IEntity>(e => { h.Velocity = new Vector2(Context.Flipped? 2.5f : -2.5f, -5); h.BoundingBox.Active = true; })
                };
                Context.Commands += new SetSprite(Context.BroStates[BroEnum.THROWING].Sprite).Repeat(16);
            }
            if(timer < 0) 
            {
                int i = HammerBro.Rand.Next() % 2;
                switch(i)
                {
                    case 0: return BroEnum.WALKING;
                    case 1: return BroEnum.JUMPING;
                }
            }
            return base.Update();
        }
    }
    public class BroJumpingState : BroState
    {
        public override bool SolidTo(IEntity other, Point dir) => other is BlockEntity &&
        (
            jumpUp && dir.Y == 1
            ||
            !jumpUp && Context.PrevPosition.Y > level
        );
        bool jumpUp;
        float level;

        public BroJumpingState(Sprite sprite, HammerBro context) : base(sprite, context)
        {
        }

        public override void EnterState(BroEnum previousState)
        {
            level = Context.Position.Y;
            jumpUp = HammerBro.Rand.Next() % 2 == 0;
            if(Context.BoundingBox.Rectangle.Bottom >= 288) jumpUp = true;
            Context.Velocity += new Vector2(0, jumpUp? -7 : -3);
        }
        public override BroEnum HandleCollision(IEntity other, Point direction)
        {
            if(SolidTo(other, direction) && direction.Y == 1) 
            {
                return BroEnum.WALKING;
            }
            return base.HandleCollision(other, direction);
        }

        public override BroEnum HandleEvent(GameEvent e)
        {
            return base.HandleEvent(e);
        }
        public override BroEnum Update()
        {
            return SelfEnum;
        }
    }
    public class BroThrowingState : BroState
    {
        public BroThrowingState(Sprite sprite, HammerBro context) : base(sprite, context) { }
    }
}