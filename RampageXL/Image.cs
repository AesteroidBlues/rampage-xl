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

		private int _width;
		private int _height;

		public int width
		{
			get{return _width;}
		}

		public int height
		{
			get {return _height;}
		}

		private ImageName _name;
		public ImageName name 
		{
			get { return _name; }
		}

		private string _path;
		public string path
		{
			get { return _path; }
		}

		public string Filename
		{
			get
			{
				return path.Substring(path.LastIndexOf("/") + 1);
			}
		}

		public Image(String path)
		{
			if (!System.IO.File.Exists(path)) {
				Console.Error.Write("File not found:" + path);
				return;
			}

			_path = path;
			_id = TexUtil.CreateTextureFromFile(path, out _width, out _height);
			color = new Color4(255, 255, 255, 255);
		}

		public void SetName(ImageName name)
		{
			_name = name;
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
