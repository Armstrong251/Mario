using Mario.Entities.Commands;
using Mario.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Mario.Entities.Mario
{
    public abstract class ActionState : MarioState<ActionEnum>
    {
        public int WalkSpeed { get; set; } = 2;
        public override ActionEnum SelfEnum => ActionEnum.SelfState;

        protected ActionState(MarioContext context) : base(context) { }
        public override ActionEnum HandleEvent(GameEvent e)
        {
            switch (e)
            {
                case GameEvent.MOVE_LEFT:
                    Context.Flipped = false;
                    Context.Position -= new Point(WalkSpeed, 0);
                    
                    break;
                case GameEvent.MOVE_RIGHT:
                    Context.Flipped = true;
                    Context.Position += new Point(WalkSpeed, 0);
                    break;
            }
            return SelfEnum;
        }
    }
}
