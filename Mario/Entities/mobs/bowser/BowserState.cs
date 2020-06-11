using System;
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Microsoft.Xna.Framework;
namespace Mario.Entities.mobs
{
    public enum BowserEnum
    {
        Walking,
        Dashing,
        Breathing,
        Jumping,
        SelfState,
    }
    public abstract class BowserState : IState<BowserEnum>
    {
        public Bowser Context { get; set; }
        protected int Delay { get; set; }
        protected Random Rnd { get; set; } = new Random();
        private MarioContext mario;
        protected BowserState(Sprite sprite, Bowser context)
        {
            mario = context.Level.Mario;
            Sprite = sprite;
            Context = context;
            Delay = 10;
        }
        public Sprite Sprite { get; }

        public BowserEnum SelfEnum => BowserEnum.SelfState;

        public virtual void EnterState(BowserEnum previousState) { }

        public virtual void ExitState(BowserEnum previousState) { }

        public virtual BowserEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is MarioContext)
            {
                if (((MarioContext)other).StarState.Active)
                {
                    Context.Kill(DeathType.Fall);
                }
            }
            else if (other is HazzardEntity&& !(other is Bowserfire))
            {
                Context.Damage();
            }
            return SelfEnum;
        }

        public virtual BowserEnum HandleEvent(GameEvent e) { return SelfEnum; }

        public virtual BowserEnum Update() {
            if (Context.AI)
            {
                if (Delay-- == 0)
                {
                    Delay = 20;
                    switch (mario.ActionStates.CurrentState)
                    {
                        case ActionEnum.STANDING:
                        case ActionEnum.WALKING:
                        case ActionEnum.CROUCHING:
                            return (BowserEnum)Rnd.Next(1, 2);
                        case ActionEnum.JUMPING:
                            return BowserEnum.Jumping;
                    }
                }
            }
            else
            {
                if (Delay-- == 0)
                {
                    return (BowserEnum)(Rnd.Next(0, 1000) % 3) ;
                }
            }
            return SelfEnum; }

    }

    public class BowserWalkingState : BowserState
    {
        public BowserWalkingState(Sprite sprite, Bowser context) : base(sprite, context) { }
        public override void EnterState(BowserEnum previousState)
        {
            Context.Speed = .75f;
            Context.PrevPosition = Context.Position;
            Context.Flipped = Context.Level.Mario.Position.X > Context.Position.X;
            if(!Context.AI)Delay = Rnd.Next(100,120);
        }
    }

    public class BowserBreathingState: BowserState
    {
        public BowserBreathingState(Sprite sprite, Bowser context) : base(sprite, context) { }
        private int timer = 5;
        public override void EnterState(BowserEnum previousState)
        {
            Context.Speed = 0;
            Context.PrevPosition = Context.Position;
            Context.Velocity = Vector2.Zero;
            Context.Flipped = Context.Level.Mario.Position.X > Context.Position.X;
            if (!Context.AI) Delay = Rnd.Next(60,80);
            
        }
        public override BowserEnum Update()
        {
            if (timer-- == 0)
            {
                Entity temp =Context.Level.Spawn(typeof(HazzardEntity), Context.Position + new Point(Context.Flipped ? 10 : -10, 10), Hazards.Bowserfire);
                temp.Flipped = Context.Flipped;
                temp.Velocity = new Vector2((Context.Flipped ? 1 : -1) * 3, 0);
                temp.Velocity +=Context.Position - Context.PrevPosition;
                timer = 5;
            }
            return base.Update();
        }

    }

    public class BowserDashingState:BowserState
    {
        public BowserDashingState(Sprite sprite, Bowser context) : base(sprite, context) { }
        public override void EnterState(BowserEnum previousState)
        {
            Context.Speed = 2f;
            Context.PrevPosition = Context.Position;
            Context.Flipped = Context.Level.Mario.Position.X > Context.Position.X;
            if (!Context.AI) Delay = Rnd.Next(10,30);
        }
    }

    public class BowserJumpingState : BowserState
    {
        public BowserJumpingState(Sprite sprite, Bowser context) : base(sprite, context) { }

        public override void EnterState(BowserEnum previousState)
        {
            Context.Speed = .75f;
            Context.PrevPosition = Context.Position;
            Context.Flipped = Context.Level.Mario.Position.X > Context.Position.X;
            if (Context.Velocity.Y == 0)
            {
                Vector2 diff = base.Context.Level.Mario.Position - base.Context.Position;
                Vector2 newVelocity = diff / (diff.Length()) * 6;
                Context.Velocity = newVelocity;
            }
        }
    }

}
