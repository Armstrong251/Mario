using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mario.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Mario.Levels
{
    public class Theme
    {

        private static Theme defaultTheme = new Theme("default", Color.CornflowerBlue);

        private static Dictionary<String, Theme> themes = new Dictionary<string, Theme>();
        private static Dictionary<string, string> music = new Dictionary<string, string>()
        {
            ["Plains"] = "MainSong",
            ["Cave"] = "02-underworld",
            ["Castle"] = "04-castle",
            ["Space"] = "galaxystep",
        };
        public string BGM => "Music/" + (music.ContainsKey(Name) ? music[Name] : music["Plains"]);

        private static Dictionary<string, string> hurryMusic = new Dictionary<string, string>()
        {
            ["Plains"] = "18-hurry-overworld-",
        };
        public string HurryBGM => "Music/" + (hurryMusic.ContainsKey(Name) ? hurryMusic[Name] : hurryMusic["Plains"]);

        public static void SetDefault(Theme theme)
        {
            defaultTheme = theme;
        }

        public static void Register(Theme theme)
        {
            themes[theme.Name] = theme;
        }

        public static Theme GetTheme(string themeName)
        {
            Theme theme = defaultTheme;

            if (themeName != null && themes.ContainsKey(themeName))
            {
                theme = themes[themeName];
            }

            return theme;
        }
        public static void InitThemes()
        {
            themes["Plains"] = new Theme("Plains", Color.CornflowerBlue);
            themes["Cave"] = new Theme("Cave", Color.Black);
            themes["Castle"] = new Theme("Castle", Color.Black);
            themes["DarkPlains"] = new Theme("DarkPlains", Color.Black);
            themes["Space"] = new Theme("Space", Color.Black);
            Theme.SetDefault(themes["Plains"]);
        }
        public void InitLayers(Level Level)
        {
            ContentManager Content = Level.Content;
            GraphicsDevice GraphicsDevice = Level.Game.GraphicsDevice;
            Camera Camera = Level.Game.Camera;
            Texture2D backgroundSheet = Content.Load<Texture2D>("Sprites/backgrounds2");
            
            if(Name == "Plains")
            {

                BackgroundLayer bushes = new BackgroundLayer(GraphicsDevice, Camera, new Point(11 * 16, Level.Height - 48), new Point(48 * 16, 32), new Point(Level.Width, 32), Vector2.One);
                Sprite singleBush = new Sprite(backgroundSheet, new Rectangle(0, 24, 32, 16));
                Sprite doubleBush = new Sprite(backgroundSheet, new Rectangle(32, 24, 48, 16));
                Sprite tripleBush = new Sprite(backgroundSheet, new Rectangle(80, 24, 64, 16));

                Sprite singleBushFlipped = new Sprite(backgroundSheet, new Rectangle(0, 24, 32, 16));
                Sprite doubleBushFlipped = new Sprite(backgroundSheet, new Rectangle(32, 24, 48, 16));
                Sprite tripleBushFlipped = new Sprite(backgroundSheet, new Rectangle(80, 24, 64, 16));

                singleBushFlipped.SpriteEffects = SpriteEffects.FlipVertically;
                doubleBushFlipped.SpriteEffects = SpriteEffects.FlipVertically;
                tripleBushFlipped.SpriteEffects = SpriteEffects.FlipVertically;

                bushes.AddSprite(tripleBush, 0, 0);
                bushes.AddSprite(singleBush, 12 * 16, 0);
                bushes.AddSprite(doubleBush, 30 * 16, 0);

                bushes.AddSprite(tripleBushFlipped, 0, 16);
                bushes.AddSprite(singleBushFlipped, 12 * 16, 16);
                bushes.AddSprite(doubleBushFlipped, 30 * 16, 16);
                BackgroundLayer hills = new BackgroundLayer(GraphicsDevice, Camera, new Point(0, Level.Height - 67), new Point(48 * 16, 67), new Point(Level.Width, 67), new Vector2(0.7f, 1f));
                Sprite bigHill = new Sprite(backgroundSheet, new Rectangle(192, 5, 80, 35));

                Sprite bigHillFlipped = new Sprite(backgroundSheet, new Rectangle(192, 5, 80, 35));
                bigHillFlipped.SpriteEffects = SpriteEffects.FlipVertically;

                hills.AddSprite(bigHillFlipped, 0, 35);
                hills.AddSprite(bigHill, -32, 32);
                hills.AddSprite(bigHill, 32, 32);
                hills.AddSprite(bigHill, 0, 0);
                hills.AddSprite(bigHill, -24, 35);
                hills.AddSprite(bigHill, 24, 35);

                //Sprite smallHill = new Sprite(backgroundSheet, new Rectangle(144, 21, 48, 19));

                hills.AddSprite(bigHillFlipped, 16 * 16, 35 + 16);
                hills.AddSprite(bigHill, -32 + 16 * 16, 32 + 16);
                hills.AddSprite(bigHill, 32 + 16 * 16, 32 + 16);
                hills.AddSprite(bigHill, 16 * 16, 16);
                hills.AddSprite(bigHill, -24 + 16 * 16, 35 + 16);
                hills.AddSprite(bigHill, 24 + 16 * 16, 35 + 16);
                BackgroundLayer clouds = new BackgroundLayer(GraphicsDevice, Camera, new Point(8 * 16, Level.Height - 12 * 16), new Point(48 * 16, 40), new Point(Level.Width, 40), new Vector2(0.5f, 1f));
                Sprite singleCloud = new Sprite(backgroundSheet, new Rectangle(0, 0, 32, 24));
                Sprite doubleCloud = new Sprite(backgroundSheet, new Rectangle(32, 0, 48, 24));
                Sprite tripleCloud = new Sprite(backgroundSheet, new Rectangle(80, 0, 64, 24));

                clouds.AddSprite(singleCloud, 0, 16);
                clouds.AddSprite(singleCloud, 11 * 16, 0);
                clouds.AddSprite(tripleCloud, 15 * 16, 16);
                clouds.AddSprite(doubleCloud, 26 * 16, 0);
                backgroundLayers = new[] {clouds, hills, bushes};
            }
            else if(Name == "DarkPlains")
            {
                BackgroundLayer clouds = new BackgroundLayer(GraphicsDevice, Camera, new Point(8 * 16, Level.Height - 12 * 16), new Point(48 * 16, 40), new Point(Level.Width, 40), new Vector2(0.5f, 1f));
                Sprite singleCloud = new Sprite(backgroundSheet, new Rectangle(0, 0, 32, 24));
                Sprite doubleCloud = new Sprite(backgroundSheet, new Rectangle(32, 0, 48, 24));
                Sprite tripleCloud = new Sprite(backgroundSheet, new Rectangle(80, 0, 64, 24));

                clouds.AddSprite(singleCloud, 0, 16);
                clouds.AddSprite(singleCloud, 11 * 16, 0);
                clouds.AddSprite(tripleCloud, 15 * 16, 16);
                clouds.AddSprite(doubleCloud, 26 * 16, 0);
                backgroundLayers = new[] { clouds };
            }
            else if(Name == "Space")
            {
                backgroundSheet = Content.Load<Texture2D>("Sprites/SpaceBackground");
                BackgroundLayer space = new BackgroundLayer(GraphicsDevice, Camera, new Point(8 * 16, Level.Height - 12 * 16), new Point(Level.Width, 64), new Point(Level.Width, 64), new Vector2(0.25f, 1f));
                BackgroundLayer lowerSpace = new BackgroundLayer(GraphicsDevice, Camera, new Point(8 * 16, Level.Height - 8 * 16), new Point(20*16, 64), new Point(Level.Width, 64), new Vector2(0.25f, 1f));
                Sprite mushroomPlanet = new Sprite(backgroundSheet, new Rectangle(0, 0, 24, 24));
                Sprite bigDipper = new Sprite(backgroundSheet, new Rectangle(32, 0, 31, 32));
                Sprite ringPlanet = new Sprite(backgroundSheet, new Rectangle(63, 0, 18, 18));
                Sprite orion = new Sprite(backgroundSheet, new Rectangle(82, 0, 30, 32));
                Sprite stars = new Sprite(backgroundSheet, new Rectangle(0, 32, 64, 32));
                Sprite bigStar = new Sprite(backgroundSheet, new Rectangle(64, 32, 64, 32));
                Sprite starCastles = new Sprite(backgroundSheet, new Rectangle(0, 78, 50, 64));
                Sprite bowserShip = new Sprite(backgroundSheet, new Rectangle(64, 65, 64, 63));
                space.AddSprite(stars, 0, 0);
                space.AddSprite(mushroomPlanet, 5 * 16, 0);
                space.AddSprite(bigDipper, 7 * 16, 0);
                space.AddSprite(stars, 9 * 16, 0);
                space.AddSprite(ringPlanet, 14 * 16, 0);
                space.AddSprite(orion, 16 * 16, 0);
                space.AddSprite(stars, 19 * 16, 0);
                space.AddSprite(bigStar, 24 * 16, 0);
                space.AddSprite(stars, 27 * 16, 0);
                space.AddSprite(starCastles, 32 * 16, 0);
                space.AddSprite(stars, 37 * 16, 0);
                space.AddSprite(bowserShip, 42 * 16, 0);
                lowerSpace.AddSprite(orion, 0, 0);
                lowerSpace.AddSprite(stars, 3 * 16, 0);
                lowerSpace.AddSprite(bigDipper, 8 * 16, 0);
                lowerSpace.AddSprite(orion, 11 * 16, 0);
                lowerSpace.AddSprite(stars, 14 * 16, 0);

                backgroundLayers = new[] { space, lowerSpace };
            }
            else
            {
                backgroundLayers = new BackgroundLayer[] {};
            }

            foreach (BackgroundLayer layer in backgroundLayers)
            {
                layer.Render();
            }
        }

        public string Name { get; private set; }
        public Color BackgroundColor { get; private set; }

        private BackgroundLayer[] backgroundLayers;

        public Theme(string name, Color backgroundColor)
        {
            Name = name;
            BackgroundColor = backgroundColor;
        }


        public void DrawBackground(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(BackgroundColor);

            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

            foreach (BackgroundLayer layer in backgroundLayers)
            {
                layer.Draw(spriteBatch);
            }
        }

    }
}
