using Mario.Entities.Block;
using Mario.Entities.Mario;
using Mario.Input;
using Microsoft.Xna.Framework;
using Mario.Entities.Commands;
using System;
using Mario.Levels;

namespace Mario.Entities
{

    public abstract class HazzardEntity : Entity, IEventObserver
    {
        private readonly Sprite deathSprite;
        public override bool SolidTo(IEntity other, Point dir) => other is BlockEntity;
        public virtual bool Damages(IEntity other) => other is MarioContext;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected HazzardEntity(Level l, Point position, Sprite sprite, Sprite deathSprite) : base(l, position, sprite)
        {
            /*
            Velocity = new Vector2((l.Mario.Flipped? 1 : -1) * 3, 0);
            Velocity += Level.Mario.Position - Level.Mario.PrevPosition;
            BoundingBox = new Bounds(this, Color.Pink, new Vector2(-1, -1), new Vector2(10, 10));
            Flipped = l.Mario.Flipped;
            HasGravity = true;
            timer = 70;
            */
            this.deathSprite = deathSprite;
        }

        public override void Update()
        {
            //timer -= 1;
            //if (timer == 0) this.Kill();
        }
        public void Kill()
        {
            BoundingBox.Active = false;
            Position += new Point(-4, -4);
            Commands += new SetSprite(deathSprite).Repeat(100);
            Commands += new Command[]{new Move(0, 0).Repeat(deathSprite.Delay * deathSprite.Length - 1), new MethodCall<IEntity>(e => e.Destroy())};
        }
        public void OnEventTriggered(GameEvent e)
        {
        }
    }
}
