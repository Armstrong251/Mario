using System.Linq;
using System.Windows.Input;
using Mario.Entities;
using Mario.Entities.Block;
using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Microsoft.Xna.Framework;

namespace Mario.Levels
{
    public class Level2Beginning : Level
    {
        public Level2Beginning(MarioGame game, string levelFolder, Vector2 respawnPosition, PowerUpEnum powerUp) : base(game, levelFolder, respawnPosition, powerUp)
        {
            Mario.Velocity = new Vector2(1, 0);
            Mario.ActionStates.CurrentState = ActionEnum.WALKING;
            Sprite s = Mario.Sprite.Clone();
            Mario.Commands += new Command[]
            {
                new Wait()
                    .Until(e => e.CollidingWith.Any(b => b.entity is BlockEntity block && block.BlockStates.CurrentState == States.PipeSide)),
                new MethodCall<IEntity>((e) => s.Layer = 0.4f),
                new Move(0.6f, 0).Repeat(16),
                new MethodCall<IEntity>((e) => GoToNextLevel())
            };
            Mario.Commands += new Command[]
            {
                new SetSprite(s).Repeat(1000)
            };
        }
        public override void UpdateStart()
        {
            base.UpdateStart();
            Mario.DisableEvents = true;
        }
    }
}