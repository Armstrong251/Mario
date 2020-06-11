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
    class Star : ItemEntity
    {
        public Star(Level l, Point position, Sprite sprite, ItemType type = ItemType.STAR) : base(l, position, sprite, type)
        {
            Flipped = Level.Mario.Position.X > position.X;
            HasGravity = true;
            BoundingBox = new Bounds(this, Color.Green, new Vector2(-2, -2), new Vector2(20, 18));
        }
        public override void Update()
        {
            this.Position += new Point(Flipped ? -1 : 1, 0);
        }
        public override void OnCollision(IEntity other, Point direction)
        {
            if(SolidTo(other, direction))
            {
                if (direction.Y == 1) Velocity = new Point(0, -6);
                else if (direction.X != 0) Flipped = (direction.X == 1);
            }
            base.OnCollision(other, direction);
        }
        public override void OnCollected()
        {
            Level.Spawn(typeof(Points), Position, PointValue.p1000);
            base.OnCollected();
        }
    }
}