using Mario.Entities.Block;
using Mario.Input;
using Microsoft.Xna.Framework;

using System;

namespace Mario.Entities.Mario
{
    public abstract class MarioState<E> : IState<E>
        where E : Enum
    {
        public abstract E SelfEnum { get; }
        public MarioContext Context { get; set; }
        protected MarioState(MarioContext context)
        {
            this.Context = context;
        }

        public virtual E HandleEvent(GameEvent e) { return SelfEnum; }
        public virtual void EnterState(E previousState) { }
        public virtual void ExitState(E nextState) { }
        public virtual E Update() { return SelfEnum; }

        public virtual E HandleCollision(IEntity other, Point direction) { return SelfEnum; }
    }
}
