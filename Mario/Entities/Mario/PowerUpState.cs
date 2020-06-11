

using Mario.Entities.mobs;
using Microsoft.Xna.Framework;

namespace Mario.Entities.Mario
{
    public abstract class PowerUpState : MarioState<PowerUpEnum>
    {
        public override PowerUpEnum SelfEnum => PowerUpEnum.SelfState;
        private Sprite[] sprites;
        public Sprite Sprite
        {
            get
            {
                return sprites[(int)Context.ActionStates.CurrentState];
            }
        }
        public Bounds BoundingBox { get; }

        protected PowerUpState(MarioContext context, Sprite[] sprites, Bounds box) : base(context)
        {
            this.sprites = sprites;
            foreach (var s in sprites) s.Layer = 0.55f;
            Context.Solid = true;
            this.BoundingBox = box;
        }
        public override PowerUpEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is ItemEntity)
            {
                switch (((ItemEntity)other).Type)
                {
                    case ItemType.MUSHROOM:
                        if (Context.PowerUpStates.CurrentState == PowerUpEnum.NORMAL)
                        {
                            Context.Level.MusicPlayer.PlaySoundEffect("powerup");
                            return PowerUpEnum.SUPER;
                        }
                        break;
                    case ItemType.FIRE_FLOWER:
                        if (Context.PowerUpStates.CurrentState != PowerUpEnum.FIRE)
                        {
                            Context.Level.MusicPlayer.PlaySoundEffect("powerup");
                            return PowerUpEnum.FIRE;
                        }
                        break;
                    case ItemType.STAR:
                        Context.StarState.Active = true;
                        break;
                }
            }
            else if (other is MonsterEntity monster && (direction.Y != 1 || !monster.Stompable) && monster.Damaging)
            {
                Context.OnEventTriggered(Input.GameEvent.DAMAGE);
            }
            else if(other is HazzardEntity h && h.Damages(Context))
            {
                Context.OnEventTriggered(Input.GameEvent.DAMAGE);
            }
            return SelfEnum;
        }
        public virtual PowerUpEnum HandleDamage() { return SelfEnum; }
    }
}