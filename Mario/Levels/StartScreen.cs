using Mario.Collisions;
using Mario.Entities;
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KB = Microsoft.Xna.Framework.Input.Keyboard;

namespace Mario.Levels
{
    class TitleScreen : Transition, IEventObserver
    {
        public ControllerManager Manager => Game.Manager;
        private Texture2D Title;
        private Sprite floor;
        private Sprite mario;

        private Theme theme;

        private bool warping = false;
        private int digit = -1;
        private KeyboardState prevState;


        public TitleScreen(MarioGame game) : base(game)
        {
            Manager.AddObserver(this, new[] { GameEvent.START, GameEvent.WARP });
            theme = Theme.GetTheme("default");
            theme.InitLayers((Level)Game.Level);
            floor = (Game.Level as Level).Factory.CreateSprite(typeof(BlockEntity), States.Floor, null);
            Title = (Game.Content.Load<Texture2D>("TitleScreen"));// new Rectangle(1, 60, 175, 87));
            mario = (Game.Level as Level).Factory.CreateSprite(typeof(MarioContext), PowerUpEnum.NORMAL, ActionEnum.STANDING);
            mario.SpriteEffects = SpriteEffects.FlipHorizontally;
            BackgroundColor = Color.CornflowerBlue;
        }
        public override void Update()
        {
            if(warping)
            {
                var state = KB.GetState();
                for(int i = (int)Keys.D1; i < (int)Keys.D9 + 1; i++)
                {
                    if(state.IsKeyDown((Keys)i) && !prevState.IsKeyDown((Keys)i))
                    {
                        int d = (i - (int)Keys.D0);
                        if(digit == -1) 
                        {
                            digit = d;
                        }
                        else
                        {
                            Game.RemoveLevel();
                            Game.AddLevel("Level"+((digit - 1) * 4 + d));
                            Game.RemoveTransition();
                        }
                    }
                }
                prevState = state;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.Lives = 3;
            Game.Coins = 0;
            Game.Points = 0;
            spriteBatch.Draw(Title, new Vector2(30, 40), new Rectangle(1, 60, 175, 87), Color.White);
            for (int i = 0; i < 20; i++)
            {
                floor.Draw(spriteBatch, new Point(i * 16, 180));
                floor.Draw(spriteBatch, new Point(i * 16, 196));
            }
            mario.Draw(spriteBatch, new Point(10, 164));
            spriteBatch.DrawString(Game.Font, "Press Space to begin", new Vector2(30, 130), Color.White, 0, Vector2.Zero, .40f, SpriteEffects.None, 0);

        }
        public void OnEventTriggered(GameEvent e)
        {
            switch(e)
            {
                case GameEvent.START:
                    if (Game.Transition == this)
                    {
                        Game.RemoveTransition();
                        Game.MusicPlayer.RestartBGM();
                    }
                    break;
                case GameEvent.WARP:
                    warping = true;
                    break;

            }
        }

        public override void DrawBackground(GraphicsDevice graphicsDevice)
        {
            theme.DrawBackground(graphicsDevice);
        }
        public override void DrawText(SpriteBatch spriteBatch)
        {
        }
    }
}
