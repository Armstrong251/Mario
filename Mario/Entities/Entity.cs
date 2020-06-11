using System.Collections.Generic;
using Mario.Entities.Commands;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Entities.Mario;
using Utils;

namespace Mario.Entities
{

    public delegate void EntityCreated(IEntity e);

    public abstract class Entity : IEntity
    {
        public bool Dependent { get; set; }

        private VectorPoint _position;
        private Bounds boundingBox;
        private Sprite sprite;

        public VectorPoint Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (!Commands.IsNext<Move>() || Commands.Running)
                {
                    if (BoundingBox != null && (Vector2)(value - _position) != Vector2.Zero)
                    {
                        Level.Collider.Remove(this);
                        _position = value;
                        Level.Collider.UpdateLocation(this);
                    }
                    else
                    {
                        _position = value;
                    }
                }
            }
        }
        public virtual bool Flipped { get; set; }
        public VectorPoint PrevPosition { get; set; }
        public VectorPoint Velocity { get; set; }
        public bool HasGravity { get; set; } = false;

        public virtual Bounds BoundingBox
        {
            get => boundingBox; set
            {
                if (boundingBox != null) Level.Collider.Remove(this);
                boundingBox = value;
                Level.Collider.UpdateLocation(this);
            }
        }
        public virtual bool SolidTo(IEntity other, Point dir) => Solid;
        public static bool SolidTo(IEntity one, IEntity two, Point dir) => one.SolidTo(two, dir) && two.SolidTo(one, dir.Mult(-1));
        public bool Solid { get; set; }
        public bool ForceMove { get; set; }
        public List<(IEntity entity, Point dir)> CollidingWith { get; private set; }
        private List<(IEntity entity, Point dir)> newCollidingWith;
        public Level Level { get; set; }
        public Sprite Sprite
        {
            get
            {
                if (Commands.IsNext<SetSprite>()) return Commands.GetNext<SetSprite>().Sprite;
                return SpriteOverride;
            }
            protected set => sprite = value;
        }
        public virtual Sprite SpriteOverride => sprite;
        public CommandComponent Commands { get; set; }
        protected Entity(Level l, Point position) : this(l, position, null) { }
        protected Entity(Level l, Point position, Sprite sprite)
        {
            Level = l;
            Commands = new CommandComponent(this);
            Sprite = sprite;
            Position = PrevPosition = position.ToVector2();
            Dependent = false;
            CollidingWith = new List<(IEntity entity, Point dir)>();
            newCollidingWith = new List<(IEntity entity, Point dir)>();
        }
        public void UpdateStart()
        {
            PrevPosition = Position;
            ForceMove = false;
            if (newCollidingWith.Count > 0 || CollidingWith.Count > 0)
            {
                CollidingWith = newCollidingWith;
                newCollidingWith = new List<(IEntity entity, Point dir)>();
            }
        }
        public abstract void Update();
        public void UpdateBase()
        {
            //BoundingBox?.Update();
            Sprite.Update();
            Update();
            if(HasGravity) Velocity += new Vector2(0, Level.Gravity);
            Position += Velocity;
            Commands.Update();
        }
        public virtual void Draw(SpriteBatch batch)
        {
            BoundingBox?.Draw(batch);
            Sprite.Draw(batch, Position, Flipped? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
        public virtual void Destroy()
        {
            if (BoundingBox != null) BoundingBox.Active = false;
            Level.Destroy(this);
        }

        public virtual void OnCollision(IEntity other, Point direction)
        {
            newCollidingWith.Add((other, direction));
            if (other is KillBox)
            {
                Destroy();
            }
        }
    }
}
