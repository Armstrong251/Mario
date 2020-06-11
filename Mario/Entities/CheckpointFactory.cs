using System;
using Mario.Levels;
using Microsoft.Xna.Framework;

namespace Mario.Entities
{
    public class CheckpointFactory : Factory
    {
        public CheckpointFactory(Level level) : base(level) { }

        public override Entity Create(Point position, Enum id)
        {
            return new Checkpoint(Level, position);
        }

        public override Sprite CreateSprite(Enum id, Enum state)
        {
            return null;
        }
    }
}