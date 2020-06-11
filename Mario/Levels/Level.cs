using Mario.Collisions;
using Mario.Entities;
using Mario.Entities.Mario;
using Mario.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Utils;

namespace Mario.Levels
{
    public class Level : IScene, IEventObserver
    {
        public IEnumerable<IEntity> Entities => entities;
        private List<IEntity> entities;
        private bool destroying = false;
        public MarioContext Mario { get; set; }
        private Theme theme;
        //protected bool warping = false;
        public Theme Theme
        {
            get
            {
                return theme ?? Theme.GetTheme("default");
            }
            set
            {
                theme = value;
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return this.Theme.BackgroundColor;
            }
        }
        private EntityFactory factory;
        public ControllerManager Manager => Game.Manager;
        public ContentManager Content => Game.Content;
        public MusicPlayer MusicPlayer => Game.MusicPlayer;
        public MarioGame Game { get; }
        private Map map;
        private string levelFolder;
        public int Width => Map.Width * 16;
        public int Height => Map.Height * 16;
        internal Collider Collider { get; set; }
        public bool BoundsVisible { get; private set; }
        public float Gravity { get; set; } = 0.25f;
        public Map Map => map;
        private bool pause = false;
        public bool PauseTime { get; set; } = false;
        private int time;
        public int Time { get => time / 24; set => time = value * 24; }
        public Vector2 RespawnPosition { get; set; }
        public EntityFactory Factory { get => factory; set => factory = value; }
        public string LevelName { get; set; }
        public string NextLevel { get; set; }
        private int loadCount = 0;

        public Level(MarioGame game, string levelFolder, Vector2 respawnPosition, PowerUpEnum powerUp)
        {

            Time = 400;
            Game = game;
            this.levelFolder = levelFolder;
            LevelProperties lp = new LevelProperties(Content.RootDirectory + "/Levels/" + levelFolder + "/Properties.txt");
            lp.LoadProperties(this, "Level");
            Factory = new EntityFactory(this);
            RespawnPosition = respawnPosition;
            map = new Map(this, levelFolder);//, Game);
            Collider = new Collider(this);
            entities = new List<IEntity>();
            map.InitMap(lp);
            Theme.InitLayers(this);
            // DialogueBox db = new DialogueBox("Press E to Close This Dialogue Box ........", DialogueBox.Icons.Mario, this, new Point(5 * 16, 1));
            // entities.Add(db);
            Mario = Spawn(typeof(MarioContext), RespawnPosition.ToPoint(), powerUp) as MarioContext;
            Manager.AddObserver(this, new[] {
                GameEvent.COINSCHEAT, 
                GameEvent.BOUNDING, 
                GameEvent.PAUSE, 
                GameEvent.DISPLAY_DIALOGUE, 
                GameEvent.INTERACT 
            });
            Mario.PowerUpStates.CurrentState = powerUp;
            Mario.DisableEvents = true;
        }
        public void OnAppear()
        {
            loadCount++;
            if(loadCount == 1)
            {
                Mario.DisableEvents = false;
                MusicPlayer.PlaySoundEffect("start");
            }
            MusicPlayer.LoadBGM(Time < 100? Theme.HurryBGM : Theme.BGM);
            MusicPlayer.RestartBGM();
            Mario.DisableEvents = false;
        }
        public Entity Spawn(Type type, Point position, Enum id)
        {
            Entity e = Factory.CreateEntity(type, position, id);
            if (e != null) entities.Add(e);
            return e;
        }
        public void Destroy()
        {
            if(destroying) return;
            destroying = true;
            foreach (var e in entities.ToArray()) e.Destroy();
            Manager.RemoveObserver(this);
            if(Game.Level == this) Game.RemoveLevel();
            if (Game.Levels.Count > 0)
                Game.Level.Mario.PowerUpStates.CurrentState = Mario.PowerUpStates.CurrentState;
        }
        public void Restart()
        {
            Destroy();
            Game.AddTransition(new StatsScreen(Game));
            Game.AddLevel(levelFolder, RespawnPosition);
            //Game.Level.Mario.PowerUpStates.CurrentState = PowerUpEnum.NORMAL;
        }
        public void GoToNextLevel()
        {
            Destroy();
            if (NextLevel == null) Game.AddTransition(new Victory(Game, -1));
            else Game.AddLevel(NextLevel, Mario.PowerUpStates.CurrentState);
        }
        public Level MoveToLevelThroughPipe(string newLevelFolder, bool returning)
        {
            MusicPlayer.PlaySoundEffect("pipe");
            if (String.IsNullOrEmpty(newLevelFolder) || !returning)
            {
                Destroy();
            }
            if (!String.IsNullOrEmpty(newLevelFolder))
            {
                Game.AddTransition(new StatsScreen(Game));
                Game.AddLevel(newLevelFolder, Mario.PowerUpStates.CurrentState);
            }
            return (Level)Game.Level;
        }
        public void Destroy(IEntity e)
        {
            entities.Remove(e);
            Collider.Remove(e);
            if (e is MarioContext && !destroying)
            {
                Game.Lives--;
                if (Game.Lives == 0)
                {
                    Destroy();
                    Game.AddTransition(new GameOver(Game, -1));
                }
                else
                {
                    Restart();
                }
            }
        }
        public virtual void UpdateStart()
        {
            if (!pause)
            {
                if (!PauseTime && --time == 0)
                {
                    Mario.ActionStates.CurrentState = ActionEnum.DEATH;
                    Time = 400;
                }
                else if (!PauseTime && time == 100 * 24)
                {
                    MusicPlayer.LoadBGM(Theme.HurryBGM);
                    MusicPlayer.RestartBGM();
                }
                foreach (var e in entities) e.UpdateStart();
            }
        }
        public virtual void Update()
        {
            //ToArray() avoids problems with adding entities during Update()
            if (!pause)
            {
                Game.Camera.LookAt(Mario.Position + new Vector2(17, 0));
                Map.SpawnDynamic((int)(Game.Camera.Position.X) / 16 - 1, (int)(Game.Camera.Position.X + Game.Camera.Size.X) / 16 + 2);
                foreach (var e in entities.ToArray()) e.UpdateBase();
                Collider.Update();
                Game.Camera.LookAt(Mario.Position + new Vector2(17, 0));
            }
        }
        public void Draw(SpriteBatch batch)
        {
            foreach (var e in entities) e.Draw(batch);
        }
        public void OnEventTriggered(GameEvent e)
        {
            if (e == GameEvent.BOUNDING) BoundsVisible = !BoundsVisible;
            if (e == GameEvent.COINSCHEAT) Game.Coins = 99;
            if (e == GameEvent.PAUSE && Game.Level == this)
            {
                if (pause)
                {
                    Mario.DisableEvents = false;
                    MusicPlayer.UnpauseBGM();
                }
                else
                {
                    Mario.DisableEvents = true;
                    MusicPlayer.PauseBGM();
                }
                pause = !pause;

            }

            if (e == GameEvent.DISPLAY_DIALOGUE && Game.Level == this)
            {
                pause = true;
                Mario.DisableEvents = true;
                MusicPlayer.PauseBGM();
            }
            if (e == GameEvent.INTERACT)
            {
                pause = false;
                Mario.DisableEvents = false;
                MusicPlayer.UnpauseBGM();
            }
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            foreach (var e in entities)
            {
                if (e is DialogueBox)
                {
                    ((DialogueBox)e).DrawText(spriteBatch);
                }
            }
        }

        public void DrawBackground(GraphicsDevice graphicsDevice)
        {
            Theme.DrawBackground(graphicsDevice);
        }
    }
}
