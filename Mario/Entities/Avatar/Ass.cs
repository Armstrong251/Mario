using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities.Avatar
{
    class Ass
    {
        private Sprite tailbone;
        private Sprite cheeks;

        public Ass (Sprite tailbone, Sprite cheeks)
        {
            this.tailbone = tailbone;
            this.cheeks = cheeks;

        }
        public void Draw(SpriteBatch batch)
        {
            //Give position based on center point 
            tailbone.Draw(batch, (0, 0));
            cheeks.Draw(batch, (0, 0));
        }
    }
}
