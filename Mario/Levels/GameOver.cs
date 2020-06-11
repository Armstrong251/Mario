using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Input;
using Utils;

namespace Mario.Levels
{
    public class GameOver : Transition , IEventObserver
    {
        public ControllerManager Manager => Game.Manager;

        public GameOver(MarioGame game, float time) : base(game, time) {
            Manager.AddObserver(this, GameEvent.QUIT);
            Manager.AddObserver(this, GameEvent.CONTINUE);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            
            spriteBatch.DrawString(Game.Font, "GAME OVER", new Vector2(Width * 11 / 8f, Height * 11 / 8f) - Game.Font.MeasureString("GAME OVER") / 2f, Color.White);
            spriteBatch.DrawString(Game.Font, "Press Q to quit or", new Vector2(Width * 10 / 8f, Height * 14 / 8f) - Game.Font.MeasureString("Press Q to quit or n to continue") / 2.5f, Color.White);
            spriteBatch.DrawString(Game.Font, "N to continue", new Vector2(Width * 10 / 8f, Height * 15 / 8f) - Game.Font.MeasureString("N to continue") / 2.5f, Color.White);

        }
        public void OnEventTriggered(GameEvent e)
        {
            if (e == GameEvent.CONTINUE && Game.Transition == this)
            {
                Game.OnEventTriggered(GameEvent.RESET);
            }
        }
    }
    public class Victory : Transition
    {
        public Victory(MarioGame game, float time) : base(game, time) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            MusicPlayer.StopBGM();
            spriteBatch.DrawString(Game.Font, "VICTORY", new Vector2(Width * 11 / 8f, Height * 11 / 8f) - Game.Font.MeasureString("VICTORY") / 2f, Color.White);
        }
    }

    public class StatsScreen : Transition
    {
        private Texture2D mario;

        public StatsScreen(MarioGame game) : base(game, 1.5F)
        {
            mario = (Game.Content.Load<Texture2D>("Sprites/marioSheet3"));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mario, new Vector2(MarioGame.hResolution / 2 - 16, MarioGame.vResolution / 2 - 8), new Rectangle(129, 0, 16, 16), Color.White);
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.Font, "*" + Game.Lives.ToString("00"), (new Vector2(MarioGame.hResolution / 2, MarioGame.vResolution / 2)) * 11 / 4, Color.White);
        }
    }
}