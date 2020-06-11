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
    abstract class Coin : ItemEntity
    {
        public Coin(Level l, Point position, Sprite sprite, ItemType type) : base(l, position, sprite, type) { }

        public void CollectCoin()
        {
            if(++Level.Game.Coins >= 100)
            {
                Level.Game.Coins = 0;
                Level.Game.Lives++;
                Level.MusicPlayer.PlaySoundEffect("1-Up");
            }
            Level.MusicPlayer.PlaySoundEffect("coin");
        }
    }
    class WorldCoin : Coin
    {
        public override bool SolidTo(IEntity other, Point dir) => false;
        public WorldCoin(Level l, Point position, Sprite sprite, ItemType type = ItemType.WORLD_COIN) : base(l, position, sprite, type)
        {
            BoundingBox = new Bounds(this, Color.Green, Vector2.Zero, new Vector2(16, 16));
            PrevPosition = Position;
        }
        public override void Update()
        {
            if((Vector2)Velocity != Vector2.Zero) Velocity = Point.Zero;
        }
        public override void OnCollected()
        {
            CollectCoin();
            Level.Game.Points += 200;
            base.OnCollected();
        }
    }
    class BlockCoin : Coin
    {
        public BlockCoin(Level l, Point position, Sprite sprite, ItemType type = ItemType.BLOCK_COIN) : base(l, position, sprite, type)
        {
            HasGravity = true;
            Position += new Point(4, 0);
            PrevPosition = Position;
            Velocity = new Vector2(0, -4.5f);
            Sprite.Delay = 2;
            CollectCoin();
            Commands += new Command[] { 
                new Wait().Repeat(32), 
                new MethodCall<ItemEntity>(e => {
                    e.OnCollected();
                    Level.Spawn(typeof(Points), e.Position, PointValue.p200);
                }) 
            };
        }
    }
}