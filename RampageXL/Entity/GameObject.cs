using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

using RampageXL.Shape;

namespace RampageXL.Entity
{
    /// <summary>
    /// An interface for any object which can be directly interacted with by the player
    /// Object must support collisions and must define an update function
    /// </summary>
    abstract class GameObject
    {
        public Vector2     pos;
        public BoundingBox boundingBox;
        public Rectangle rectangle;

        public virtual bool isColliding(GameObject other)
        {
            //Console.Write("Checking collisions\n");
            return this.boundingBox.isColliding(other.boundingBox);
        }

        public abstract void Draw();
        public abstract void Update();
    }
}
