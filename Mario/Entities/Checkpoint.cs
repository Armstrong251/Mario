using Mario.Entities.Mario;
using Mario.Levels;
using Microsoft.Xna.Framework;

namespace Mario.Entities
{
    public class Checkpoint : Entity
    {
        public Checkpoint(Level l, Point position) : base(l, position, NullSprite.Instance) 
        {
            BoundingBox = new Bounds(this, Color.Gold, new Vector2(0, -position.Y), new Vector2(1, Level.Height));
        }

        public override void Update() {}
        public override void OnCollision(IEntity other, Point direction)
        {
            if(other is MarioContext && Position.X > Level.RespawnPosition.X)
            {
                Level.RespawnPosition = Position;
            }
        }
    }
}