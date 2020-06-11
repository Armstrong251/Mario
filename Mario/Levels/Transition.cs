using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Entities.Mario;

namespace Mario.Levels
{
    public abstract class Transition : IScene
    {
        private int time;

        public MarioGame Game { get; }
        public int Height { get; }
        public int Width { get; }
        public Color BackgroundColor {get; set;}

        protected Transition(MarioGame marioGame) : this(marioGame, -1) { }

        protected Transition(MarioGame marioGame, float time)
        {
            Game = marioGame;
            Height = MarioGame.vResolution;
            Width = MarioGame.hResolution;
            this.time = (int)(time * 60);
            BackgroundColor = Color.Black;
        }


        public int Time => -1;

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void DrawText(SpriteBatch spriteBatch);

        public virtual void Update()
        {
            if(time > 0) time--;
            if (time == 0)
            {
                Game.RemoveTransition();
            }
        }

        public void UpdateStart() { }

        public virtual void DrawBackground(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(BackgroundColor);
        }
    }
}