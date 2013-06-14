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
		public Bounds bounds;

		public BoundingBox(Vector2 vect, Bounds bounds) : this(vect.X, vect.Y, bounds) { }
		public BoundingBox(float x, float y, Bounds bounds)
		{
			this.x      = x;
			this.y      = y;
			this.bounds = bounds;
		}

		public bool isColliding(BoundingBox other)
		{
			float myLeft = x - bounds.halfWidth;
			float myRight = x + bounds.halfWidth;
			float myTop = y - bounds.halfHeight;
			float myBottom = y + bounds.halfHeight;

			float theirLeft = other.x - other.bounds.halfWidth;
			float theirRight = other.x + other.bounds.halfWidth;
			float theirTop = other.y - other.bounds.halfHeight;
			float theirBottom = other.y + other.bounds.halfHeight;

			if (myLeft > theirRight ||
				myRight < theirLeft ||
				myTop > theirBottom ||
				myBottom < theirTop)
			{
				return false;
			}

			return true;
		}

		public void setPosition(Vector2 pos)
		{
			this.x = pos.X;
			this.y = pos.Y;
		}
	}
}
