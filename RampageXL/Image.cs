using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using TexLib;

using RampageXL.Shape;

namespace RampageXL
{
	class Image
	{
		private int _id;
		private Color4 color;

		public Image(String path)
		{
			if (!System.IO.File.Exists(path)) {
				Console.Error.Write("File not found:" + path);
				return;
			}

			_id = TexUtil.CreateTextureFromFile(path);
			color = new Color4(255, 255, 255, 255);
		}

		public void Draw(Vector3 position, Bounds bounds)
		{
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, _id);
			GL.Begin(BeginMode.Quads);
			GL.Color4(color);

			GL.TexCoord2(0.0, 0.0);
			GL.Vertex2(position.X - bounds.halfWidth, position.Y - bounds.halfHeight);
			GL.TexCoord2(0.0, 1.0);
			GL.Vertex2(position.X - bounds.halfWidth, position.Y + bounds.halfHeight);
			GL.TexCoord2(1.0, 1.0);
			GL.Vertex2(position.X + bounds.halfWidth, position.Y + bounds.halfHeight);
			GL.TexCoord2(1.0, 0.0);
			GL.Vertex2(position.X + bounds.halfWidth, position.Y - bounds.halfHeight);

			GL.End();
			GL.Disable(EnableCap.Texture2D);
		}
	}
}
