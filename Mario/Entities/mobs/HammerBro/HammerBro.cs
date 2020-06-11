using System;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities.mobs
{
    public class HammerBro : MonsterEntity
    {
        static Random rand = new Random();
        public static Random Rand { get => rand; }
        public StateList<BroState, BroEnum> BroStates { get; set; }
        public override Sprite SpriteOverride => BroStates.State.Sprite;
        public override bool Flipped { get; set; }
        public override Bounds BoundingBox { get => base.BoundingBox; set => base.BoundingBox = value; }
        public override bool Damaging { get => base.Damaging; set => base.Damaging = value; }
        public override bool CommonCollide { get => base.CommonCollide; set => base.CommonCollide = value; }
        public override bool Stompable { get => base.Stompable; set => base.Stompable = value; }

        public override PointValue PointValue => base.PointValue;

        public override bool SolidTo(IEntity other, Point dir) => BroStates.State.SolidTo(other, dir);

        public override void Update()
        {
            BroStates.Update();
            base.Update();
        }

        public override void Kill(DeathType type, bool spawnPoints)
        {
            base.Kill(DeathType.Fall, spawnPoints);
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            BroStates.HandleCollision(other, direction);
            base.OnCollision(other, direction);
        }

        public HammerBro(Level l, Point position) : base(l, position, NullSprite.Instance, NullSprite.Instance)
        {
            Speed = 0;
            Position -= new Point(0, 8);
            PrevPosition = Position;
            BoundingBox = new Bounds(this, Color.Red, new Vector2(2, 2), new Vector2(12, 22));
            HasGravity = true;
        }
        
    }
}