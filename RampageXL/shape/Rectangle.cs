﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using RampageXL.Mugic;

namespace RampageXL.Shape
{
	class Rectangle : Drawable
	{
		private Bounds bounds;
		private Vector3 position;
		private Color4 color;
		private bool wasCreated = false;

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

		public override void Draw() 
		{
			GL.Begin(BeginMode.TriangleStrip);
			GL.Color4(color);

			GL.Vertex2(position.X - bounds.halfWidth, position.Y + bounds.halfHeight); // LL  2---4
			GL.Vertex2(position.X - bounds.halfWidth, position.Y - bounds.halfHeight); // UL  | \ |
			GL.Vertex2(position.X + bounds.halfWidth, position.Y + bounds.halfHeight); // LR  |  \|
			GL.Vertex2(position.X + bounds.halfWidth, position.Y - bounds.halfHeight); // UR  1---3 

			GL.End();
		}

		public void Send()
		{
			MugicPacket packet = new MugicPacket();
			base.InitPacket(MugicCommand.Rectangle, packet);

			packet.Parameter(MugicParam.X, position.X);
			packet.Parameter(MugicParam.Y, position.Y);
			packet.Parameter(MugicParam.Z, position.Z);

			packet.Parameter(MugicParam.Width, bounds.width);
			packet.Parameter(MugicParam.Height, bounds.height);
		}
	}
}