using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RampageXL.shape
{
	class Rectangle : Drawable
	{
		private Bounds bounds;
		private Vector3 position;
		private Color4 color;

		public Rectangle(int x, int y, int w, int h)
		{
			bounds = new Bounds(w, h);
			position = new Vector3(x, y, 0);
		}

		public Rectangle(int x, int y, int z, int w, int h)
		{
			bounds = new Bounds(w, h);
			position = new Vector3(x, y, z);
		}

		public Rectangle setColor(byte r, byte g, byte b)
		{
			color = new Color4(r, g, b, 255);
			return this;
		}

		public Rectangle setColor(byte r, byte g, byte b, byte a)
		{
			color = new Color4(r, g, b, a);
			return this;
		}

		public Rectangle setColor(Color4 c)
		{
			color = c;
			return this;
		}

		public void Draw() 
		{
			GL.FrontFace(FrontFaceDirection.Cw);
			GL.Begin(BeginMode.TriangleStrip);
			GL.Color4(color);

			GL.Vertex2(position.X - bounds.halfWidth, position.Y + bounds.halfHeight);
			GL.Vertex2(position.X - bounds.halfWidth, position.Y - bounds.halfHeight);
			GL.Vertex2(position.X + bounds.halfWidth, position.Y + bounds.halfHeight);
			GL.Vertex2(position.X + bounds.halfWidth, position.Y - bounds.halfHeight);

			GL.End();
		}
	}
}
