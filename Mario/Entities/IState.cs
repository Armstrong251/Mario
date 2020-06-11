using Mario.Input;
using Microsoft.Xna.Framework;
using System;

namespace Mario.Entities
{
    public interface IState<E>
        where E : Enum
    {
        E SelfEnum { get; }
        E HandleEvent(GameEvent e);
        E HandleCollision(IEntity other, Point direction);
        void EnterState(E previousState);
        void ExitState(E nextState);
        E Update();
    }
}