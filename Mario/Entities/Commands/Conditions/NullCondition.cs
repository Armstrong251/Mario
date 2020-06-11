namespace Mario.Entities.Commands
{
    public class NullCondition : CommandCondition
    {
        private static NullCondition _instance;
        public static NullCondition Instance
        {
            get
            {
                if(_instance == null) _instance = new NullCondition();
                return _instance;
            }
        }
        public override bool EvaluateCondition(Entity entity) => true;
    }
}