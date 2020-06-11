using System;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities
{
    public enum PointValue
    {
        p0 = 0,
        p100 = 1,
        p200 = 2,
        p400 = 4,
        p500 = 5,
        p800 = 8,
        p1000 = 10,
        p2000 = 20,
        p4000 = 40,
        p5000 = 50,
        p8000 = 80,
        p1up,
    }
    public class PointsFactory : Factory
    {
        public PointsFactory(Level level) : base(level) { }

        public override Entity Create(Point position, Enum id)
        {
            return new Points(Level, position, CreateSprite(id, null), (PointValue)id);
        }

        public override Sprite CreateSprite(Enum id, Enum state)
        {
            return ((PointValue)id == PointValue.p0) ? new NullSprite() : new Sprite(Content.Load<Texture2D>("Sprites/Points/"+Enum.GetName(typeof(PointValue), id).Substring(1)));
        }
    }
}