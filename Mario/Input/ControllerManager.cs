using System;
using System.Collections.Generic;

namespace Mario.Input
{
    public class ControllerManager
    {
        private List<IController> controllers;
        private List<IEventObserver>[] observers;
        public ControllerManager()
        {
            observers = new List<IEventObserver>[Enum.GetValues(typeof(GameEvent)).Length];
            foreach (GameEvent e in Enum.GetValues(typeof(GameEvent)))
            {
                observers[(int)e] = new List<IEventObserver>();
            }
            controllers = new List<IController>();
            controllers.Add(new Keyboard(this));
            controllers.Add(new Gamepad(this, 0));
        }
        public void Update()
        {
            controllers.ForEach(c => c.Update());
        }
        //public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        //{
        //}
        public void ReportEvent(GameEvent e)
        {
            foreach (var obs in observers[(int)e].ToArray()) obs.OnEventTriggered(e);
        }
        public void AddObserver(IEventObserver observer, GameEvent[] e)
        {
            foreach (var ev in e) AddObserver(observer, ev);
        }
        public void AddObserver(IEventObserver observer, GameEvent e)
        {
            observers[(int)e].Add(observer);
        }
        public void RemoveObserver(IEventObserver observer, GameEvent e)
        {
            observers[(int)e].Remove(observer);
        }
        public void RemoveObserver(IEventObserver observer)
        {
            foreach (var ev in observers) ev.Remove(observer);
        }
    }
}