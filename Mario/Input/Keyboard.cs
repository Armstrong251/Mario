using Mario.Input;
using Microsoft.Xna.Framework.Input;
using KB = Microsoft.Xna.Framework.Input.Keyboard;
class Keyboard : Controller
{
    public Keyboard(ControllerManager callback) : base(callback)
    {
        SetMapping(Keys.Q, GameEvent.QUIT);
        SetMapping(Keys.W, GameEvent.JUMP);
        SetMapping(Keys.Up, GameEvent.JUMP);
        SetMapping(Keys.A, GameEvent.MOVE_LEFT, KeyEvent.KEY_HELD);
        SetMapping(Keys.Left, GameEvent.MOVE_LEFT, KeyEvent.KEY_HELD);
        SetMapping(Keys.S, GameEvent.CROUCH);
        SetMapping(Keys.Down, GameEvent.CROUCH);
        SetMapping(Keys.S, GameEvent.CROUCH_RELEASE, KeyEvent.KEY_UP);
        SetMapping(Keys.Down, GameEvent.CROUCH_RELEASE, KeyEvent.KEY_UP);
        SetMapping(Keys.D, GameEvent.MOVE_RIGHT, KeyEvent.KEY_HELD);
        SetMapping(Keys.Right, GameEvent.MOVE_RIGHT, KeyEvent.KEY_HELD);
        SetMapping(Keys.Y, GameEvent.STANDARD_MARIO);
        SetMapping(Keys.U, GameEvent.SUPER_MARIO);
        SetMapping(Keys.I, GameEvent.FIRE_MARIO);
        SetMapping(Keys.O, GameEvent.DAMAGE);
        SetMapping(Keys.OemQuestion, GameEvent.QUESTION_BLOCK_HIT);
        SetMapping(Keys.X, GameEvent.USED_BLOCK_HIT);
        SetMapping(Keys.B, GameEvent.BRICK_BLOCK_HIT);
        SetMapping(Keys.H, GameEvent.HIDDEN_BLOCK_HIT);
        SetMapping(Keys.C, GameEvent.BOUNDING);
        SetMapping(Keys.Z, GameEvent.FIREBALL);
        SetMapping(Keys.R, GameEvent.RESET);
        SetMapping(Keys.M, GameEvent.MUTE);
        SetMapping(Keys.Space, GameEvent.START);
        SetMapping(Keys.L, GameEvent.COINSCHEAT);
        SetMapping(Keys.P,GameEvent.PAUSE);
        SetMapping(Keys.N, GameEvent.CONTINUE);
        SetMapping(Keys.E, GameEvent.INTERACT);
        SetMapping(Keys.T, GameEvent.WARP);
        SetMapping(Keys.K, GameEvent.STAR);
    }
    protected override bool GetButton(int key)
    {
        return KB.GetState().IsKeyDown((Keys)key);
    }
}