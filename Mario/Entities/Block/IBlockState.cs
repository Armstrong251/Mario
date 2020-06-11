using Mario.Entities.Mario;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities.Block
{
    //This interface defines what each block state does.
    public interface IBlockState : IState<States>
    {
        States HandleActivation(MarioContext activator, Point direction);
        //Handle any visual changes necessary.
        void HandleDraw(SpriteBatch spriteBatch);
    }
}
