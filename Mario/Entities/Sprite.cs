using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities
{
    public class Sprite
    {
        public const int DefaultDelay = 7;
        private Texture2D spritesheet;
        private Point size;
        private Point[] animation;
        public int Length => animation.Length;
        private int timer;
        private int currentFrame;
        public float Rotation { get; set; } = 0;
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        public bool Visible { get; set; }
        public int Delay { get; set; }
        public float Layer { get; set; } = 0.5f;
        public int CurrentFrame { get => currentFrame; set => currentFrame = value; }

        public Sprite()
        {
            //ONLY USE WITH NULLSPRITE
        }
        public Sprite(Texture2D spritesheet) : this(spritesheet, spritesheet.Bounds) { }
        public Sprite(Texture2D spritesheet, Rectangle sourceArea)
                 : this(spritesheet, sourceArea.Size, new Point[] { sourceArea.Location }) { }
        public Sprite(Texture2D spritesheet, Point size, Point[] animation) : this(spritesheet, size, animation, DefaultDelay) { }
        public Sprite(Texture2D spritesheet, Point size, Point[] animation, int delay)
        {
            this.spritesheet = spritesheet;
            this.size = size;
            Delay = delay;
            if (animation != null) this.animation = animation;
            else this.animation = new Point[] { new Point(0, 0) };
            Visible = true;
        }
        public virtual void Update()
        {
            if (Visible)
            {
                timer++;
                if (timer == Delay)
                {
                    CurrentFrame = (CurrentFrame + 1) % animation.Length;
                    timer = 0;
                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch, Point position) 
        {
            Draw(spriteBatch, position, SpriteEffects.None);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Point position, SpriteEffects extra)
        {
            if (Visible)
            {
                Vector2 offset = size.ToVector2() / 2;
                spriteBatch.Draw(spritesheet, position.ToVector2() + offset,
                                new Rectangle(animation[CurrentFrame], size), 
                                Color.White, Rotation, offset, 1, SpriteEffects | extra, Layer);
            }
        }
        public Sprite Clone()
        {
            return new Sprite(this.spritesheet, this.size, this.animation);
        }
    }
}