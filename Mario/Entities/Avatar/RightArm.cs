using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;


namespace Mario.Entities.Avatar
{
    class RightArm
    {
        private Sprite upperarm;
        private Sprite lowerarm;
        private Sprite hand;
        public RightArm(Sprite upperarm, Sprite lowerarm, Sprite hand)
        {
            this.upperarm = upperarm;
            this.lowerarm = lowerarm;
            this.hand = hand;

        }
        public void Draw(SpriteBatch batch)
        {
            //Give position based on center point 
            upperarm.Draw(batch, (0, 0));
            lowerarm.Draw(batch, (0, 0));
            hand.Draw(batch, (0, 0));


        }
    }
}
