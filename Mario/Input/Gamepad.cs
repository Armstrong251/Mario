using Mario.Input;
using Microsoft.Xna.Framework.Input;
class Gamepad : Controller
{
    private int gamepadId;
    public Gamepad(ControllerManager controllerManager, int gamepadId) : base(controllerManager)
    {
        this.gamepadId = gamepadId;
        SetMapping(Buttons.Start, GameEvent.QUIT);
        SetMapping(Buttons.DPadUp, GameEvent.JUMP);
        SetMapping(Buttons.DPadLeft, GameEvent.MOVE_LEFT, KeyEvent.KEY_HELD);
        SetMapping(Buttons.DPadDown, GameEvent.CROUCH);
        SetMapping(Buttons.DPadDown, GameEvent.CROUCH_RELEASE, KeyEvent.KEY_UP);
        SetMapping(Buttons.DPadRight, GameEvent.MOVE_RIGHT, KeyEvent.KEY_HELD);
        SetMapping(Buttons.X, GameEvent.FIREBALL);
    }
    protected override bool GetButton(int button)
    {
        return GamePad.GetState(gamepadId).IsButtonDown((Buttons)button);
    }
}