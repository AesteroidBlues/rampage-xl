using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

using RampageXL.Shape;

namespace RampageXL.Entity
{
	class Player : GameObject
	{
        readonly int PLAYER_WIDTH = 64;
        readonly int PLAYER_HEIGHT = 64;

		Vector2 pos;
        Bounds bounds;

		private bool moveLeft;
		private bool moveRight;

		public Player() : this(0, 0) {}
		public Player(int x, int y) : this(new Vector2(x, y)) {}
		public Player(Vector2 p) {
			pos = p;
			rectangle = new Rectangle(p.X, p.Y, PLAYER_WIDTH, PLAYER_HEIGHT);
			rectangle.setColor(200, 50, 50);

            bounds = new Bounds(PLAYER_WIDTH, PLAYER_HEIGHT);

            boundingBox = new BoundingBox(p.X, p.Y, bounds);

			XLG.keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(OnKeyDown);
			XLG.keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);
		}

		public void OnKeyDown(Object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.A) {
				moveLeft = true;
			} if (e.Key == Key.D) {
				moveRight = true;
			}
		}

		public void OnKeyUp(Object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.A) {
				moveLeft = false;
			} if (e.Key == Key.D) {
				moveRight = false;
			}
		}

		public void Update()
		{
			if (moveLeft) { pos.X -= 1; }
			if (moveRight) { pos.X += 1; }

            if (moveLeft || moveRight)
            {
                rectangle.setPosition(pos);
                boundingBox.setPosition(pos);
            }

		}

		public override void Draw()
		{
			rectangle.Draw();
		}
	}
}
