using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace RampageXL
{
	class XLG
	{

		public static KeyboardDevice keyboard;

		public static void Init()
		{
			GL.FrontFace(FrontFaceDirection.Cw);
			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Ortho(0, 1280, 360, 0, 0, 100);
		}

		public static void RenderFrame()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		}
	}
}
