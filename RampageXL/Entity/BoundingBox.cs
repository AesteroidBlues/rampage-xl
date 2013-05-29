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
            float otherLeftEdge = other.x;
            float otherRightEdge = other.x + other.bounds.width;
            float otherTopEdge = other.y + other.bounds.height;
            float otherBottomEdge = other.y;

            float leftEdge = x;
            float rightEdge = x + bounds.width;
            float topEdge =  y + bounds.height;
            float bottomEdge = y;

            bool left = (otherLeftEdge >= leftEdge) && (otherLeftEdge <= (rightEdge));
            bool right = (otherRightEdge > leftEdge) && (otherRightEdge <= (rightEdge));
            bool top = (otherTopEdge >= bottomEdge) && (otherTopEdge < topEdge) ;
            bool bot = (otherBottomEdge >= bottomEdge) && (otherBottomEdge < topEdge);
            bool insideX = (otherLeftEdge < leftEdge) && (otherRightEdge > rightEdge);
            bool insideY = (otherTopEdge < topEdge) && (otherBottomEdge > bottomEdge);
            bool surroundOtherX = (otherLeftEdge > leftEdge) && (otherRightEdge < rightEdge);
            bool surroundOtherY = (otherTopEdge > topEdge) && (otherBottomEdge < bottomEdge);

            if (
                    (left && top)
                    ||
                    (left && bot)
                    ||
                    (right && top)
                    ||
                    (right && bot)
                    ||
                    insideX || insideY || surroundOtherX || surroundOtherY
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setPosition(Vector2 pos)
        {
            this.x = pos.X;
            this.y = pos.Y;
        }
    }
}
