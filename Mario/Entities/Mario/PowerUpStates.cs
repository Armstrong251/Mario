using Mario.Input;
using Microsoft.Xna.Framework;
using static Mario.Entities.Mario.PowerUpEnum;
namespace Mario.Entities.Mario
{
    public enum PowerUpEnum
    {
        NORMAL,
        SUPER,
        FIRE,
        STAR,
        SelfState
    }
    class NormalState : PowerUpState
    {
        public NormalState(MarioContext context, Sprite[] sprites)
            : base(context, sprites, new Bounds(context, Color.Yellow, new Vector2(0, 0), new Vector2(16, 16))) { }
        public override void EnterState(PowerUpEnum previousState)
        {
            if (previousState != NORMAL)
            {
                Context.Level.Collider.UpdateLocation(Context);
                Context.PrevPosition += new Point(0, 16);
                Context.Position += new Point(0, 16);
            }
            base.EnterState(previousState);
        }
        public override PowerUpEnum HandleDamage()
        {
            if(Context.ActionStates.CurrentState != ActionEnum.DEATH)
            {
                Context.Level.MusicPlayer.PlaySoundEffect("Damage");
                Context.ActionStates.CurrentState = ActionEnum.DEATH;
            }
            return SelfEnum;
        }
    }

    class SuperState : PowerUpState
    {
        public SuperState(MarioContext context, Sprite[] sprites)
            : base(context, sprites, new Bounds(context, Color.Yellow, new Vector2(0, 0), new Vector2(16, 32))) {}

        public override void EnterState(PowerUpEnum previousState)
        {
            if (previousState == NORMAL)
            {
                Context.Level.Collider.UpdateLocation(Context);
                Context.PrevPosition -= new Point(0, 16);
                Context.Position -= new Point(0, 16);
            }
            base.EnterState(previousState);
        }
        public override PowerUpEnum HandleDamage()
        {
            return NORMAL;
        }
    }
    class FireState : SuperState
    {
        public FireState(MarioContext context, Sprite[] sprites)
            : base(context, sprites) {}
        public override PowerUpEnum HandleDamage()
        {
            return SUPER;
        }
        public override PowerUpEnum HandleEvent(GameEvent e)
        {
            if(e == GameEvent.FIREBALL)
            {
                Context.Level.MusicPlayer.PlaySoundEffect("fireball");
                Context.Level.Spawn(typeof(HazzardEntity), Context.Position + new Point(Context.Flipped ? 10 : -10, 3), Hazards.MarioFireball);
            }
            return base.HandleEvent(e);
        }
        public override PowerUpEnum HandleCollision(IEntity other, Point direction)
        {
            return base.HandleCollision(other, direction);
        }
    }
}