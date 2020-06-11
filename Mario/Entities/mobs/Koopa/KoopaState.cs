using System;
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Microsoft.Xna.Framework;
using static Mario.Entities.mobs.KoopaEnum;

namespace Mario.Entities.mobs
{
    public enum KoopaEnum
    {
        WALKING,
        IN_SHELL,
        IN_SHELL_MOVING,
        IN_SHELL_TRANSITION,
        FLYING,
        SelfState,
    }
    public abstract class KoopaState : IState<KoopaEnum>
    {
        public Koopa Context { get; set; }
        public KoopaEnum SelfEnum => SelfState;
        public Sprite Sprite { get; }
        public virtual bool SolidTo(IEntity entity, Point direction) => false;
        protected KoopaState(Sprite sprite, Koopa context) 
        {
            Sprite = sprite;
            Context = context;
        }
        public virtual void EnterState(KoopaEnum previousState) { }
        public virtual void ExitState(KoopaEnum nextState) { }
        public virtual KoopaEnum HandleCollision(IEntity other, Point direction) 
        {
            if (other is MonsterEntity && direction == Point.Zero)
            {
                Context.Position += new Point(Math.Sign(Context.Position.X - other.Position.X), 0);
            }
            if (Context.SolidTo(other, direction) && direction.Y == 0)
            {
                Context.Flipped = (direction.X == -1);
            }
            if (other is MarioContext mario)
            {
                if (mario.StarState.Active)
                {
                    Context.Kill(DeathType.Fall);
                }
            }
            else if (other is HazzardEntity
                    || (other is BlockEntity && other.Commands.IsNext<Move>()))
            {
                Context.Kill(DeathType.Fall);
            }
            return SelfEnum;
        }

        public virtual KoopaEnum HandleEvent(GameEvent e) { return SelfEnum; }

        public virtual KoopaEnum Update() { return SelfEnum; }
    }
    public class KoopaWalkingState : KoopaState
    {
        public override bool SolidTo(IEntity entity, Point direction) => entity is MarioContext && direction.Y == -1;
        public KoopaWalkingState(Sprite sprite, Koopa context) : base(sprite, context) { }
        public override void EnterState(KoopaEnum previousState)
        {
            Context.Speed = 1;
            Context.BoundingBox = new Bounds(Context, Color.Red, new Vector2(2, 8), new Vector2(12, 16));
            Context.Damaging = true;
            if(previousState != KoopaEnum.FLYING)
            {
                Context.PrevPosition -= new Point(0, 8);
                Context.Position = Context.PrevPosition;
                Context.Flipped = Context.Level.Mario.Position.X > Context.Position.X;
            }
        }
        public override KoopaEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is MarioContext mario && !mario.StarState.Active && direction.Y == -1)
            {
                return IN_SHELL_TRANSITION;
            }
            else
            {
                return base.HandleCollision(other, direction);
            }
        }
    }
    public class KoopaShellState : KoopaState
    {
        public override bool SolidTo(IEntity entity, Point direction) => true;
        private int timer;
        public KoopaShellState(Sprite sprite, Koopa context) : base(sprite, context) { }
        public override void EnterState(KoopaEnum previousState)
        {
            Context.Damaging = false;
            Context.Speed = 0;
            Context.Flipped = Context.Flipped;
            timer = 60 * 3;
        }

        public override KoopaEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is MarioContext mario && !mario.StarState.Active)
            {
                return IN_SHELL_MOVING;
            }
            else
            {
                return base.HandleCollision(other, direction);
            }
        }
        public override KoopaEnum Update()
        {
            if (--timer == 0) return IN_SHELL_TRANSITION;
            return base.Update();
        }
    }
    public class KoopaShellMovingState : KoopaState
    {
        public override bool SolidTo(IEntity entity, Point direction) => direction.Y == -1;
        int combo = 0;
        public override void EnterState(KoopaEnum previousState)
        {
            Context.Speed = 2.2f;
            Context.Damaging = true;
            Context.Flipped = Context.Level.Mario.Position.X < Context.Position.X;
        }
        public override KoopaEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is MarioContext mario && !mario.StarState.Active && direction.Y == -1)
            {
                return IN_SHELL;
            }
            else if (other is MonsterEntity monster)
            {
                monster.Kill(DeathType.Fall, false);
                Context.Level.Spawn(typeof(Points), other.Position,
                        (Enum)Enum.GetValues(typeof(PointValue))
                            .GetValue(MathHelper.Min(combo + 1, Enum.GetValues(typeof(PointValue)).Length - 1)));
                Context.Level.MusicPlayer.PlaySoundEffect("stomp");
                combo++;
                return SelfEnum;
            }
            else
            {
                return base.HandleCollision(other, direction);
            }
        }

        public KoopaShellMovingState(Sprite sprite, Koopa context) : base(sprite, context) { }
    }
    public class KoopaShellTransitionState : KoopaState
    {
        int timer;
        KoopaEnum destination;

        public KoopaShellTransitionState(Sprite sprite, Koopa context) : base(sprite, context) { }

        public override void EnterState(KoopaEnum previousState)
        {
            if (previousState == WALKING)
            {
                Context.BoundingBox = new Bounds(Context, Color.Red, new Vector2(2,2), new Vector2(12, 14));
                Context.PrevPosition += new Point(0, 8);
                Context.Damaging = false;
                Context.Position = Context.PrevPosition;
                destination = IN_SHELL;
            }
            else
            {
                destination = WALKING;
            }
            timer = 5;
        }
        public override KoopaEnum Update()
        {
            if (--timer == 0) return destination;
            return base.Update();
        }
    }
}