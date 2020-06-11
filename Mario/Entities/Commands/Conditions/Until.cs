using System;

namespace Mario.Entities.Commands
{
    public class Until : CommandCondition
    {
        public Until(Predicate<Entity> condition)
        {
            Condition = condition;
        }

        public Predicate<Entity> Condition { get; }

        public override bool EvaluateCondition(Entity entity)
        {
            return Condition(entity);
        }
    }
}