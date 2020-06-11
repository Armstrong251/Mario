using Mario.Entities.Mario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.Entities.Commands
{
    class Invincibility : Command
    {
        private const int FLASH_TIME = 2;
        private int spriteTimer = FLASH_TIME;
        private Sprite last;
        public override bool Invoke(Entity entity)
        {
            if(entity is MarioContext mario)
            {
                if (last != null && mario.Sprite != last) last.Visible = true;
                if (spriteTimer == 0)
                {
                    mario.Sprite.Visible = !mario.Sprite.Visible;
                    last = mario.Sprite;
                    spriteTimer = FLASH_TIME;
                }
                else
                {
                    spriteTimer--;
                }
                if (Condition.EvaluateCondition(entity))
                {
                    mario.Invincible = false;
                    mario.Sprite.Visible = true;
                }
            }
            return base.Invoke(entity);
        }
    }
}

