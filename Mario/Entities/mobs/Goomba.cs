using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Mario.Entities.mobs
{
    class Goomba:MonsterEntity
    {
        public Goomba(Level l, Point position, Sprite sprite, Sprite squashSprite) : base(l, position, sprite, squashSprite)
        {
            BoundingBox = new Bounds(this, Color.Red, new Vector2(2, 2), new Vector2(12, 14));
            HasGravity = true;
            Flipped = Level.Mario.Position.X > Position.X;
        }
    }
}
