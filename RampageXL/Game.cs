using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

namespace RampageXL
{
	class Game : GameWindow
	{

		public Game()
			: base(1280, 360, GraphicsMode.Default, "RampageXL")
		{
			VSync = VSyncMode.Off;
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
