using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities
{
    class NullSprite : Sprite
    {
        private static Sprite _instance;
        public static Sprite Instance
        {
            get
            {
                if (_instance == null) _instance = new NullSprite();
                return _instance;
            }
        }
        public override void Update()
        {
        }
        public override void Draw(SpriteBatch spriteBatch, Point position)
        {
        }
    }
}
