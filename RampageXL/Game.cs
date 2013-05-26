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
			p = new Player(90, 90);

			XLG.Init();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			XLG.RenderFrame();
			p.Update();
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
