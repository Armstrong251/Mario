using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Mario.Entities.mobs
{
    class FatBill : MonsterEntity
    {
        public FatBill(Level l, Point position, Sprite sprite) : base(l, position, sprite, NullSprite.Instance)
        {
            BoundingBox = new Bounds(this, Color.Red, new Vector2(2, 2), new Vector2(62, 62));
            this.BoundingBox.Color = Color.Red;
            this.HasGravity = false;
            this.Speed = 1;
            Stompable = true;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            if (other is BlockEntity block)
            {
                if (block.BlockStates.CurrentState == States.BulletLauncher|| block.BlockStates.CurrentState == States.Floor)
                {
                    Kill(DeathType.Fall);
                }
                else
                {
                    block.Shatter();
                }
            }
            if (other is MarioContext mario)
            {
                if (mario.StarState.Active)
                {
                    Kill(DeathType.Fall);
                }
                else if (direction.Y <= -1)
                {
                    Kill(DeathType.Fall, false);
                }
            }
        }
    }
}
