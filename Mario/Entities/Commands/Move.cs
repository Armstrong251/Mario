using Mario.Entities.Block;
using Microsoft.Xna.Framework;

namespace Mario.Entities.Commands
{
    class Move : Command
    {

        public Vector2 Direction { get; }
        public Move(float x, float y)
        {
            Direction = new Vector2(x, y);
        }

        public Move(Vector2 dir)
        {
            Direction = dir;
        }
        public Move(Point dir)
        {
            Direction = dir.ToVector2();
        }

        public override bool Invoke(Entity entity)
        {
            //TODO: fix
            entity.Position += Direction;
            entity.ForceMove = true;
            return base.Invoke(entity);
        }
    }
    
}
