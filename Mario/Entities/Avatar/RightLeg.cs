using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities.Avatar
{
    class RightLeg
    {
        private Sprite upperleg;
        private Sprite knee;
        private Sprite lowerleg;
        private Sprite foot;


        public RightLeg(Sprite upperleg, Sprite knee, Sprite lowerleg, Sprite foot)
        {
            this.upperleg = upperleg;
            this.knee = knee;
            this.lowerleg = lowerleg;
            this.foot = foot;
        }
        public void Draw(SpriteBatch batch)
        {
            upperleg.Draw(batch, (0, 0));
            lowerleg.Draw(batch, (0, 0));
            knee.Draw(batch, (0, 0));
            foot.Draw(batch, (0, 0));
        }
           
    }
}
