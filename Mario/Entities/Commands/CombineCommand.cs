namespace Mario.Entities.Commands
{
    public class CombineCommand : Command
    {
        private Command[] commands;
        public CombineCommand(params Command[] a)
        {
            commands = a;
        }
        public override bool Invoke(Entity entity)
        {
            foreach(var c in commands) c.Invoke(entity);
            return base.Invoke(entity);
        }
    }
}