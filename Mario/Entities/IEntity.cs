using System.Collections.Generic;
using Mario.Entities.Commands;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities
{
    public interface IEntity
    {
        Bounds BoundingBox { get; }
        VectorPoint Position { get; set; }
        VectorPoint PrevPosition { get; set; }
        VectorPoint Velocity { get; set; }
        bool SolidTo(IEntity other, Point dir);
        bool ForceMove { get; set; }
        List<(IEntity entity, Point dir)> CollidingWith { get; }
        Level Level { get; set; }
        CommandComponent Commands { get; set; }

        void OnCollision(IEntity other, Point direction);
        void UpdateBase();
        void UpdateStart();
        void Draw(SpriteBatch batch);
        void Destroy();
    }
}