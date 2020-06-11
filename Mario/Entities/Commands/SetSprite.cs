namespace Mario.Entities.Commands
{
    public class SetSprite : Command
    {
        public Sprite Sprite { get; }
        public SetSprite(Sprite sprite)
        {
            this.Sprite = sprite;
        }

    }
}