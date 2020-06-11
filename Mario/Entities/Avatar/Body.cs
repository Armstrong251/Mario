using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mario;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities.Avatar
{
    public enum Chestparts
    {
        ribcage, 
        heart,
        lungs,
        liver,
        intestines, 
        stomach
    }
    class Body
    {
        private Sprite ribcage;
        private Sprite heart;
        private Sprite lungs;
        private Sprite liver;
        private Sprite intestines;
        private Sprite stomach;

        public Body (Sprite ribcage, Sprite heart, Sprite lungs, Sprite liver, Sprite intestines, Sprite stomach)
        {
            this.ribcage = ribcage;
            this.heart = heart;
            this.lungs = lungs;
            this.liver = liver;
            this.intestines = intestines;
            this.stomach = stomach;
        }
        public void Draw(SpriteBatch batch)
        {
            //Give position based on center point 
            ribcage.Draw(batch, (0, 0));
            heart.Draw(batch, (0, 0));
            lungs.Draw(batch, (0, 0));
            liver.Draw(batch, (0, 0));
            intestines.Draw(batch, (0, 0));
            stomach.Draw(batch, (0, 0));

        }
        public void ReplaceHeart(Sprite heart)
        {
            this.heart = heart;
        }
        public void ReplaceIntestines(Sprite intesti)
        {
            this.intestines = intesti;
        }
        public void ReplaceRibcage(Sprite ribcage)
        {
            this.ribcage = ribcage;
        }
        public void ReplaceLungs(Sprite lungs)
        {
            this.lungs = lungs;
        }
        public void ReplaceStomach(Sprite stomach)
        {
            this.stomach = stomach;
        }
        public void ReplaceLiver(Sprite liver)
        {
            this.liver = liver;
        }
    }
}
