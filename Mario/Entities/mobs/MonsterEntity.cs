using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Entities
{
    public enum DeathType
    {
        Squash,
        Fall
    }
    public class MonsterEntity : Entity, IEventObserver
    {
        public override bool Flipped
        {
            set
            {
                Velocity = new Vector2(value ? Speed : -Speed, Velocity.Y);
                base.Flipped = value;
            }
        }
        public virtual bool Damaging { get; set; } = true;
        public virtual bool CommonCollide { get; set; } = true;

        private readonly Sprite squashSprite;
        private float speed = 1;

        public virtual bool Stompable { get; set; } = true;
        public virtual PointValue PointValue => PointValue.p100;
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                Velocity = new Vector2(Flipped ? Speed : -Speed, Velocity.Y);
            }
        }
        public bool Dead { get; set; } = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MonsterEntity(Level l, Point position, Sprite sprite, Sprite squashSprite) : base(l, position, sprite)
        {
            //BoundingBox = new Bounds(this, Color.Red, new Vector2(2, 2), new Vector2(12, 14));
            //HasGravity = true;
            this.squashSprite = squashSprite;
        }
        public override bool SolidTo(IEntity other, Point dir) => other is BlockEntity || other is MonsterEntity;
        public override void Update() { }
        
        public void Kill(DeathType type)
        {
            Kill(type, true);
        }

        public virtual void Kill(DeathType type, bool spawnPoints)
        {
            Dead = true;
            BoundingBox.Active = false;
            switch (type)
            {
                case DeathType.Squash:
                    Commands += new SetSprite(squashSprite).Repeat(1000);
                    Commands += new Command[]
                    {
                        new Move(0, 0).Repeat(50),
                        new MethodCall<IEntity>(e => e.Destroy())
                    };
                    break;
                case DeathType.Fall:
                    HasGravity = true;
                    Velocity = new Vector2(Velocity.X, -4);
                    Sprite.SpriteEffects |= SpriteEffects.FlipVertically;
                    break;
            }
            if (spawnPoints) Level.Spawn(typeof(Points), Position, PointValue);
        }
        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is MonsterEntity && direction == Point.Zero && ((MonsterEntity)other).CommonCollide)
            {
                Position += new Point(Math.Sign(Position.X - other.Position.X), 0);
            }
            if (SolidTo(other, direction) && direction.Y == 0)
            {
                Flipped = (direction.X == -1);
            }
            if (other is MarioContext mario)
            {
                if (mario.StarState.Active)
                {
                    Kill(DeathType.Fall);
                }
                else if (direction.Y == -1)
                {
                    Kill(DeathType.Squash, false);
                }
            }
            else if (other is Fireball
                    || (other is BlockEntity && other.Commands.IsNext<Move>()))
            {
                Kill(DeathType.Fall);
            }
            base.OnCollision(other, direction);
        }
        public void OnEventTriggered(GameEvent e)
        {
        }
    }
}
