using System;
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
		public Bounds bounds;
		public Vector3 position;
		private Color4 color;
		private Image image;

		private bool dirty = false;
		private bool hidden = false;

		public Rectangle(float x, float y, int w, int h) : this(x, y, 0, w, h) { }

		public Rectangle(float x, float y, int z, int w, int h) {
			bounds = new Bounds(w, h);
			position = new Vector3(x, y, z);
			MugicObjectManager.Register(this);
		}

		public Rectangle setColor(byte r, byte g, byte b) {
			return setColor(new Color4(r, g, b, 255));
		}

		public Rectangle setColor(byte r, byte g, byte b, byte a) {
			return setColor(new Color4(r, g, b, a));
		}

		public Rectangle setColor(Color4 c) {
			dirty = true;
			color = c;

			return this;
		}

		public Rectangle setPosition(float x, float y) {
			return setPosition(new Vector2(x, y));
		}

		public Rectangle setPosition(Vector2 p) {
			
			if (position.X == p.X && position.Y == p.Y)
			{
				return this;
			}

			dirty = true;
			position.X = p.X;
			position.Y = p.Y;

			return this;
		}

		public Rectangle setTexture(string path)
		{
			image = new Image(path);
			dirty = true;
			return this;
		}

		public void setTexture(ImageName name)
		{
			image = ImageManager.GetImage(name);
			dirty = true;
		}

		public void Hide()
		{
			hidden = true;
			dirty = true;
			this.setColor(255, 255, 255, 0);
		}

		public void Unhide()
		{
			hidden = false;
			dirty = true;
			this.setColor(255, 255, 255, 255);
		}

		public override void Draw()  {
			if(hidden) return;
			if (image != null)
			{
				image.Draw(position, bounds);
			}
			else
			{
				GL.Begin(BeginMode.TriangleStrip);
				GL.Color4(color);

				GL.TexCoord2(0.0, 1.0);
				GL.Vertex2(position.X - bounds.halfWidth, position.Y + bounds.halfHeight); // LL  2---4
				GL.TexCoord2(0.0, 0.0);
				GL.Vertex2(position.X - bounds.halfWidth, position.Y - bounds.halfHeight); // UL  | \ |
				GL.TexCoord2(1.0, 1.0);
				GL.Vertex2(position.X + bounds.halfWidth, position.Y + bounds.halfHeight); // LR  |  \|
				GL.TexCoord2(1.0, 0.0);
				GL.Vertex2(position.X + bounds.halfWidth, position.Y - bounds.halfHeight); // UR  1---3 

				GL.End();
			}
		}

		/// <summary>
		/// ONLY USE THIS IF YOU NEED TO SEND THE RECT AS A MUGIC PACKET.
		/// </summary>
		public void MarkDirty()
		{
			dirty = true;
		}

		public override MugicPacket GetUpdate() {
			if(!dirty) return null;

			MugicPacket packet = new MugicPacket();

			base.InitPacket(MugicCommand.Rectangle, packet);

			packet.Parameter(MugicParam.X, position.X * Config.WallScalar);
			packet.Parameter(MugicParam.Y, position.Y * Config.WallScalar);
			packet.Parameter(MugicParam.Z, position.Z * Config.WallScalar);
			packet.Parameter(MugicParam.a, color.A);

			packet.Parameter(MugicParam.Width, bounds.width * Config.WallScalar);
			packet.Parameter(MugicParam.Height, bounds.height * Config.WallScalar);
			

			if (image != null)
			{
				packet.Parameter(MugicParam.Texture, image.Filename);
			}

			dirty = false;

			return packet;
		}
	}
}
