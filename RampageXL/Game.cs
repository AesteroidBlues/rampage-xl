﻿using System;
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
			ImageManager.Init();

			ImageManager.LoadImage(ImageName.player_standing_L00, "../../res/tex/player/george_standingL000.png");
			ImageManager.LoadImage(ImageName.player_standing_R00, "../../res/tex/player/george_standingR000.png");
			ImageManager.LoadImage(ImageName.player_walking_L00, "../../res/tex/player/george_walkL000.png");
			ImageManager.LoadImage(ImageName.player_walking_L01, "../../res/tex/player/george_walkL001.png");
			ImageManager.LoadImage(ImageName.player_walking_L02, "../../res/tex/player/george_walkL002.png");
			ImageManager.LoadImage(ImageName.player_walking_R00, "../../res/tex/player/george_walkR000.png");
			ImageManager.LoadImage(ImageName.player_walking_R01, "../../res/tex/player/george_walkR001.png");
			ImageManager.LoadImage(ImageName.player_walking_R02, "../../res/tex/player/george_walkR002.png");


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
			List<Building> buildingsToRemove = new List<Building>();
			foreach (Building b in buildings)
			{
				b.Update();
				if (b.isColliding(p))
				{
					Console.Write("\nLOOK OUT JC A COLLISION (with " + b.ToString() + ")!\n");
				}
				if (p.currentPunch != null && b.isColliding(p.currentPunch))
				{
					b.health--;
					b.hit = true;
					if (b.health <= 0)
					{
						buildingsToRemove.Add(b);
					}
				}
			}
			foreach (Building b in buildingsToRemove)
			{
				buildings.Remove(b);
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
