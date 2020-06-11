using Mario.Collisions;
using Mario.Entities;
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace Mario.Levels
{
    public interface IScene
    {
        int Height { get; }
        int Width { get; }
        Color BackgroundColor { get; }
        int Time { get; }

        void UpdateStart();
        void Update();

        void DrawBackground(GraphicsDevice graphicsDevice);
        void Draw(SpriteBatch spriteBatch);
        void DrawText(SpriteBatch spriteBatch);
    }
}