using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using RampageXL.Shape;
using RampageXL.AnimationPackage;

namespace RampageXL.Entity
{
    class Building : GameObject
    {
        private Animation currentAnim;

        private Animation standingAnim;
        private Rectangle standing000;
        private Rectangle standing001;
        private Rectangle standing002;

        private Animation collapsing;

        public Building(Vector2 pos, Bounds bounds)
        {


            this.pos = pos;
            this.boundingBox = new BoundingBox(pos.X, pos.Y, bounds);
            this.rectangle = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);

            standing000 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
            standing001 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
            standing002 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
            standing000.setTexture("../../res/tex/building/standing000.png");
            standing001.setTexture("../../res/tex/building/standing001.png");
            standing002.setTexture("../../res/tex/building/standing002.png");
            List<Rectangle> standingFrames = new List<Rectangle>();
            standingFrames.Add(standing000);
            standingFrames.Add(standing001);
            standingFrames.Add(standing002);
            standingAnim = new Animation(standingFrames, 100, AnimationMode.LOOP);

            rectangle = standing000;

            currentAnim = standingAnim;

            rectangle.setColor(255, 0, 255);
        }

        public void Update()
        {
            currentAnim.Update();
        }

        public override void Draw()
        {
            currentAnim.Draw(pos);
        }
    }
}
