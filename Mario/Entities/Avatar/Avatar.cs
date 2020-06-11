using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;


namespace Mario.Entities.Avatar
{
    class Avatar
    {
        private Body body;
        private Head head;
        private LeftArm leftarm;
        private LeftLeg leftleg;
        private RightArm rightarm;
        private RightLeg rightleg;
        public Points center; 


        public Avatar(Points center, Sprite ribcage, Sprite heart, Sprite lungs, Sprite liver, Sprite intestines, Sprite stomach, Sprite brain, Sprite lefteye, Sprite righteye, Sprite mouth, Sprite upperarm, Sprite lowerarm, Sprite hand, Sprite upperleg, Sprite knee, Sprite lowerleg, Sprite foot) {
            head = new Head(brain, lefteye, righteye, mouth);
            body = new Body(ribcage, heart, lungs, liver, intestines, stomach);
            leftarm = new LeftArm(upperarm, lowerarm, hand);
            rightarm = new RightArm(upperarm, lowerarm, hand);
            leftleg = new LeftLeg(upperleg, knee, lowerleg, foot);
            rightleg = new RightLeg(upperleg, knee, lowerleg, foot);
            this.center = center;
        }

        public void Draw(SpriteBatch batch)
        {
            body.Draw(batch);
            head.Draw(batch);
            leftarm.Draw(batch);
            rightarm.Draw(batch);
            leftleg.Draw(batch);
            rightleg.Draw(batch);

        }

        public void ChangeBody(Sprite part , int decider)
        {
            Chestparts ID = (Chestparts)decider; 
            switch (ID)
            {
                case (Chestparts.heart):
                    this.body.ReplaceHeart(part);
                    break;
                case (Chestparts.intestines):
                    this.body.ReplaceIntestines(part);
                    break;
                case (Chestparts.lungs):
                    this.body.ReplaceLungs(part);
                    break;
                case (Chestparts.ribcage):
                    this.body.ReplaceRibcage(part);
                    break;
                case (Chestparts.liver):
                    this.body.ReplaceLiver(part);
                    break;
                case (Chestparts.stomach):
                    this.body.ReplaceStomach(part);
                    break;


            }
        }

        
    }
}