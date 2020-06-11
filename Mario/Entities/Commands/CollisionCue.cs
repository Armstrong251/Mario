using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Mario.Entities.Commands
{
    class CollisionCue : Command
    {
        public const int LENGTH = 6;
        private Color old = Color.Black;
        public override bool Invoke(Entity entity)
        {
            if(old == Color.Black && entity.BoundingBox.Color != Color.Black)
            {
                old = entity.BoundingBox.Color;
                entity.BoundingBox.Color = Color.Black;
            }
            if (Condition.EvaluateCondition(entity) && old != Color.Black) entity.BoundingBox.Color = old;
            return base.Invoke(entity);
        }

    }
}
