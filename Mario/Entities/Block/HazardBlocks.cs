using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Mario.Entities.Block
{
    class Firebar : BlockState
    {
        private float rotation = 0;
        private Entity[] fireballs;

        public int Size
        {
            get
            {
                return fireballs == null? 0 : fireballs.Length;
            }
            set
            {
                fireballs = new Entity[value];
                for(int i = 0; i < value; i++)
                {
                    fireballs[i] = Level.Spawn(typeof(HazzardEntity), Block.Position, Hazards.FirebarFireball);
                    fireballs[i].Sprite.Layer = 0.6f;
                }
            }
        }

        //Suppressed because the setter needs to be public, so a private getter wouldn't make sense.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public bool Clockwise { get; set; }
        public Firebar(BlockEntity block, Sprite sprite) : base(block, sprite)
        {
            Size = 5;
        }
        public override States Update()
        {
            for(int i = 0; i < Size; i++)
            {
                fireballs[i].Position = 10 * new Vector2(i * (float)Math.Cos(rotation), i * (float)Math.Sin(rotation)) + (Vector2)Block.Position + new Vector2(4);
            }
            rotation += MathHelper.TwoPi / 100;
            return SelfEnum;
        }

    }
    class BulletLauncher : BlockState
    {
        protected int minDelay = 50, maxDelay = 80;
        int Delay = 70;
        Random rnd = new Random();
        public Mobs BulletType { get; set; } = Mobs.BulletBill;
        protected int offset = 18;
        public BulletLauncher(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Chocolate, new Vector2(0, 0), new Vector2(16, 32));
        }
        public override States Update()
        {
            if (Delay-- == 0)
            {
                if (Level.Mario.Position.X - Block.Position.X <= 220 && Level.Mario.Position.X - Block.Position.X >= -220 && Level.Mario.Position.X - Block.Position.X != 0)
                {
                    IEntity e;
                    if (Level.Mario.Position.X < Block.Position.X)
                    {
                        e = Level.Spawn(typeof(MonsterEntity), new Point((int)(Block.Position.X - offset), (int)Block.Position.Y), BulletType);
                        (e as MonsterEntity).Flipped = false;
                    }
                    else
                    {
                        e = Level.Spawn(typeof(MonsterEntity), new Point((int)(Block.Position.X + offset), (int)Block.Position.Y), BulletType);
                        (e as MonsterEntity).Flipped = true;
                    }
                }

                Delay = rnd.Next(minDelay, maxDelay);
            }
            return SelfEnum;
        }
    }
    class BanzaiLauncher : BulletLauncher
    {
        public bool flipped;
        public BanzaiLauncher(BlockEntity block, Sprite sprite) : base(block, sprite)
        {
            BulletType = Mobs.FatBill;
            base.offset = 33;//66;
            minDelay = 200;
            maxDelay = 240;
        }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Chocolate, new Vector2(0, 0), new Vector2(32, 64));
        }
        public override States Update()
        {
            if(flipped)
            {
                if (Level.Mario.Position.X > Block.Position.X) base.Update();
            }
            else
            {
                base.offset = base.offset * -1;
                if (Level.Mario.Position.X < Block.Position.X) base.Update();
            }
            return SelfEnum;
        }
    }
}