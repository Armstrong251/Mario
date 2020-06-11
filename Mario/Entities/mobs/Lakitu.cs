
using Mario.Entities.Block;
using Mario.Entities.Mario;
using Mario.Levels;
using Microsoft.Xna.Framework;

namespace Mario.Entities.mobs
{
    class Lakitu : MonsterEntity
    {

        interface ILakituState
        {
            void HandleUpdate();

            void Enter();

        }

        class NormalState : ILakituState
        {

            Lakitu lakitu;
            Sprite sprite;
            ThrowState throwState;

            int spinyTimer;

            public void HandleUpdate()
            {
                spinyTimer--;
                if (spinyTimer <= 0)
                {
                    throwState.Enter();
                }
            }

            public void Enter()
            {
                lakitu.currentState = this;
                lakitu.Sprite = sprite;
                spinyTimer = 180;
            }

            public NormalState(Lakitu lakitu, Sprite standardSprite, Sprite throwSprite)
            {
                this.lakitu = lakitu;
                sprite = standardSprite;
                throwState = new ThrowState(lakitu, throwSprite, this);
            }
            
        }

        class ThrowState : ILakituState
        {

            Lakitu lakitu;
            Sprite sprite;
            NormalState normalState;

            int waitTime;

            public void HandleUpdate()
            {
                waitTime--;
                if (waitTime <= 0)
                {
                    lakitu.ThrowSpiny();
                    normalState.Enter();
                }
            }

            public void Enter()
            {
                lakitu.currentState = this;
                lakitu.Sprite = sprite;
                waitTime = 30;
            }

            public ThrowState(Lakitu lakitu, Sprite sprite, NormalState normalState)
            {
                this.lakitu = lakitu;
                this.sprite = sprite;
                this.normalState = normalState;
            }
            
        }


        ILakituState currentState;

        Vector2 velocity;

        public Lakitu(Level l, Point position, Sprite standardSprite, Sprite throwSprite) : base(l, position, standardSprite, standardSprite)
        {
            BoundingBox = new Bounds(this, Color.Red, new Vector2(2, 2), new Vector2(12, 22));
            HasGravity = false;
            velocity = Vector2.Zero;
            (new NormalState(this, standardSprite, throwSprite)).Enter();
        }

        public override void Update()
        {
            const float speed = 1.5f;
            if (Level.Width - Level.Mario.Position.X > 16*32) {
                const float range = 56;
                
                float posDiff = Position.X - Level.Mario.Position.X;

                if (posDiff < -range)
                {
                    velocity = new Vector2(speed + Level.Mario.Velocity.X, 0);
                }
                if (posDiff > range)
                {
                    velocity = new Vector2(-speed + Level.Mario.Velocity.X, 0);
                }

                Velocity = ((-(range / 2) < posDiff && posDiff < (range / 2)) ? 1.25f : 1.0f) * velocity;

                currentState.HandleUpdate();
                base.Update();
            }
            else
            {
                Velocity = new Vector2(-1.5f * speed, 0);
            }
        }

        public void ThrowSpiny()
        {
            Level.Spawn(typeof(MonsterEntity), Position, Mobs.Spiny);
        }

        public override bool SolidTo(IEntity other, Point dir)
        {
            if (other is BlockEntity)
            {
                return false;
            }
            else
            {
                return base.SolidTo(other, dir);
            }
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is MarioContext && (direction.Y == -1 || ((MarioContext)other).StarState.Active))
            {
                Kill(DeathType.Fall);
                Destroy();
            }
        }
    }
}
