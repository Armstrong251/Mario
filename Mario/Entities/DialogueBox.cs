using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.Entities
{
    class DialogueBox : Entity, IEventObserver
    {
        string text;
        bool visible;
        Texture2D textBox;
        Vector2 stringSize;
        Icons icon;
        Level l;
        public enum Icons
        {
            None,
            Mario,
        }

        public DialogueBox(string text, Icons icon, Level l, Point position) : base(l, position, NullSprite.Instance)
        {
            BoundingBox = new Bounds(this, Color.HotPink, new Vector2(0, -position.Y), new Vector2(1, Level.Height));
            l.Manager.AddObserver(this, GameEvent.INTERACT);
            visible = false;
            stringSize = Level.Game.Font.MeasureString(text);
            textBox = l.Content.Load<Texture2D>("Sprites/Blank");
            this.icon = icon;
            this.l = l;
        this.text = text;
        }

        public override void Update()
        {

        }
        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is MarioContext)
            {
                Display();
                BoundingBox.Active = false;
            }
        }
        public void Display()
        {
            Level.Manager.ReportEvent(GameEvent.DISPLAY_DIALOGUE);
            visible = true;
        }
        public void OnEventTriggered(GameEvent e)
        {
            if (e == GameEvent.INTERACT)
            {
                Remove();
            }
        }
        public void Remove()
        {
            Destroy();
        }
        public void DrawText(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                int trueX = 300;
                int trueY = 100;
                if (icon != Icons.None)
                    DrawIcon(icon, new Vector2(trueX - stringSize.X / 2 * .4f - 34, trueY-18), spriteBatch);
                    
                spriteBatch.Draw(textBox, new Rectangle((int)(trueX - stringSize.X/2 * .4f)-2, trueY-2, (int)(stringSize.X * .4)+4, (int)(stringSize.Y * .4)+4), Color.White);
                spriteBatch.Draw(textBox, new Rectangle((int)(trueX - stringSize.X/2 * .4f)-1, trueY-1, (int)(stringSize.X * .4)+2, (int)(stringSize.Y * .4)+2), Color.Black);
                spriteBatch.DrawString(Level.Game.Font, text, new Vector2(trueX - stringSize.X/2* .4f, trueY), Color.White, 0.0f, Vector2.Zero, .4f, SpriteEffects.None, 0);

            }
        }
        public void DrawIcon(Icons i, Vector2 position, SpriteBatch spriteBatch)
        {
            Texture2D face;
            switch (i)
            {
                case Icons.Mario:
                    face = l.Content.Load<Texture2D>("Sprites/MarioFace");
                    break;
                default:
                    face = textBox;
                    break;
            }
            spriteBatch.Draw(face, new Rectangle((int)(position.X),(int)(position.Y), 32, 32), Color.White);
        }
    }
}
