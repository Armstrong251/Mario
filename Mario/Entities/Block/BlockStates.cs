using System;
using System.Diagnostics;
using System.Linq;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utils;
using static Mario.Entities.Block.States;

namespace Mario.Entities.Block
{

    //TODO: Add Constructors. Add reference to the Block class(es) when implemented.
    public enum States
    {
        Used,
        Brick,
        ItemBrick,
        Question,
        Hidden,
        Stair,
        Floor,
        BabyBrick,
        PipeTop,
        PipeSide,
        PipeVert,
        PipeHoriz,
        FlagCap,
        Flagpole,
        Flag,
        PipeJoint,
        Castle,
        Firebar,
        BulletLauncher,
        BanzaiLauncher,
        Platform,
        SmallBridge,
        BigBrick,
        LavaTop,
        LavaBottom,
        Axe,
        BossPlatformSmall,
        BossPlatformFull,
        Toad,
        Dirt,
        SelfState,
    }
    public abstract class BlockState : IBlockState
    {
        public States SelfEnum => SelfState;
        private BlockEntity block;
        public Sprite Sprite { get; set; }
        protected BlockEntity Block { get => block; set => block = value; }
        protected Level Level => Block.Level;

        protected BlockState(BlockEntity block, Sprite sprite)
        {
            this.Block = block;
            this.Sprite = sprite;

        }

        public States HandleCollision(IEntity other, Point direction)
        {
            if (other is MarioContext)
            {
                return HandleActivation((MarioContext)other, direction);
            }
            return SelfEnum;
        }
        public virtual States HandleActivation(MarioContext activator, Point direction)
        {
            if (direction.Y == 1 && Block.StoredItem.HasValue)
            {
                Block.RevealItem(activator);
                return Block.StoredItemCount == 0 ? Used : SelfState;
            }
            return SelfEnum;
        }
        public virtual States HandleItemAdd(ItemType item) { return SelfEnum; }
        public virtual States HandleEvent(GameEvent e) { return SelfEnum; }
        public virtual void EnterState(States previousState) { }
        public virtual void ExitState(States nextState) { }
        public virtual States Update() { return SelfEnum; }
        public virtual void HandleDraw(SpriteBatch spriteBatch) { Block.DrawBase(spriteBatch); }

        public virtual bool SolidTo(IEntity other, Point dir) { return true; }
    }

    //The state for a block that has been used.
    class UsedState : BlockState
    {
        public UsedState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override States HandleActivation(MarioContext activator, Point direction) { return SelfEnum; }
    }

    //The state for a standard brick block.
    class BrickState : BlockState
    {
        public BrickState(BlockEntity block, Sprite sprite) : base(block, sprite) { }

        public override States HandleItemAdd(ItemType item)
        {
            return ItemBrick;
        }
        public override States HandleActivation(MarioContext activator, Point direction)
        {
            switch (activator.PowerUpStates.CurrentState)
            {
                case PowerUpEnum.NORMAL:
                    if(direction.Y == 1) Block.Bump();
                    return base.HandleActivation(activator, direction);
                default:
                    //break
                    if (direction.Y == 1 || direction == Point.Zero)
                    {
                        Block.Shatter();
                        Level.Game.Points += 50;
                    }
                    return SelfEnum;
            }
        }
    }

    //The state for a brick block that has coins in it.
    class ItemBrickState : BlockState
    {
        //private int timer;

        public ItemBrickState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
    }


    //The state for a question block.
    class QuestionState : BlockState
    {
        //Stored item field.

        public QuestionState(BlockEntity block, Sprite sprite) : base(block, sprite) { }

        public override States HandleActivation(MarioContext activator, Point direction)
        {
            //Bump then give item.
            if (direction.Y == 1 && !Block.StoredItem.HasValue) Block.StoredItem = ItemType.BLOCK_COIN;
            return base.HandleActivation(activator, direction);
        }
    }

    //The state for a block that is invisible.
    class HiddenState : BlockState
    {
        //May need to override others.
        public HiddenState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override bool SolidTo(IEntity other, Point dir) => other is MarioContext && dir.Y == 1;
        public override States HandleItemAdd(ItemType item)
        {
            Block.StoredItemCount = 1;
            return SelfEnum;
        }
        public override States HandleActivation(MarioContext activator, Point direction)
        {
            if(direction.Y == 1 && Block.StoredItemCount == 0)
            {
                Block.Bump();
                return States.Used;
            }
            return SelfEnum;
        }
    }
    class StairState : BlockState
    {
        public StairState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
    }

    //The state for a floor block.
    class FloorState : BlockState
    {
        public FloorState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
    }
    class BabyBrick : BlockState
    {
        public BabyBrick(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override void EnterState(States previousState)
        {
            Block.HasGravity = true;
            Block.BoundingBox.Active = false;
            base.EnterState(previousState);
        }

        public override void HandleDraw(SpriteBatch spriteBatch)
        {
            Sprite.Rotation += .1f;
            base.HandleDraw(spriteBatch);
        }
    }
    class PipeState : BlockState
    {
        public PipeState(BlockEntity block, Sprite sprite) : base(block, sprite)
        {
        }

        public string StoredLevel { get; set; }
        public bool Return { get; set; }
        public Point Destination { get; set; }
    }
    class PipeTopState : PipeState
    {
        public PipeTopState(BlockEntity block, Sprite sprite) : base(block, sprite)
        {
        }

        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Blue, new Vector2(0, 0), new Vector2(32, 16));
        }
    }
    class PipeVertState : BlockState
    {
        public PipeVertState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Blue, new Vector2(0, 0), new Vector2(32, 16));
        }
        public override States HandleActivation(MarioContext activator, Point direction) { return SelfEnum; }
    }
    class PipeHorizState : BlockState
    {
        public PipeHorizState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Blue, new Vector2(0, 0), new Vector2(16, 32));
        }
        public override States HandleActivation(MarioContext activator, Point direction)
        {

            return SelfState;
        }
    }
    class PipeSideState : PipeState
    {
        public PipeSideState(BlockEntity block, Sprite sprite) : base(block, sprite)
        {
        }

        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Blue, new Vector2(0, 0), new Vector2(16, 32));
        }
    }
    class PipeJointState : BlockState
    {
        public PipeJointState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Blue, new Vector2(0, 0), new Vector2(32, 32));
        }
        public override States HandleActivation(MarioContext activator, Point direction) { return SelfEnum; }
    }
    class Castle : BlockState
    {
        public Castle(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Honeydew, new Vector2(48, 112), new Vector2(16, 16));
        }
        public override States HandleActivation(MarioContext activator, Point direction) { return SelfEnum; }
    }
    class PlatformState : BlockState
    {
        //May need to override others.
        public PlatformState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override bool SolidTo(IEntity other, Point dir) => other is MarioContext && dir.Y == -1;
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.Honeydew, new Vector2(0, 0), new Vector2(16, 7));
        }
    }
    class LavaTopState : BlockState
    {
        //May need to override others.
        public LavaTopState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override bool SolidTo(IEntity other, Point dir) { return false; }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.RosyBrown, new Vector2(0, 0), new Vector2(0, 0));
        }
    }
    class LavaBottomState : BlockState
    {
        //May need to override others.
        public LavaBottomState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override bool SolidTo(IEntity other, Point dir) { return false; }
        public override void EnterState(States prevState)
        {
            Block.BoundingBox = new Bounds(Block, Color.RosyBrown, new Vector2(0, 0), new Vector2(0, 0));
        }
    }
    class AxeState : BlockState
    {
        private BlockEntity[] array;
        public AxeState(BlockEntity block, Sprite sprite, BlockFactory bf) : base(block, sprite) {
            int xOFfset = -208;
            int yOffset = 32;
            array = new BlockEntity[]
            {
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+16+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+32+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+48+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+64+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+80+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+96+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+112+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+128+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+144+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+160+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+176+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+192+xOFfset),((int)block.Position.Y+yOffset)),States.BossPlatformSmall)),
            };
        }
        public override bool SolidTo(IEntity other, Point dir) { return false; }
        public override void HandleDraw(SpriteBatch spriteBatch)
        {
            foreach (BlockEntity bs in array)
            {
                bs.Draw(spriteBatch);
            }
            base.HandleDraw(spriteBatch);
        }

        //Only actually has a complexity of 2.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override States HandleActivation(MarioContext activator, Point direction)
        {
            
            if (Block.StoredItemCount < 2)
            {
                Block.Commands += new[] { new MethodCall<IEntity>(g => { Block.StoredItemCount = 2;Level.MusicPlayer.PlaySoundEffect("break"); Fall(); }),
                    new MethodCall<IEntity>(g => { Sprite.Rotation = Sprite.Rotation - .1f; }).Repeat(12),


                };
                activator.DisableEvents = true;
                Level.PauseTime = true;
                activator.Velocity = Vector2.Zero;
                //activator.Commands += new Message("toad").Until(m => m.CollidingWith.Any(e => ((BlockEntity)e.entity).BlockStates.CurrentState == States.Toad));
                activator.Commands += new Command[]
                {
                new MethodCall<MarioContext>(m => { m.Flipped = false; m.Sprite.Delay = -1; m.Position += new Point(16, 0); }),
                new Move(0, 0).Repeat(50),
                // new Move(2, 0),
                new MethodCall<MarioContext>(m => { m.Flipped = true; m.Velocity = new Vector2(1, 0); }),
                new MethodCall<MarioContext>(m => {m.ActionStates.CurrentState = ActionEnum.WALKING; m.BoundingBox.Active = true; m.HasGravity = true;}),
                new Wait().AddCondition(new Until(m => m.CollidingWith.Count > 0)),
                new MethodCall<MarioContext>(m => m.ActionStates.CurrentState = ActionEnum.WALKING)
                        .AddCondition(new Until(m => m.CollidingWith.Any(e => ((BlockEntity)e.entity).BlockStates.CurrentState == States.Toad))),
                new MethodCall<MarioContext>(m => m.Velocity = Vector2.Zero),
                new MethodCall<MarioContext>(m => { m.Level.Time--; m.Level.Game.Points += 50; }).AddCondition(new Until(m => m.Level.Time == 0)),
                new Move(0,0).Repeat(10),
                new MethodCall<MarioContext>(m => m.Level.GoToNextLevel())
                };
                Level.MusicPlayer.PlaySoundEffect("WorldClear");
                MusicPlayer.StopBGM();
            }
            return SelfEnum;
        }
        public void Fall()
        {
            Block.Commands += new[] { new MethodCall<IEntity>(f => { array[12].Destroy(); array[12].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break");}), new Wait().Repeat(15),
                 new MethodCall<IEntity>(f => { array[11].Destroy(); array[11].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                  new MethodCall<IEntity>(f => { array[10].Destroy(); array[10].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                   new MethodCall<IEntity>(f => { array[9].Destroy(); array[9].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break");  }), new Wait().Repeat(15),
                    new MethodCall<IEntity>(f => { array[8].Destroy(); array[8].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                    new MethodCall<IEntity>(f => { array[7].Destroy(); array[7].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                  new MethodCall<IEntity>(f => { array[6].Destroy(); array[6].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                   new MethodCall<IEntity>(f => { array[5].Destroy(); array[5].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break");  }), new Wait().Repeat(15),
                    new MethodCall<IEntity>(f => { array[4].Destroy(); array[4].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                    new MethodCall<IEntity>(f => { array[3].Destroy(); array[3].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                    new MethodCall<IEntity>(f => { array[2].Destroy(); array[2].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                  new MethodCall<IEntity>(f => { array[1].Destroy(); array[1].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break"); }), new Wait().Repeat(15),
                   new MethodCall<IEntity>(f => { array[0].Destroy(); array[0].Sprite.Visible = false; Level.MusicPlayer.PlaySoundEffect("break");  }), new Wait().Repeat(15),
            };
        }
    }
    class BossPlatformFull : BlockState
    {
        private BlockEntity[] array;
        public BossPlatformFull(BlockEntity block, Sprite sprite, BlockFactory bf) : base(block, sprite)
        {

            array = new BlockEntity[]
            {
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X),((int)block.Position.Y)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+16),((int)block.Position.Y)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+32),((int)block.Position.Y)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+48),((int)block.Position.Y)),States.BossPlatformSmall)),
                ((BlockEntity)bf.Create(new Point(((int)block.Position.X+64),((int)block.Position.Y)),States.BossPlatformSmall)),
            };
        }
        public override void HandleDraw(SpriteBatch spriteBatch)
        {
            foreach (BlockEntity bs in array)
            {
                bs.Draw(spriteBatch);
            }
            base.HandleDraw(spriteBatch);
        }
        public override States Update()
        {
            if (Block.StoredItemCount > 2)
            {
                Debug.Write("Hey");

            }
            return base.Update();
        }
        private void Fall()
        {
            Block.Commands += new[] { new MethodCall<IEntity>(f => { array[0].Solid = false; array[0].Sprite.Visible = false; }), new Wait().Repeat(5),
                 new MethodCall<IEntity>(f => { array[1].Solid = false; array[1].Sprite.Visible = false; }), new Wait().Repeat(5),
                  new MethodCall<IEntity>(f => { array[2].Solid = false; array[2].Sprite.Visible = false; }), new Wait().Repeat(5),
                   new MethodCall<IEntity>(f => { array[3].Solid = false; array[3].Sprite.Visible = false; }), new Wait().Repeat(5),
                    new MethodCall<IEntity>(f => { array[4].Solid = false; array[4].Sprite.Visible = false; }), new Wait().Repeat(5),


            };
        }
    }
    class BossPlatformSmall : BlockState
    {
        public BossPlatformSmall(BlockEntity block, Sprite sprite) : base(block, sprite) { }
    }
    class Toad : BlockState
    {
        public Toad(BlockEntity block, Sprite sprite) : base(block, sprite) { block.Position = new Vector2(block.Position.X, block.Position.Y - 12); }



        public override bool SolidTo(IEntity other, Point dir)
                => !(other is MarioContext m
                        && m.Commands.IsNext<Message>()
                        && m.Commands.GetNext<Message>().Text == "flagpole");
    }
    class DirtState : BlockState
    {
        public DirtState(BlockEntity block, Sprite sprite) : base(block, sprite) { }
    }
}
