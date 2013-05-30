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

using RampageXL.Shape;

namespace RampageXL
{
	class Image
	{
		private int _id;

		public Image(String path)
		{
			if (!System.IO.File.Exists(path)) {
				Console.Error.Write("File not found:" + path);
				return;
			}

			Bitmap textureBitmap = new Bitmap(path);
			BitmapData data = textureBitmap.LockBits(
				new System.Drawing.Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height),
				ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb
			);

			GL.GenTextures(1, out _id);
			GL.BindTexture(TextureTarget.Texture2D, _id);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
			OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

			textureBitmap.UnlockBits(data);
			textureBitmap.Dispose();
		}

		public void Draw(Vector3 position, Bounds bounds)
		{
			GL.BindTexture(TextureTarget.Texture2D, _id);
			GL.Begin(BeginMode.TriangleStrip);

			GL.TexCoord2(0.0, 0.0);
			GL.Vertex2(position.X - bounds.halfWidth, position.Y + bounds.halfHeight);
			GL.TexCoord2(1.0, 0.0);
			GL.Vertex2(position.X - bounds.halfWidth, position.Y - bounds.halfHeight);
			GL.TexCoord2(1.0, 1.0);
			GL.Vertex2(position.X + bounds.halfWidth, position.Y + bounds.halfHeight);
			GL.TexCoord2(0.0, 1.0);
			GL.Vertex2(position.X + bounds.halfWidth, position.Y - bounds.halfHeight);

			GL.End();
		}
	}
}
