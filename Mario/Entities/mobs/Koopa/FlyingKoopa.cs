using Mario.Entities.Mario;
using Microsoft.Xna.Framework;

namespace Mario.Entities.mobs
{
    public class KoopaFlyingState : KoopaState
    {
        public override bool SolidTo(IEntity entity, Point direction) => entity is MarioContext && direction.Y == -1;
        public KoopaFlyingState(Sprite sprite, Koopa context) : base(sprite, context)
        {
        }
        public override void EnterState(KoopaEnum previousState)
        {
            if (Context is GreenKoopa) Context.Speed = 1;
            else Context.Speed = 0;
            Context.BoundingBox = new Bounds(Context, Color.Red, new Vector2(2, 8), new Vector2(12, 16));
            Context.PrevPosition -= new Point(0, 8);
            Context.Position = Context.PrevPosition;
            Context.Damaging = true;
            Context.Flipped = Context.Level.Mario.Position.X > Context.Position.X;
        }

        public override KoopaEnum Update()
        {
            if(!Context.Dead) Context.Velocity += new Vector2(0, -0.125f);
            return SelfEnum;
        }
        public override KoopaEnum HandleCollision(IEntity other, Point direction)
        {
            if (other is MarioContext && direction.Y == -1)
            {
                return KoopaEnum.WALKING;
            }
            else if (Context.SolidTo(other, direction) && direction.Y == 1)
            {
                Context.Velocity = new Vector2(Context.Velocity.X, -2.875f);
            }
            return base.HandleCollision(other, direction);
        }
    }
}