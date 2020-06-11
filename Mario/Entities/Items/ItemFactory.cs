using System;
using System.Collections.Generic;
using System.Linq;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities
{
    class ItemFactory : Factory
    {
        public ItemFactory(Level level) : base(level) { }
        public override Entity Create(Point position, Enum type)
        {
                switch ((ItemType)type)
                {
                    case ItemType.MUSHROOM:
                        return new SuperMushroom(Level, position, CreateSprite(type), (ItemType)type);
                    case ItemType.FIRE_FLOWER:
                        return new FireFlower(Level, position, CreateSprite(type), (ItemType)type);
                    case ItemType.ONEUP:
                        return new LifeMushroom(Level, position, CreateSprite(type), (ItemType)type);
                    case ItemType.STAR:
                        return new Star(Level, position, CreateSprite(type), (ItemType)type);
                    case ItemType.BLOCK_COIN:
                        return new BlockCoin(Level, position, CreateSprite(type), (ItemType)type);
                    case ItemType.WORLD_COIN:
                        return new WorldCoin(Level, position, CreateSprite(type), (ItemType)type);
                }
            
            //Should never happen 
            return new ItemEntity(Level, position, CreateSprite(type), (ItemType)type);
        }
        public override Sprite CreateSprite(Enum id, Enum state) { return CreateSprite(id); }
        static Dictionary<string, Point> offsets = new Dictionary<string, Point>()
        {
            ["Plains"] = new Point(0, 0),
            ["Cave"] = new Point(0, 16),
            ["Castle"] = new Point(0, 32),
            ["DarkPlains"] = new Point(0, 0),
            ["Space"] = new Point(0, 0),
        };
        Sprite CreateSprite(Enum ID)
        {
            Point? size = null;
            Point[] animation = null;
            bool themeOffset = true;
            int delay = Sprite.DefaultDelay;
            switch (ID)
            {
                case ItemType.MUSHROOM:
                    return new Sprite(Content.Load<Texture2D>("Sprites/itemsSheet"), new Rectangle(0, 0, 16, 16));
                case ItemType.ONEUP:
                    return new Sprite(Content.Load<Texture2D>("Sprites/itemsSheet"), new Rectangle(0, 16, 16, 16));
                case ItemType.FIRE_FLOWER:
                    size = new Point(16, 16);
                    animation = new[] { new Point(0, 64), new Point(16, 64), new Point(32, 64), new Point(48, 64) };
                    break;
                case ItemType.STAR:
                    size = new Point(14, 16);
                    animation = new[] { new Point(64, 64), new Point(80, 64), new Point(96, 64), new Point(112, 64) };
                    delay = 2;
                    break;
                case ItemType.BLOCK_COIN:
                    size = new Point(8, 16);
                    animation = new[] { new Point(64, 0), new Point(72, 0), new Point(80, 0), new Point(88, 0) };
                    themeOffset = false;
                    break;
                case ItemType.WORLD_COIN:
                    size = new Point(16, 16);
                    animation = new[] { new Point(96, 0), new Point(112, 0), new Point(128, 0), new Point(112, 0) };
                    break;
                case ItemType.LEVEL_HOLDER:
                    size = new Point(0, 0);
                    animation = new[] { new Point(0, 0) };
                    break;
            }
            if (themeOffset) animation = animation.Select(p => p + offsets[Theme]).ToArray();
            Sprite s = new Sprite(Content.Load<Texture2D>("Sprites/itemsSheet"), size.Value, animation, delay);
            return s;
        }
    }
}