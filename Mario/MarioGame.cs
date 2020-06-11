using Mario.Entities;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Utils;

namespace Mario
{
    public class MarioGame : Game, IEventObserver
    {
        SpriteBatch spriteBatch;

        public SpriteFont Font { get; private set; }

        private GraphicsDeviceManager graphics;

        public const int hResolution = 256;
        public const int vResolution = 208;

        public ControllerManager Manager { get; set; }
        public MusicPlayer MusicPlayer { get; set; }
        public Level Level => Levels.Count == 0? null : Levels.Peek();
        public Stack<Level> Levels { get; private set; }
        public Stack<Transition> Transitions { get; private set; }
        public Transition Transition => Transitions.Count == 0? null : Transitions.Peek();
        public IScene Scene => (IScene)Transition ?? Level;

        public Camera Camera { get; set; }

        private RenderTarget2D mainScreen;

        private HUD hud;

        public int Coins { get; set; } = 0;
        public int Points { get; set; } = 0;
        public int Lives { get; set; } = 3;

        public MarioGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = hResolution * 11 / 4,
                PreferredBackBufferHeight = vResolution * 11 / 4
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            mainScreen = new RenderTarget2D(GraphicsDevice, hResolution, vResolution);
        }

        protected override void LoadContent()
        {
            Levels = new Stack<Level>();
            Transitions = new Stack<Transition>();

            MusicPlayer = new MusicPlayer(Content);
            MusicPlayer.LoadSoundEffect("SoundEffects/start", "start");
            MusicPlayer.LoadSoundEffect("SoundEffects/breakblock", "break");
            MusicPlayer.LoadSoundEffect("SoundEffects/1-Down", "Damage");
            MusicPlayer.LoadSoundEffect("SoundEffects/1-Up", "1-Up");
            MusicPlayer.LoadSoundEffect("SoundEffects/bump", "bump");
            MusicPlayer.LoadSoundEffect("SoundEffects/coin", "coin");
            MusicPlayer.LoadSoundEffect("SoundEffects/fireball", "fireball");
            MusicPlayer.LoadSoundEffect("SoundEffects/jump-small", "jump-small");
            MusicPlayer.LoadSoundEffect("SoundEffects/jump-super", "jump-super");
            MusicPlayer.LoadSoundEffect("SoundEffects/pause", "pause");
            MusicPlayer.LoadSoundEffect("SoundEffects/pipe", "pipe");
            MusicPlayer.LoadSoundEffect("SoundEffects/powerup", "powerup");
            MusicPlayer.LoadSoundEffect("SoundEffects/powerup_appears", "powerup_appears");
            MusicPlayer.LoadSoundEffect("SoundEffects/stomp", "stomp");
            MusicPlayer.LoadSoundEffect("SoundEffects/flagpole", "flagpole");
            MusicPlayer.LoadSoundEffect("SoundEffects/Hurry", "Hurry");
            MusicPlayer.LoadSoundEffect("SoundEffects/WorldClear", "WorldClear");

            Camera = new Camera(new Viewport(0, 0, hResolution, vResolution));
            Manager = new ControllerManager();
            Manager.AddObserver(this, new[] { GameEvent.QUIT, GameEvent.RESET, GameEvent.MUTE });
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("MarioFont");
            Bounds.Pixel = new Texture2D(GraphicsDevice, 1, 1);

            Theme.InitThemes();

            AddLevel("Level1", false);
            AddTransition(new TitleScreen(this));


            Bounds.Pixel.SetData(new[] { new Color(0xFF, 0xFF, 0xFF) });
            Texture2D coinIcon = Content.Load<Texture2D>("Sprites/coin_icon");
            Point[] coinIconAnimation =
            {

                new Point(0, 0),
                new Point(0, 0),
                new Point(5, 0),
                new Point(10, 0),
                new Point(5, 0),
                new Point(0, 0),
                new Point(0, 0)
            };

            Texture2D marioSheet = Content.Load<Texture2D>("Sprites/marioSheet3");

            hud = new HUD(this, Font, new Sprite(coinIcon, new Point(5, 8), coinIconAnimation), new Sprite(marioSheet, new Rectangle(131, 0, 11, 7)));

            //Load all the audio into the music player class
            //MusicPlayer.LoadSoundEffect("SoundEffects/GameOver", "Death");
        }

        public void AddLevel(string levelFolder) => AddLevel(levelFolder, true);
        public void AddLevel(string levelFolder, bool appearing) { AddLevel(levelFolder, Vector2.Zero, PowerUpEnum.NORMAL, appearing); }
        
        public void AddLevel(string levelFolder, Vector2 respawnPosition) => AddLevel(levelFolder, respawnPosition, true);
        public void AddLevel(string levelFolder, Vector2 respawnPosition, bool appearing) { AddLevel(levelFolder, respawnPosition, PowerUpEnum.NORMAL, appearing); }

        public void AddLevel(string levelFolder, PowerUpEnum powerUp) => AddLevel(levelFolder, powerUp, true);
        public void AddLevel(string levelFolder, PowerUpEnum powerUp, bool appearing) { AddLevel(levelFolder, Vector2.Zero, powerUp, appearing); }
        
        public void AddLevel(string level, Vector2 respawnPosition, PowerUpEnum powerUp, bool appearing) { AddLevel(LevelFactory.LoadLevel(this, level, respawnPosition, powerUp), appearing); }
        public void AddLevel(Level level, bool appearing)
        {
            if(Levels.Count != 0) Level.Mario.DisableEvents = true;
            Levels.Push(level);
            if(appearing && Transitions.Count == 0)
            {
                Level.OnAppear();
            }
        }
        public void AddTransition(Transition transition)
        {
            if(Levels.Count != 0) Level.Mario.DisableEvents = true;
            Transitions.Push(transition);
        }
        public void RemoveTransition()
        {
            Transitions.Pop();
            if(Transitions.Count == 0 && Levels.Count != 0) 
            {
                Level.OnAppear();
            }
        }
        public void RemoveLevel()
        {
            Levels.Pop().Destroy();
            if(Levels.Count != 0) 
            {
                Level.OnAppear();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            Camera.Limits = new Rectangle(0, Scene.Height - vResolution, Scene.Width, vResolution);
            if (Levels.Count == 0 && Transitions.Count == 0) Exit();
            Scene.UpdateStart();
            Manager.Update();
            Scene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Vector2 parallax = Vector2.One;

            Camera.Origin = Camera.Position;


            GraphicsDevice.SetRenderTarget(mainScreen);

            Scene.DrawBackground(GraphicsDevice);


            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp,
                            depthStencilState: DepthStencilState.DepthRead, transformMatrix: Camera.GetViewMatrix(parallax));
            Scene.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            hud.DrawIcons(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(mainScreen, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            //Draw text here. Remember to adjust your coordinates by 11 / 4 (multiply first) for the higher resolution.

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            hud.DrawText(spriteBatch);
            Scene.DrawText(spriteBatch);
            spriteBatch.End();

            /*
             * So, you may be wondering, "Why are text and sprites drawn separately?"
             * It's because Monogame doesn't like drawing pixel-art text super small, so it has
             * to be drawn after everything else and I don't know how to do it any other way.
             */

            base.Draw(gameTime);
        }

        public void OnEventTriggered(GameEvent e)
        {
            switch (e)
            {
                case GameEvent.RESET:
                    MusicPlayer.PauseBGM();
                    LoadContent();
                    break;
                case GameEvent.QUIT: Exit(); break;
                case GameEvent.MUTE: MusicPlayer.ToggleMute(); break;
            }
        }
    }
}