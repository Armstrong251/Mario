using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mario.Entities.Mario.ActionEnum;
using static Mario.Entities.Mario.PowerUpEnum;

namespace Mario.Entities.Mario
{
    public class MarioFactory : Factory
    {
        public MarioFactory(Level level) : base(level) { }
        private Dictionary<PowerUpEnum, Func<MarioContext, Sprite[], PowerUpState>> powers = new Dictionary<PowerUpEnum, Func<MarioContext, Sprite[], PowerUpState>>
        {
            [NORMAL] = (m, s) => new NormalState(m, s),
            [SUPER] = (m, s) => new SuperState(m, s),
            [FIRE] = (m, s) => new FireState(m, s),
        };
        private Dictionary<ActionEnum, Func<MarioContext, ActionState>> actions = new Dictionary<ActionEnum, Func<MarioContext, ActionState>>
        {
            [STANDING] = (m) => new StandingState(m),
            [WALKING] = (m) => new WalkingState(m),
            [JUMPING] = (m) => new JumpingState(m),
            [CROUCHING] = (m) => new CrouchingState(m),
            [DEATH] = (m) => new DeathState(m),
            [FLAG] = (m) => new FlagState(m),
        };
        public override Entity Create(Point position, Enum id)
        {
            MarioContext mc = new MarioContext(Level, position, this);
            mc.PowerUpStates.CurrentState = (PowerUpEnum)id;
            return mc;
        }
        public StateList<ActionState, ActionEnum> CreateActionStates(MarioContext context)
        {
            ActionState[] actionStates = new ActionState[actions.Count];
            foreach (KeyValuePair<ActionEnum, Func<MarioContext, ActionState>> t in actions)
            {
                actionStates[(int)t.Key] = t.Value(context);
            }
            return new StateList<ActionState, ActionEnum>(actionStates, STANDING);
        }
        public StateList<PowerUpState, PowerUpEnum> CreatePowerUpStates(MarioContext context)
        {
            PowerUpState[] powerUpStates = new PowerUpState[powers.Count];
            foreach (KeyValuePair<PowerUpEnum, Func<MarioContext, Sprite[], PowerUpState>> t in powers)
            {
                powerUpStates[(int)t.Key] = t.Value(context, CreateSprites(t.Key));
            }
            return new StateList<PowerUpState, PowerUpEnum>(powerUpStates, NORMAL);
        }
        public override Sprite CreateSprite(Enum id, Enum state)
        {
            return CreateSprites((PowerUpEnum)id)[(int)(ActionEnum)state];
        }
        public StarState CreateStarState(MarioContext context)
        {
            Texture2D texture = Content.Load<Texture2D>("Sprites/StarMario");
            Dictionary<ActionEnum, Point[]> animations = new Dictionary<ActionEnum, Point[]>();
            Dictionary<ActionEnum, int> delay = new Dictionary<ActionEnum, int>();
            animations[DEATH] = new[] { new Point(16, 0) };
            delay[FLAG] = 4;

            animations[WALKING] = new[] { new Point(208, 0), new Point(224, 0), new Point(240, 0) };
            animations[STANDING] = new[] { new Point(144, 0) };
            animations[JUMPING] = new[] { new Point(176, 0) };
            animations[FLAG] = new[] { new Point(128, 0), new Point(112, 0) };
            animations[CROUCHING] = new[] { new Point(160, 0) };
            Sprite[,] sprites = new Sprite[actions.Count, 3];
            Sprite[,] supersprites = new Sprite[actions.Count, 3];
            for (int j = 0; j < 3; j++)
            {
                (Point[] large, Point[] small)[] anim = animations.OrderBy(p => p.Key)
                            .Select(a => 
                                (a.Value.Select(pt => pt + new Point(0, 48 * j)).ToArray(),
                                a.Value.Select(pt => pt + new Point(0, 32 + 48 * j)).ToArray())
                            ).ToArray();
                anim[(int)CROUCHING].small = anim[(int)STANDING].small;
                for (int i = 0; i < actions.Count; i++)
                {
                    sprites[i, j] = new Sprite(texture,
                        new Point(16), anim[(int)i].small, delay.ContainsKey((ActionEnum)i) ? delay[(ActionEnum)i] : Sprite.DefaultDelay);
                    supersprites[i, j] = new Sprite(texture,
                        new Point(16, 32), anim[(int)i].large, delay.ContainsKey((ActionEnum)i) ? delay[(ActionEnum)i] : Sprite.DefaultDelay);
                }
            }
            return new StarState(context, sprites, supersprites);
        }
        /* Still need to update the sprite sheet associated once it gets uploaded*/
        public Sprite[] CreateSprites(PowerUpEnum ID)
        {
            Dictionary<ActionEnum, Point> sizes = new Dictionary<ActionEnum, Point>();
            Dictionary<ActionEnum, Point[]> animations = new Dictionary<ActionEnum, Point[]>();
            Dictionary<ActionEnum, int> delay = new Dictionary<ActionEnum, int>();
            sizes[DEATH] = new Point(16, 16);
            animations[DEATH] = new[] { new Point(16, 0) };
            delay[FLAG] = 4;
            switch (ID)
            {
                case NORMAL:
                    sizes[WALKING] = new Point(16, 16);
                    animations[WALKING] = new[] { new Point(64, 0), new Point(80, 0), new Point(96, 0) };
                    sizes[STANDING] = new Point(16, 16);
                    animations[STANDING] = new[] { new Point(112, 0) };
                    sizes[JUMPING] = new Point(16, 16);
                    animations[JUMPING] = new[] { new Point(32, 0) };
                    sizes[FLAG] = new Point(16, 16);
                    animations[FLAG] = new[] { new Point(32, 16), new Point(48, 16) };
                    sizes[CROUCHING] = sizes[STANDING];
                    animations[CROUCHING] = animations[STANDING];
                    break;
                case SUPER:
                    sizes[WALKING] = new Point(16, 32);
                    animations[WALKING] = new[] { new Point(64, 32), new Point(80, 32), new Point(96, 32) };
                    sizes[STANDING] = new Point(16, 32);
                    animations[STANDING] = new[] { new Point(112, 32) };
                    sizes[JUMPING] = new Point(16, 32);
                    animations[JUMPING] = new[] { new Point(32, 32) };
                    sizes[CROUCHING] = new Point(16, 32);
                    animations[CROUCHING] = new[] { new Point(16, 32) };
                    sizes[FLAG] = new Point(16, 32);
                    animations[FLAG] = new[] { new Point(0, 64), new Point(16, 64) };
                    break;
                case FIRE:
                    sizes[WALKING] = new Point(16, 32);
                    animations[WALKING] = new[] { new Point(64, 96), new Point(80, 96), new Point(96, 96) };
                    sizes[STANDING] = new Point(16, 32);
                    animations[STANDING] = new[] { new Point(112, 96) };
                    sizes[JUMPING] = new Point(16, 32);
                    animations[JUMPING] = new[] { new Point(16, 96) };
                    sizes[CROUCHING] = new Point(16, 32);
                    animations[CROUCHING] = new[] { new Point(0, 96) };
                    sizes[FLAG] = new Point(16, 32);
                    animations[FLAG] = new[] { new Point(0, 128), new Point(16, 128) };
                    break;
            }
            Sprite[] sprites = new Sprite[actions.Count];
            for (int i = 0; i < actions.Count; i++)
            {
                if (sizes.ContainsKey((ActionEnum)i) && animations.ContainsKey((ActionEnum)i))
                {
                    sprites[i] = new Sprite(Content.Load<Texture2D>("Sprites/marioSheet3"),
                        sizes[(ActionEnum)i], animations[(ActionEnum)i], delay.ContainsKey((ActionEnum)i) ? delay[(ActionEnum)i] : Sprite.DefaultDelay);
                }
            }
            return sprites;
        }
    }
}
