using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using RampageXL.Shape;

namespace RampageXL
{
	class Game : GameWindow
	{

		public Game()
			: base(Config.WindowWidth, Config.WindowHeight, Config.GraphicsMode, Config.Title)
		{
			VSync = VSyncMode.On;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			XLG.Init();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			XLG.RenderFrame();

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
