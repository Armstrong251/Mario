using Mario.Entities;
using Microsoft.Xna.Framework;

interface ISpriteFactory
{
    Sprite CreateSprite(Point position, int ID);
}
