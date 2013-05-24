using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using RampageXL.shape;

namespace RampageXL
{
	class Game : GameWindow
	{

		Rectangle r;

		public Game()
			: base(1280, 360, GraphicsMode.Default, "RampageXL")
		{
			VSync = VSyncMode.On;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Ortho(0, 1280, 360, 0, 0, 100);

			r = new Rectangle(0, 0, 20, 20);
			r.setColor(90, 40, 60);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			r.Draw();

			SwapBuffers();
		}

		static void Main()
		{
			using (Game game = new Game())
			{
				game.Run();
			}
		}
	}
}
