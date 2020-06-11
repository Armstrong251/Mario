using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mario.Entities;
using Mario.Levels;

namespace Mario
{
    public class HUD
    {

        static readonly Vector2 displacement = new Vector2(24, 7);

        String playerName;

        int score => Game.Points;

        int lives => Game.Lives;

        int coins => Game.Coins;

        String LevelName => Game.Level?.LevelName;

        int time => Scene.Time;

        public bool Visible { get; set; }
        public MarioGame Game { get; }

        public IScene Scene => Game.Scene;

        private SpriteFont font;
        private Sprite coinIcon;
        private Sprite livesIcon;
        
        public HUD(MarioGame game, SpriteFont font, Sprite coinIcon, Sprite livesIcon)
        {
            playerName = "MARIO";
            Visible = true;
            Game = game;
            this.font = font;
            this.coinIcon = coinIcon;
            this.livesIcon = livesIcon;
            
        }

        public void DrawIcons(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                coinIcon.Draw(spriteBatch, new Point(60 + (int)displacement.X, 9 + (int)displacement.Y));
                coinIcon.Update();
                livesIcon.Draw(spriteBatch, new Point(56 + (int)displacement.X, (int)displacement.Y));
                livesIcon.Update();
            }
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.DrawString(font, playerName + "   *" + lives.ToString("00") + "   WORLD TIME", (Vector2.Zero + displacement) * 11 / 4, Color.White);
                spriteBatch.DrawString(font, score.ToString("000000") +"  *" + coins.ToString("00") + "    " + (LevelName ?? "   ") + "   " + 
                    (time >= 0? String.Format("{0,3:###}", time) : ""), (new Vector2(0, 9) + displacement) * 11 / 4, Color.White);
            }
        }
    }
}
