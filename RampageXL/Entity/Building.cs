using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using RampageXL.Shape;
using RampageXL.AnimationPackage;
using RampageXL.Timing;

namespace RampageXL.Entity
{
    class Building : GameObject
    {
        public int health {get; set;}

        private Animation currentAnim;

        private Animation standingAnim;
        private Rectangle standing000;
        private Rectangle standing001;
        private Rectangle standing002;

        private Animation hitAnim;
        private Rectangle hit000;
        private Rectangle hit001;
        private Rectangle hit002;

        private Animation collapsing;

        public bool hit { get; set; }
        public Timer hitAnimTimer;

        public Building(Vector2 pos, Bounds bounds)
        {
            health = 5;

            this.pos = pos;
            this.boundingBox = new BoundingBox(pos.X, pos.Y, bounds);

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

            hit000 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
            hit001 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
            hit002 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
            hit000.setTexture("../../res/tex/building/hit000.png");
            hit001.setTexture("../../res/tex/building/hit001.png");
            hit002.setTexture("../../res/tex/building/hit002.png");
            List<Rectangle> hitFrames = new List<Rectangle>();
            hitFrames.Add(hit000);
            hitFrames.Add(hit001);
            hitFrames.Add(hit002);
            hitAnim = new Animation(hitFrames, 8, AnimationMode.LOOP);

            hit = false;
            hitAnimTimer = new Timer(30);

            currentAnim = standingAnim;
        }

        public override void Update()
        {
            if (hit)
            {
                currentAnim = hitAnim;
                hitAnimTimer.startTimer();
                hit = false;
            }
            if (hitAnimTimer.timeIsUp())
            {
                currentAnim = standingAnim;
            }
            hitAnimTimer.Update();
            currentAnim.Update();
        }

        public override void Draw()
        {
            currentAnim.Draw(pos);
        }
    }
}
