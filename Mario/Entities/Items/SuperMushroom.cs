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
    class SuperMushroom:ItemEntity
    {
        public SuperMushroom(Level l, Point position, Sprite sprite, ItemType type = ItemType.MUSHROOM) : base(l, position, sprite, type)
        {
            Flipped = false;
            HasGravity = true;
            BoundingBox = new Bounds(this, Color.Green, new Vector2(-2, -2), new Vector2(20, 18));

        }
        public override void Update()
        {
            this.Position += new Point(Flipped ? -1 : 1, 0);
        }
        public override void OnCollected()
        {
            Level.Spawn(typeof(Points), Position, PointValue.p1000);
            base.OnCollected();
        }
    }
}