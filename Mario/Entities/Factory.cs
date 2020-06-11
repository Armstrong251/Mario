using System;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mario.Entities
{
    public abstract class Factory
    {
        public ContentManager Content => Level.Content;
        public string Theme => Level.Theme.Name;

        public Level Level { get; set; }

        protected Factory(Level level)
        {
            Level = level;
        }
        public abstract Entity Create(Point position, Enum id);
        public abstract Sprite CreateSprite(Enum id, Enum state);
    }
}