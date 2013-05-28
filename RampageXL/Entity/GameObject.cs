using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

namespace RampageXL.Entity
{
    /// <summary>
    /// An interface for any object which can be directly interacted with by the player
    /// </summary>
    abstract class GameObject
    {
        protected Vector2     pos;
        protected BoundingBox boundingBox;

        virtual bool isColliding(GameObject other)
        {
            return this.boundingBox.isColliding(other.boundingBox);
        }
    }
}
