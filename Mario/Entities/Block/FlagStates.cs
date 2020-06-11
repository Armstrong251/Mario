
using System;
using System.Linq;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utils;

namespace Mario.Entities.Block
{
    public class Flag : BlockState
    {
        public override bool SolidTo(IEntity other, Point dir) => false;
        public Flag(BlockEntity block, Sprite sprite) : base(block, sprite)
        {
            Block.Position += new Point(8, 1);
            Block.PrevPosition = Block.Position;
        }
    }
    public class FlagBlock : BlockState
    {
        public FlagBlock(BlockEntity block, Sprite sprite) : base(block, sprite) { }
        public override void EnterState(States previousState)
        {
            Block.BoundingBox.Offset = new Vector2(7, 0);
            Block.BoundingBox.Size = new System.Drawing.SizeF(2, 16);
        }

        //Does not have a complexity over 25.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override States HandleActivation(MarioContext activator, Point direction)
        {
            if(!SolidTo(activator, direction)) return SelfEnum;
            int points = (272 - (int)activator.BoundingBox.Rectangle.Bottom) / 16;
            Level.Spawn(typeof(Points), activator.Position, (Enum)Enum.GetValues(typeof(PointValue)).GetValue(points));
            activator.ActionStates.CurrentState = ActionEnum.FLAG;
            activator.DisableEvents = true;
            activator.Flipped = true;
            activator.HasGravity = false;
            activator.BoundingBox.Active = false;
            Level.PauseTime = true;
            activator.Position = new Vector2(Block.Position.X - 8, (float)Math.Round(activator.Position.Y));
            activator.Velocity = Vector2.Zero;
            activator.Commands += new Message("flagpole").Until(m => m.CollidingWith.Any(e => ((BlockEntity)e.entity).BlockStates.CurrentState == States.Castle));
            activator.Commands += new Command[]
            {
                new Move(0, 1).AddCondition(new Until(e => e.BoundingBox.Rectangle.Bottom >= 270)),
                new MethodCall<MarioContext>(m => { m.Flipped = false; m.Sprite.Delay = -1; m.Position += new Point(16, 0); }),
                new Move(0, 0).Repeat(50),
                // new Move(2, 0),
                new MethodCall<MarioContext>(m => { m.Flipped = true; m.Velocity = new Vector2(1, 0); }),
                new MethodCall<MarioContext>(m => {m.ActionStates.CurrentState = ActionEnum.WALKING; m.BoundingBox.Active = true; m.HasGravity = true;}),
                new Wait().AddCondition(new Until(m => m.CollidingWith.Count > 0)),
                new MethodCall<MarioContext>(m => m.ActionStates.CurrentState = ActionEnum.WALKING)
                        .AddCondition(new Until(m => m.CollidingWith.Any(e => ((BlockEntity)e.entity).BlockStates.CurrentState == States.Castle))),
                new MethodCall<MarioContext>(m => m.Sprite.Visible = false),
                new MethodCall<MarioContext>(m => { m.Level.Time--; m.Level.Game.Points += 50; }).AddCondition(new Until(m => m.Level.Time == 0)),
                new Move(0,0).Repeat(10),
                new MethodCall<MarioContext>(m => m.Level.GoToNextLevel())
            };
            IEntity f = Level.Collider.GridAt(activator.Position).FirstOrDefault(e => (e as BlockEntity).BlockStates.CurrentState == States.Flag);
            if(f == null) f = Level.Collider.GridAt(activator.Position + new Vector2(0, 16)).FirstOrDefault(e => (e as BlockEntity).BlockStates.CurrentState == States.Flag);
            if(f != null)
            {
                f.Commands += new Move(0, 1).Until(e => e.BoundingBox.Rectangle.Bottom >= 272);
            }
            Level.MusicPlayer.PlaySoundEffect("WorldClear");
            MusicPlayer.StopBGM();
            return SelfEnum;
        }
        public override bool SolidTo(IEntity other, Point dir) 
                => !(other is MarioContext m 
                        && m.Commands.IsNext<Message>() 
                        && m.Commands.GetNext<Message>().Text == "flagpole");
    }
}