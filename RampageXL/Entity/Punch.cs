using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using RampageXL.Shape;

namespace RampageXL.Entity
{
    class Punch : GameObject
    {
        public Punch() : this(0, 0) {}
		public Punch(float x, float y) : this(new Vector2(x, y)) {}
        public Punch(Vector2 p)
        {
            pos = p;

            Bounds bounds = new Bounds(20, 20);
            boundingBox = new BoundingBox(pos, bounds);

            rectangle = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
            rectangle.setColor(255, 0, 0, 255);
        }

        public override void Update()
        {
            boundingBox.setPosition(pos);
        }

        public override void Draw()
        {
            rectangle.Draw();
        }
    }
}
