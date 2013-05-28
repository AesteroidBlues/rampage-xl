using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using RampageXL.Shape;

namespace RampageXL.Entity
{
    class BoundingBox
    {
        private float x;
        private float y;
        private Bounds bounds;

        public BoundingBox(float x, float y, Bounds bounds)
        {
            this.x      = x;
            this.y      = y;
            this.bounds = bounds;
        }

        public bool isColliding(BoundingBox other)
        {
            if ((other.x > this.x) &&
                (other.x < (this.x + bounds.width)) &&
                (other.y > this.y) &&
                (other.y < (this.y + bounds.height)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
