using System;

namespace Mario.Entities.Commands
{
    public class MethodCall<T> : Command
        where T: class, IEntity
    {
        private Action<T> method;
        public MethodCall(Action<T> method)
        {
            this.method = method;
        }
        public override bool Invoke(Entity entity)
        {
            if(entity is T t) method(t);
            return base.Invoke(entity);
        }
    }
}