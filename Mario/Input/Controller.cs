using Mario.Input;
using System.Collections.Generic;
using static Mario.Input.KeyEvent;

abstract class Controller : IController
{
    private ControllerManager callback;
    private Dictionary<int, GameEvent>[] mappings;
    private HashSet<int> mappedButtons;
    private Dictionary<int, bool> states;
    public Controller(ControllerManager callback)
    {
        this.callback = callback;
        mappings = new Dictionary<int, GameEvent>[3];
        for (int i = 0; i < 3; i++) mappings[i] = new Dictionary<int, GameEvent>();
        states = new Dictionary<int, bool>();
        mappedButtons = new HashSet<int>();
    }
    protected void SetMapping<T>(T button, GameEvent e, KeyEvent keyEvent = KEY_DOWN)
    {
        mappedButtons.Add((int)(object)button);
        mappings[(int)keyEvent][(int)(object)button] = e;
        states[(int)(object)button] = false;
    }
    public virtual void Update()
    {
        foreach (int button in mappedButtons)
        {
            KeyEvent keyEvent = NONE;
            //DIY flags - left bit is prev. state, right bit is current state
            switch ((states[button] ? 2 : 0) + (GetButton(button) ? 1 : 0))
            {
                case 0b00: break;
                case 0b01: keyEvent = KEY_DOWN; break;
                case 0b10: keyEvent = KEY_UP; break;
                case 0b11: keyEvent = KEY_HELD; break;
            }
            if (keyEvent != NONE && mappings[(int)keyEvent].ContainsKey(button))
                callback.ReportEvent(mappings[(int)keyEvent][button]);
            states[button] = GetButton(button);
        }
    }
    protected abstract bool GetButton(int button);
}