using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.mobs;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Linq;
using Utils;

namespace Mario.Entities.Mario
{
    public class MarioContext : Entity, IEventObserver
    {
        public bool Invincible { get; set; } = false;
        public bool DisableEvents { get; set; } = false;
        public int Combo { get; set; }
        private string nextLevel;
        internal StateList<ActionState, ActionEnum> ActionStates { get; }
        internal StateList<PowerUpState, PowerUpEnum> PowerUpStates { get; }
        public StarState StarState { get; }
        public override Sprite SpriteOverride => StarState.Active? StarState.Sprite : PowerUpStates?.State.Sprite;
        public override Bounds BoundingBox => PowerUpStates?.State.BoundingBox;
        public string NextLevel { get => nextLevel; set => nextLevel = value; }
        public bool Dropping { get; set; }

        public MarioContext(Level l, Point position, MarioFactory factory) : base(l, position)
        {
            HasGravity = true;
            ActionStates = factory.CreateActionStates(this);
            PowerUpStates = factory.CreatePowerUpStates(this);
            StarState = factory.CreateStarState(this);
            Level.Collider.UpdateLocation(this);
            Level.Manager.AddObserver(this, new GameEvent[]{
                GameEvent.MOVE_LEFT,
                GameEvent.MOVE_RIGHT,
                GameEvent.JUMP,
                GameEvent.CROUCH,
                GameEvent.CROUCH_RELEASE,
                GameEvent.STANDARD_MARIO,
                GameEvent.SUPER_MARIO,
                GameEvent.FIRE_MARIO,
                GameEvent.DAMAGE,
                GameEvent.FIREBALL,
                GameEvent.STAR
            });
            Flipped = true;
        }
        public void OnEventTriggered(GameEvent e)
        {
            if(DisableEvents) return;
            ActionStates.HandleEvent(e);
            PowerUpStates.HandleEvent(e);
            switch (e)
            {
                case GameEvent.DAMAGE:
                    if (!Invincible)
                    {
                        Invincible = true;
                        PowerUpStates.Handle(s => s.HandleDamage());
                        Commands += new Invincibility().AddCondition(new Repeat(60) | new Until(m => ActionStates.CurrentState == ActionEnum.DEATH));
                    }
                    break;
                case GameEvent.FIRE_MARIO:
                    PowerUpStates.CurrentState = PowerUpEnum.FIRE;
                    break;
                case GameEvent.SUPER_MARIO:
                    PowerUpStates.CurrentState = PowerUpEnum.SUPER;
                    break;
                case GameEvent.STANDARD_MARIO:
                    PowerUpStates.CurrentState = PowerUpEnum.NORMAL;
                    break;
                case GameEvent.STAR:
                    this.StarState.Active = !this.StarState.Active;
                    break;
            }
        }
        public void Drop()
        {
            Commands += new Move(new Point(0, 33));
            Dropping = true;
        }
        public void EnterPipe(BlockEntity pipe)
        {
            PipeState state = (PipeState)pipe.BlockStates.State;
            string level = state.StoredLevel;
            Point dir = pipe.BlockStates.CurrentState == States.PipeTop? new Point(0, 1) : new Point(1, 0);
            if (String.IsNullOrEmpty(level) && Level.Game.Levels.Count == 1) return;
            HasGravity = false;
            BoundingBox.Active = false;
            //Context.Position += new Point(0, WalkSpeed);
            Sprite s = Sprite.Clone();
            s.Layer = 0.1f;
            Commands += new SetSprite(s).Repeat(30);
            Commands += new Command[] { 
                        new Move(dir).Repeat(24), 
                        new MethodCall<IEntity>(f => 
                        {
                            Level l = f.Level.MoveToLevelThroughPipe(level, state.Return);
                            l.Mario.ExitPipe(state.Destination);
                        })
            };
        }
        public void ExitPipe(Point destination)
        {
            if(destination == Point.Zero) return;
            BlockEntity pipe = Level.Map.blockGrid[destination.X, destination.Y];
            Point dir;
            Vector2 exitOffset;
            Predicate<Entity> moveCondition;
            //PipeState state = (PipeState)pipe.BlockStates.State;
            Sprite s;
            if(pipe.BlockStates.CurrentState == States.PipeTop)
            {
                s = Level.Factory.CreateSprite(typeof(MarioContext), PowerUpStates.CurrentState, ActionEnum.STANDING);
                dir = new Point(0, -1);
                exitOffset = new Vector2(8, 8);
                moveCondition = m => m.BoundingBox.Rectangle.Bottom <= pipe.Position.Y;
                var list = Level.Collider.GridAt(destination.Mult(16));
                if(list.Any(e => e is PiranhaPlant))
                {
                    (list.First(e => e is PiranhaPlant) as PiranhaPlant).Kill(DeathType.Fall, false);
                }
            }
            else
            {
                s = Level.Factory.CreateSprite(typeof(MarioContext), PowerUpStates.CurrentState, ActionEnum.WALKING);
                Flipped = true;
                dir = new Point(1, 0);
                exitOffset = new Vector2(pipe.BoundingBox.Rectangle.Height - BoundingBox.Rectangle.Height, -8);
                moveCondition = m => m.Position.X >= pipe.BoundingBox.Rectangle.Right;
            }
            Position = pipe.Position + exitOffset;
            s.Layer = 0.1f;
            Commands += new SetSprite(s).Until(moveCondition);
            Commands += new Command[]{
                new Move(dir).Until(moveCondition),
                new MethodCall<Entity>(e => {e.BoundingBox.Active = true; e.HasGravity = true; })
            };
        }

        public override void Update()
        {
            ActionStates.Update();
            PowerUpStates.Update();
            StarState.Update();
        }
        public override void Destroy()
        {
            Level.Manager.RemoveObserver(this);
            base.Destroy();
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            if (!DisableEvents)
            {
                ActionStates.HandleCollision(other, direction);
                PowerUpStates.HandleCollision(other, direction);
            }
            base.OnCollision(other, direction);
        }

    }
}