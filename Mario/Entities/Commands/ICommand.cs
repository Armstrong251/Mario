using System;

namespace Mario.Entities.Commands
{
    public abstract class Command
    {
        public ICommandCondition Condition { get; set; } = NullCondition.Instance;

        public virtual bool Invoke(Entity entity)
        {
            return Condition.EvaluateCondition(entity);
        }
        public Command AddCondition(ICommandCondition condition)
        {
            Condition = condition;
            return this;
        }
        public Command Repeat(int repeat)
        {
            this.AddCondition(new Repeat(repeat));
            return this;
        }
        public Command Until(Predicate<Entity> condition)
        {
            this.AddCondition(new Until(condition));
            return this;
        }
        public static Command operator &(Command one, Command two) => new CombineCommand(one, two);
    }
}
