namespace Mario.Input
{
    public interface IEventObserver
    {
        void OnEventTriggered(GameEvent e);
    }
}