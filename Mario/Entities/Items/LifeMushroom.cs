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
    class LifeMushroom : ItemEntity
    {
        public LifeMushroom(Level l, Point position, Sprite sprite, ItemType type = ItemType.ONEUP) : base(l, position, sprite, type)
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
            Level.MusicPlayer.PlaySoundEffect("1-Up");
            Level.Spawn(typeof(Points), Position, PointValue.p1up);
            base.OnCollected();
        }
    }
}