
using System;
using System.Diagnostics;
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Input;
using Microsoft.Xna.Framework;
using Utils;
using static Mario.Entities.Mario.ActionEnum;
using static Mario.Input.GameEvent;
using Mario.Entities.mobs;

namespace Mario.Entities.Mario
{
    public enum ActionEnum
    {
        STANDING,
        WALKING,
        JUMPING,
        CROUCHING,
        DEATH,
        FLAG,
        SelfState
    }
    class StandingState : ActionState
    {
        public StandingState(MarioContext context) : base(context) { }

        public override ActionEnum HandleEvent(GameEvent e)
        {
            switch (e)
            {
                case MOVE_LEFT:
                case MOVE_RIGHT:
                    base.HandleEvent(e);
                    return WALKING;
                case JUMP:
                    return JUMPING;
                case CROUCH:
                    if (Context.CollidingWith.Exists(m => m.dir.Y == 1 && m.entity is BlockEntity b && b.BlockStates.CurrentState == States.PipeTop))
                    {
                        /*(*/IEntity entity/*, Point dir)*/ = Context.CollidingWith.Find(m => m.dir.Y == 1 && m.entity is BlockEntity b && b.BlockStates.CurrentState == States.PipeTop).ToTuple().Item1;
                        if(Context.Position.X > entity.Position.X && Context.BoundingBox.Rectangle.Right < entity.BoundingBox.Rectangle.Right)
                            Context.EnterPipe(entity as BlockEntity);
                    }
                    if (Context.CollidingWith.Exists(m => m.dir.Y == 1 && m.entity is BlockEntity b && b.BlockStates.CurrentState == States.Platform))
                    {
                        Context.Drop();
                        Context.Dropping = true;
                        return JUMPING;
                    }
                    return CROUCHING;
            }
            return base.HandleEvent(e);
        }
    }
    class WalkingState : ActionState
    {
        public WalkingState(MarioContext context) : base(context) { }
        public override ActionEnum Update()
        {
            if (Context.Position == Context.PrevPosition && Context.Velocity.X == 0)
            {
                return STANDING;
            }
            if (Context.CollidingWith.Count == 0)
            {
                return JUMPING;
            }
            return SelfState;
        }
        public override ActionEnum HandleEvent(GameEvent e)
        {
            switch (e)
            {
                case JUMP: return JUMPING;
                case MOVE_RIGHT:
                    if (Context.CollidingWith.Exists(m => m.dir.X == 1 && m.entity is BlockEntity b && b.BlockStates.CurrentState == States.PipeSide))
                    {
                        (IEntity entity, _) = Context.CollidingWith.Find(m => m.dir.X == 1 && m.entity is BlockEntity b && b.BlockStates.CurrentState == States.PipeSide);
                        Context.EnterPipe(entity as BlockEntity);
                    }
                    break;
            }
            return base.HandleEvent(e);
        }
    }
    class JumpingState : ActionState
    {
        public int JumpSpeed { get; private set; } = -6;
        public JumpingState(MarioContext context) : base(context) { }
        public override void EnterState(ActionEnum prevState)
        {
            if ((prevState == JUMPING || Context.CollidingWith.Count > 0)&&!Context.Dropping) 
            {
                Context.Velocity = new Vector2(Context.Velocity.X, prevState == JUMPING? JumpSpeed + 2 : JumpSpeed);
                if (Context.PowerUpStates.CurrentState == PowerUpEnum.NORMAL)
                {
                    Context.Level.MusicPlayer.PlaySoundEffect("jump-small");
                }
                else
                {
                    Context.Level.MusicPlayer.PlaySoundEffect("jump-super");
                }
            }
            base.EnterState(prevState);
        }
        public override void ExitState(ActionEnum nextState)
        {
            if (nextState != JUMPING)
            {
                Context.Combo = 0;
                Context.Dropping = false;
            }
        }
        public override ActionEnum HandleCollision(IEntity other, Point direction)
        {
            if (Entity.SolidTo(Context, other, direction) && direction.Y > 0 && !(other is MonsterEntity)) { return STANDING; }
            if (other is MonsterEntity monster 
                    && direction.Y == 1 
                    && monster.Stompable 
                    && !(other is FakeBill)
                    && !Context.StarState.Active)
            {
                Context.Level.Spawn(typeof(Points), other.Position, 
                        (Enum)Enum.GetValues(typeof(PointValue))
                            .GetValue(MathHelper.Min(Context.Combo + 1, Enum.GetValues(typeof(PointValue)).Length - 1)));
                Context.Level.MusicPlayer.PlaySoundEffect("stomp");
                Context.Combo++;
                return ActionEnum.JUMPING;
            }
            return SelfEnum;
        }
    }
    class CrouchingState : ActionState
    {
        public CrouchingState(MarioContext context) : base(context) { }
        public override ActionEnum HandleEvent(GameEvent e)
        {
            switch (e)
            {
                case CROUCH_RELEASE: return STANDING;
                case JUMP: return STANDING;
                case MOVE_LEFT: Context.Flipped = false; return SelfState;
                case MOVE_RIGHT: Context.Flipped = true; return SelfState;
            }
            return base.HandleEvent(e);
        }
        public override ActionEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is BlockEntity && direction.Y != 0 && other.SolidTo(Context, direction.Mult(-1)))
            {
                switch (Context.PowerUpStates.CurrentState)
                {
                    case PowerUpEnum.NORMAL:
                        return STANDING;
                    default:
                        return SelfState;
                }
            }
            return SelfEnum;
        }
    }
    class DeathState : ActionState
    {
        public DeathState(MarioContext context) : base(context) { }

        public override void EnterState(ActionEnum previousState)
        {
            if(previousState == DEATH) return;
            Context.PowerUpStates.State.BoundingBox.Active = false;
            Context.Commands += new Move(Vector2.Zero).Repeat(12);
            Context.Velocity = new Vector2(Context.Velocity.X, -8);
            base.EnterState(previousState);
            MusicPlayer.StopBGM();
        }

        public override ActionEnum HandleEvent(GameEvent e)
        {
            switch (e)
            {
                default: return SelfState;
            }
        }
        public override ActionEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is BlockEntity) { return SelfState; /*no collision*/ }
            return SelfEnum;
        }
    }
    class FlagState : ActionState
    {
        public FlagState(MarioContext context) : base(context) { }
    }
}