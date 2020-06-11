using System.Linq;

namespace Mario.Entities.Commands
{
    public interface ICommandCondition
    {
        void Update();
        bool EvaluateCondition(Entity entity);
    }
    public abstract class CommandCondition : ICommandCondition
    {
        public static ICommandCondition Or(params ICommandCondition[] conditions) => new ConditionOr(conditions);
        public static ICommandCondition And(params ICommandCondition[] conditions) => new ConditionAnd(conditions);
        public virtual void Update() { }

        public abstract bool EvaluateCondition(Entity entity);
        public static ICommandCondition operator |(CommandCondition one, ICommandCondition two) => Or(one, two);
        public static ICommandCondition operator &(CommandCondition one, ICommandCondition two) => And(one, two);
    }
    class ConditionOr : CommandCondition
    {
        private readonly ICommandCondition[] conditions;

        public ConditionOr(ICommandCondition[] conditions)
        {
            this.conditions = conditions;
        }
        public override void Update()
        {
            foreach(var c in conditions) c.Update();
        }
        public override bool EvaluateCondition(Entity entity)
        {
            return conditions.Any(c => c.EvaluateCondition(entity));
        }
    }
    class ConditionAnd : CommandCondition
    {
        private readonly ICommandCondition[] conditions;
        public override void Update()
        {
            foreach(var c in conditions) c.Update();
        }

        public ConditionAnd(ICommandCondition[] conditions)
        {
            this.conditions = conditions;
        }
        public override bool EvaluateCondition(Entity entity)
        {
            return conditions.All(c => c.EvaluateCondition(entity));
        }
    }
}