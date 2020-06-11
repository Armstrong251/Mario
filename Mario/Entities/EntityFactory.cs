
using System;
using System.Collections.Generic;
using Mario.Entities.Block;
using Mario.Entities.Mario;
using Mario.Levels;
using Microsoft.Xna.Framework;

namespace Mario.Entities
{
    public class EntityFactory
    {
        private Level level;
        private Dictionary<Type, Factory> factories;
        public EntityFactory(Level level)
        {
            this.level = level;
            factories = new Dictionary<Type, Factory>
            {
                [typeof(HazzardEntity)] = new HazzardFactory(level),
                [typeof(BlockEntity)] = new BlockFactory(level),
                [typeof(ItemEntity)] = new ItemFactory(level),
                [typeof(MarioContext)] = new MarioFactory(level),
                [typeof(MonsterEntity)] = new MonsterFactory(level),
                [typeof(Points)] = new PointsFactory(level),
                [typeof(Checkpoint)] = new CheckpointFactory(level),
            };
        }
        public Entity CreateEntity(Type type, Point position, Enum id)
        {
            Entity e;
            if (type == typeof(InvisibleWall))
            {
                e = new InvisibleWall(level);
            }
            else if (type == typeof(KillBox))
            {
                e = new KillBox(level);
            }
            else
            {
                e = factories[type].Create(position, id);
            }
            return e;
        }
        public Sprite CreateSprite(Type type, Enum id, Enum state)
        {
            return factories[type].CreateSprite(id, state);
        }
    }
}