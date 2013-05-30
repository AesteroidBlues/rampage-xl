using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using RampageXL.Shape;

namespace RampageXL.Entity
{
    class Building : GameObject
    {
        public Building(Vector2 pos, Bounds bounds)
        {
            this.pos = pos;
            this.boundingBox = new BoundingBox(pos.X, pos.Y, bounds);
            this.rectangle = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);

            rectangle.setColor(255, 0, 255);
        }

        public override void Draw()
        {
            rectangle.Draw();
        }
    }
}
