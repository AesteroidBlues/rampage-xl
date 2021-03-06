﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

using RampageXL.Shape;
using RampageXL.AnimationPackage;
using RampageXL.Timing;

namespace RampageXL.Entity
{
	class Player : GameObject
	{
		private Animation currentAnim;
		private Animation standingLAnim;
		private Animation standingRAnim;
		private Animation walkingLAnim;
		private Animation walkingRAnim;

		public Punch currentPunch;

		readonly int PLAYER_WIDTH = 64;
		readonly int PLAYER_HEIGHT = 64;

		Vector2 pos;
		Bounds bounds;

		private bool moveLeft;
		private bool moveRight;
		private bool punching;

		private int facing;

		private List<Timer> timers;
		private Timer punchCooldown;
		private Timer punchLength;

		public Player() : this(0, 0) {}
		public Player(int x, int y) : this(new Vector2(x, y)) {}
		public Player(Vector2 p) {
			pos = p;
			//rectangle = new Rectangle(p.X, p.Y, PLAYER_WIDTH, PLAYER_HEIGHT);
			//rectangle.setTexture("../../res/tex/george.png");

			bounds = new Bounds(PLAYER_WIDTH, PLAYER_HEIGHT);

			boundingBox = new BoundingBox(p.X, p.Y, bounds);

			XLG.keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(OnKeyDown);
			XLG.keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);

			facing = -1;

			/// TIMER INIT
			timers  = new List<Timer>();
			punchCooldown = new Timer(40);
			punchLength = new Timer(2);

			timers.Add(punchCooldown);
			timers.Add(punchLength);

			/// ANIM INIT
			standingLAnim = new Animation(1, AnimationMode.PAUSE);
			standingRAnim = new Animation(1, AnimationMode.PAUSE);
			walkingLAnim = new Animation(8, AnimationMode.LOOP, LoopMode.PINGPONG);
			walkingRAnim = new Animation(8, AnimationMode.LOOP, LoopMode.PINGPONG);

			standingLAnim.AddFrame(ImageManager.GetImage(ImageName.player_standing_L00), bounds.width, bounds.height);
			standingRAnim.AddFrame(ImageManager.GetImage(ImageName.player_standing_R00), bounds.width, bounds.height);

			walkingLAnim.AddFrame(ImageManager.GetImage(ImageName.player_walking_L00), bounds.width, bounds.height);
			walkingLAnim.AddFrame(ImageManager.GetImage(ImageName.player_walking_L01), bounds.width, bounds.height);
			walkingLAnim.AddFrame(ImageManager.GetImage(ImageName.player_walking_L02), bounds.width, bounds.height);

			walkingRAnim.AddFrame(ImageManager.GetImage(ImageName.player_walking_R00), bounds.width, bounds.height);
			walkingRAnim.AddFrame(ImageManager.GetImage(ImageName.player_walking_R01), bounds.width, bounds.height);
			walkingRAnim.AddFrame(ImageManager.GetImage(ImageName.player_walking_R02), bounds.width, bounds.height);

			currentAnim = standingLAnim;
		}

		public void OnKeyDown(Object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.A) {
				moveLeft = true;
			} if (e.Key == Key.D) {
				moveRight = true;
			} if (e.Key == Key.Space && punchCooldown.timeIsUp()) {
				punching = true;
				punchCooldown.startTimer();
				punchLength.startTimer();
			}
		}

		public void OnKeyUp(Object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.A) {
				moveLeft = false;
			} if (e.Key == Key.D) {
				moveRight = false;
			} if (e.Key == Key.Space) {
				punching = false;
			}
		}

		public override void Update()
		{
			if (punching)
			{
				currentPunch = new Punch(pos.X + (int) (facing * 30), pos.Y);
				punching = false;
			}
			if (moveLeft) 
			{ 
				pos.X -= 1;
				currentAnim = walkingLAnim;
				facing = -1;
			}
			if (moveRight) 
			{
				pos.X += 1;
				currentAnim = walkingRAnim;
				facing = 1;
			}

			if (moveLeft || moveRight)
			{
				boundingBox.setPosition(pos);
			}

			if (!moveLeft && !moveRight)
			{
				if (facing == -1)
				{
					currentAnim = standingLAnim;
				}
				if (facing == 1)
				{
					currentAnim = standingRAnim;
				}
			}
			foreach (Timer t in timers)
			{
				t.Update();
			}
			if (punchLength.timeIsUp())
			{
				currentPunch = null;
			}

			currentAnim.Update();
		}

		public override void Draw()
		{
			//rectangle.Draw();
			currentAnim.Draw(pos);
			if (currentPunch != null)
				currentPunch.Draw();
		}
	}
}
