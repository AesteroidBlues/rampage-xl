using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using RampageXL.Shape;
using RampageXL.Mugic;
using RampageXL.Entity;

namespace RampageXL
{
	class Game : GameWindow
	{
		List<Building> buildings;

		Player p;

		public Game()
			: base(Config.WindowWidth, Config.WindowHeight, Config.GraphicsMode, Config.Title)
		{
			VSync = VSyncMode.On;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			XLG.keyboard = this.Keyboard;
			MugicConnection.Connect(Config.CalVRIP);
			p = new Player(400, 300);

			buildings = new List<Building>();

			buildings.Add(new Building(new Vector2(90, 180), new Bounds(250, 300)));
			buildings.Add(new Building(new Vector2(1000, 180), new Bounds(250, 300)));

			XLG.Init();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			MugicObjectManager.SendShapes();

			p.Update();

			//Collision checking
			foreach (Building b in buildings)
			{
                b.Update();
				if (b.isColliding(p))
				{
					Console.Write("\nLOOK OUT JC A COLLISION (with " + b.ToString() + ")!\n");
				}
			}
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			XLG.RenderFrame();
			foreach (Building b in buildings)
			{
				b.Draw();
			}
            p.Draw();

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
