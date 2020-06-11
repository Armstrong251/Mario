
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Utils;

namespace Mario.Entities.mobs
{
    public abstract class Koopa : MonsterEntity
    {
        public static Koopa CreateKoopa(Level level, Point position, Mobs mob)
        {
            switch(mob)
            {
                case Mobs.GreenFlyingKoopa:
                case Mobs.greenKoopa: return new GreenKoopa(level, position);
                case Mobs.RedFlyingKoopa:
                case Mobs.redKoopa: return new RedKoopa(level, position);
            }
            return null;
        }
        public override Sprite SpriteOverride => States.State.Sprite;
        public override bool SolidTo(IEntity other, Point dir) => base.SolidTo(other, dir) || States.State.SolidTo(other, dir);
        public StateList<KoopaState, KoopaEnum> States { get; set; }
        protected Koopa(Level level, Point position) : base(level, position, NullSprite.Instance, NullSprite.Instance)
        {
            HasGravity = true;
        }
        public override void OnCollision(IEntity other, Point direction)
        {
            States.HandleCollision(other, direction);
        }
        public override void Kill(DeathType type, bool spawnPoints)
        {
            Commands += new SetSprite(States[KoopaEnum.IN_SHELL].Sprite).Until(e => false);
            base.Kill(type, spawnPoints);
        }
        public override void Update()
        {
            States.Update();
        }
    }
    class GreenKoopa: Koopa
    {
        public GreenKoopa(Level l, Point position) : base(l, position) {
        }
    }
    class RedKoopa: Koopa
    {
        int timer;
        public RedKoopa(Level l, Point position) : base(l, position) {
        }
        public override void Update()
        {
            if(!Dead)
            {
                timer++;
                switch (States.CurrentState)
                {
                    case KoopaEnum.WALKING:
                        Vector2 posR = BoundingBox.Rectangle.BR() + new Vector2(1, 1);
                        Vector2 posL = BoundingBox.Rectangle.BL() + new Vector2(-1, 1);
                        Predicate<Vector2> check = c => (Level.Collider.GridAt(c.ToPoint())?.All(e => !(e is BlockEntity)) ?? false);
                        bool checkR = check(posR);
                        bool checkL = check(posL);
                        if ((Flipped && checkR && !checkL) != (!Flipped && checkL && !checkR))
                        {
                            Flipped = !Flipped;
                        }
                        break;
                    case KoopaEnum.FLYING:
                        Velocity = new Vector2(Velocity.X, (float)(-Math.Cos(timer * MathHelper.TwoPi / 240) - 0.125f));
                        break;
                }
            }
            base.Update();
        }
    }
}