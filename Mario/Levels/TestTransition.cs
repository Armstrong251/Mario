using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Levels
{
    public class TestTransition : Transition
    {
        public TestTransition(MarioGame marioGame, int time) : base(marioGame, time)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
        public override void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.Font, "this is a test", new Vector2(100, 100), Color.White);
        }
    }
}