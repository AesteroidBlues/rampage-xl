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

using System.Windows.Forms;

namespace RampageXL.Entity
{
	public enum Direction
	{
		Left,
		Right
	}

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

		public Vector2 pos;
		Bounds bounds;

		private bool moveLeft;
		private bool moveRight;
		private bool punching;

		private int facing;

		private List<RampageXL.Timing.Timer> timers;
        private RampageXL.Timing.Timer punchCooldown;
        private RampageXL.Timing.Timer punchLength;

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
            timers = new List<RampageXL.Timing.Timer>();
            punchCooldown = new RampageXL.Timing.Timer(40);
            punchLength = new RampageXL.Timing.Timer(2);

			timers.Add(punchCooldown);
			timers.Add(punchLength);

			/// ANIM INIT
			Rectangle standingL000 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			standingL000.setTexture("../../res/tex/player/george_standingL000.png");
			List<Rectangle> standingLFrames = new List<Rectangle>();
			standingLFrames.Add(standingL000);
			standingLAnim = new Animation(standingLFrames, 1, AnimationMode.PAUSE);

			Rectangle standingR000 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			standingR000.setTexture("../../res/tex/player/george_standingR000.png");
			List<Rectangle> standingRFrames = new List<Rectangle>();
			standingRFrames.Add(standingR000);
			standingRAnim = new Animation(standingRFrames, 1, AnimationMode.PAUSE);

			Rectangle walkingL000 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			Rectangle walkingL001 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			Rectangle walkingL002 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			walkingL000.setTexture("../../res/tex/player/george_walkL000.png");
			walkingL001.setTexture("../../res/tex/player/george_walkL001.png");
			walkingL002.setTexture("../../res/tex/player/george_walkL002.png");
			List<Rectangle> walkingLFrames = new List<Rectangle>();
			walkingLFrames.Add(walkingL000);
			walkingLFrames.Add(walkingL001);
			walkingLFrames.Add(walkingL002);
			walkingLAnim = new Animation(walkingLFrames, 8, AnimationMode.LOOP, LoopMode.PINGPONG);

			Rectangle walkingR000 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			Rectangle walkingR001 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			Rectangle walkingR002 = new Rectangle(pos.X, pos.Y, bounds.width, bounds.height);
			walkingR000.setTexture("../../res/tex/player/george_walkR000.png");
			walkingR001.setTexture("../../res/tex/player/george_walkR001.png");
			walkingR002.setTexture("../../res/tex/player/george_walkR002.png");
			List<Rectangle> walkingRFrames = new List<Rectangle>();
			walkingRFrames.Add(walkingR000);
			walkingRFrames.Add(walkingR001);
			walkingRFrames.Add(walkingR002);
			walkingRAnim = new Animation(walkingRFrames, 8, AnimationMode.LOOP, LoopMode.PINGPONG);

			currentAnim = standingLAnim;
		}

		public void OnKeyDown(Object sender, KeyboardKeyEventArgs e)
		{
            if (e.Key == Key.A)
            {
                moveLeft = true;
            } if (e.Key == Key.D)
            {
                moveRight = true;
            } if (e.Key == Key.Space && punchCooldown.timeIsUp())
            {
                punching = true;
                punchCooldown.startTimer();
                punchLength.startTimer();
            }
		}

		public void OnKeyUp(Object sender, KeyboardKeyEventArgs e)
		{
            if (e.Key == Key.A)
            {
                moveLeft = false;
            } if (e.Key == Key.D)
            {
                moveRight = false;
            } if (e.Key == Key.Space)
            {
                punching = false;
            }
		}

		public void DoPunch(Direction dir)
		{
			switch (dir)
			{
				case Direction.Left: DoPunchLeft(); break;
				case Direction.Right: DoPunchRight(); break;
			}
		}

		private void DoPunchLeft()
		{
			this.facing = 1;
			this.punching = true;
		}

		private void DoPunchRight()
		{
			this.facing = -1;
			this.punching = true;
		}

		public void SetPosition(Vector2 newPos)
		{
			this.rectangle.setPosition(newPos);
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
            foreach (RampageXL.Timing.Timer t in timers)
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
