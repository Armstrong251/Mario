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
    class FireFlower : ItemEntity
    {
        public FireFlower(Level l, Point position, Sprite sprite, ItemType type = ItemType.FIRE_FLOWER) : base(l, position, sprite, type)
        {
            HasGravity = true;
            BoundingBox = new Bounds(this, Color.Green, new Vector2(-2, -2), new Vector2(20, 18));

        }
        public override void OnCollected()
        {
            Level.Spawn(typeof(Points), Position, PointValue.p1000);
            base.OnCollected();
        }
    }
}