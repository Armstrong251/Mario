namespace Mario.Entities.Commands
{
    public class Message : Command
    {
        public Message(string message) {
            Text = message;
        }
        public string Text { get; }
    }
}