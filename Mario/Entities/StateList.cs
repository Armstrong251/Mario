using Mario.Input;
using Microsoft.Xna.Framework;
using System;

namespace Mario.Entities
{
    public class StateList<T, E>
        where T : IState<E>
        where E : Enum
    {
        private T[] states;
        private int currentState;
        private readonly Func<E, T> createFunc;

        public T State => this[CurrentState];
        public E CurrentState
        {
            get
            {
                return (E)(object)currentState;
            }
            set
            {
                if (State == null || !value.Equals(State.SelfEnum))
                {
                    State?.ExitState(value);
                    this[value].EnterState(CurrentState);
                    currentState = (int)(object)value;
                }
            }
        }
        public StateList(E start, Func<E, T> createFunc)
        {
            states = new T[Enum.GetValues(typeof(E)).Length - 1];
            this.createFunc = createFunc;
            CurrentState = start;
        }
        public StateList(T[] states, E start)
        {
            this.states = states;
            CurrentState = start;
        }
        public void Handle(Func<T, E> stateFunc)
        {
            CurrentState = stateFunc(State);
        }

        public void HandleEvent(GameEvent e)
        {
            CurrentState = State.HandleEvent(e);
        }

        public void HandleCollision(IEntity other, Point direction)
        {
            CurrentState = State.HandleCollision(other, direction);
        }
        public void Update()
        {
            CurrentState = State.Update();
        }
        public T this[E index]
        {
            get => states[(int)(object)index] == null ? (states[(int)(object)index] = createFunc(index)) : states[(int)(object)index];
            set => states[(int)(object)index] = value;
        }
    }
}