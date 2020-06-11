using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities.Avatar
{
    class Head
    {
        private Sprite brain;
        private Sprite lefteye;
        private Sprite righteye;
        private Sprite mouth;

        public Head(Sprite brain, Sprite lefteye, Sprite righteye, Sprite mouth)
        {
            this.brain = brain;
            this.lefteye = lefteye;
            this.righteye = righteye;
            this.mouth = mouth;

        }
        public void Draw(SpriteBatch batch)
        {
            //Give position based on center point 
            brain.Draw(batch, (0, 0));
            lefteye.Draw(batch, (0, 0));
            righteye.Draw(batch, (0, 0));
            mouth.Draw(batch, (0, 0));
        }
    }

}
