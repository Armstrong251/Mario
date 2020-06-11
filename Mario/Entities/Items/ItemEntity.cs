using Mario.Entities.Mario;
using Mario.Input;
using Microsoft.Xna.Framework;
using Mario.Entities.Block;
using System.Diagnostics;
using Mario.Entities.Commands;
using Mario.Levels;
using System;

namespace Mario.Entities
{
    public enum ItemType
    {
        MUSHROOM,
        ONEUP,
        FIRE_FLOWER,
        STAR,
        BLOCK_COIN,
        WORLD_COIN,
        LEVEL_HOLDER,
    }
    class ItemEntity : Entity, IEventObserver
    {
        public ItemType Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemEntity(Level l, Point position, Sprite sprite, ItemType type) : base(l, position, sprite)
        {
            Type = type;
            sprite.Layer = 0.45f;
            //if(type != ItemType.COIN) HasGravity = true;
            //if(type ==  ItemType.COIN)
            //{
            //    BoundingBox = new Bounds(this, Color.Green, new Point(-2, -2), new Point(12, 18));
            //    Position += new Point(4, 0);
            //    PrevPosition = Position;
            //}

        }
        public override void Update()
        {
            //if (Type != ItemType.COIN && Type != ItemType.FIRE_FLOWER) {
            //    this.Position += new Point(Flipped?-1:1, 0);
            //}
            // if(Type == ItemType.COIN && Velocity != Point.Zero) Velocity = Point.Zero;
        }
        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is MarioContext)
            {
                OnCollected();
            }
            if(other is BlockEntity)
            {
                if (Position==PrevPosition)
                {
                    Flipped = !Flipped;
                }
                if(other.Commands.IsNext<Move>())
                {
                    Flipped = (other.Position.X - Position.X) > 0;
                    Velocity = new Point(0, -2);
                }
            }
            base.OnCollision(other, direction);
        }
        public virtual void OnCollected()
        {
            Destroy();
        }
        public override bool SolidTo(IEntity other, Point dir) => other is BlockEntity;
        public void OnEventTriggered(GameEvent e)
        {

        }
    }
}
