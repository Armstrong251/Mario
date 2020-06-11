namespace Mario.Entities.Mario
{
    public class StarState
    {
        private int timer;
        private bool active;
        public static readonly int Delay = 4;

        public StarState(MarioContext context, Sprite[,] normalSprites, Sprite[,] largeSprites)
        {
            Context = context;
            this.normalSprites = normalSprites;
            this.largeSprites = largeSprites;
        }

        public void Update()
        {
            if(timer > 0)
            {
                Sprite[,] current = (Context.PowerUpStates.CurrentState == PowerUpEnum.NORMAL? 
                            normalSprites :
                            largeSprites);
                for(int i = 0; i < current.GetLength(1); i++)
                {
                    if(i != timer / Delay % 3)
                        current[(int)Context.ActionStates.CurrentState, i].Update();
                }
                timer--;
            } 
            else if(Active) 
            {
                Active = false;
                Context.Invincible = false;
            }
        }
        public MarioContext Context { get; }
        private Sprite[,] normalSprites;
        private Sprite[,] largeSprites;
        public Sprite Sprite => Context.PowerUpStates.CurrentState == PowerUpEnum.NORMAL ?
                normalSprites[(int)Context.ActionStates.CurrentState, timer / Delay % 3] :
                largeSprites[(int)Context.ActionStates.CurrentState, timer / Delay % 3];
        public bool Active
        {
            get => active;
            set
            {
                active = value;
                if (value) timer = 600;
                Context.Invincible = true;
            }
        }
    }
}