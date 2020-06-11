using System;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mario.Entities.Hazzards;
namespace Mario.Entities
{
    public enum Hazards
    {
        MarioFireball,
        FirebarFireball,
        EnemyFireball,
        Hammer,
        Bowserfire,
    }
    public class HazzardFactory : Factory
    {
        enum States
        {
            Normal,
            Death
        }
        public HazzardFactory(Level level) : base(level) { }
        public override Entity Create(Point position, Enum id)
        {
            switch ((Hazards)id)
            {
                case Hazards.MarioFireball:
                case Hazards.FirebarFireball:
                case Hazards.EnemyFireball:
                    return new Fireball(Level, position, CreateSprite(id), CreateDeathSprite(id), (Hazards)id);
                case Hazards.Bowserfire:
                    return new Bowserfire(Level, position, CreateSprite(Hazards.Bowserfire));
                case Hazards.Hammer:
                    return new Hammer(Level, position, CreateSprite(id), CreateDeathSprite(id));
            }
            return null;
        }
        public override Sprite CreateSprite(Enum id, Enum state)
        {
            switch((States)state)
            {
                case States.Normal: return CreateSprite(id);
                case States.Death: return CreateDeathSprite(id);
            }
            return null;
        }
        Sprite CreateDeathSprite(Enum id)
        {
            switch ((Hazards)id)
            {
                case Hazards.MarioFireball:
                    Sprite s = new Sprite(Content.Load<Texture2D>("Sprites/fireBall"), new Point(16, 16), new[]{new Point(32, 0), new Point(48, 0), new Point(64, 0)});
                    s.Delay = 2;
                    return s;
            }
            return null;
        }

        Sprite CreateSprite(Enum ID)
        {
            switch ((Hazards)ID)
            {
                case Hazards.MarioFireball:
                case Hazards.FirebarFireball:
                    Point size = new Point(8, 8);
                    Point[] animation = { new Point(0, 0), new Point(8, 0), new Point(16, 0), new Point(24, 0) };
                    return new Sprite(Content.Load<Texture2D>("Sprites/fireBall"), size, animation);
                case Hazards.Hammer:
                    size = new Point(16);
                    animation = new[] { new Point(384, 0), new Point(400, 0), new Point(384, 16), new Point(400, 16) };
                    return new Sprite(Content.Load<Texture2D>("Sprites/Enimies2"), size, animation);
                case Hazards.Bowserfire:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(783, 0) };
                    return new Sprite(Content.Load<Texture2D>("Sprites/Enimies2"), size, animation);
            }
            return null;
        }
    }
}