using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using RampageXL.Shape;
using RampageXL.Mugic;
using RampageXL.Entity;

using Microsoft.Kinect;
using GestureFramework;
using WindowsInput;

using System.Windows.Forms;

namespace RampageXL
{
	class Game : GameWindow
	{
		//KINECT VARS
		public KinectSensor sensor;
		public Skeleton[] skeletons;
		public SkeletonFrame skeletonFrame;

		public GestureMap _gestureMap;
		public Dictionary<int, GestureMapState> _gestureMaps;
		public string GestureFileName;

		public int playerId;
		//END KINECT VARS

		List<Building> buildings;

		Player p;

        Random rand;

		public Game()
			: base(Config.WindowWidth, Config.WindowHeight, Config.GraphicsMode, Config.Title)
		{
			VSync = VSyncMode.On;
		}

		protected override void OnLoad(EventArgs e)
		{
			//KINECT LOAD
			// First Load the XML File that contains the application configuration
			_gestureMap = new GestureMap();
			GestureFileName = "../../gestures.xml";
			_gestureMap.LoadGesturesFromXml(GestureFileName);

			sensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
			sensor.SkeletonStream.Enable();
			skeletons = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];
			sensor.Start();

			// Instantiate the in memory representation of the gesture state for each player
			_gestureMaps = new Dictionary<int, GestureMapState>();
			//END KINECT LOAD

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

            rand = new Random();

			XLG.Init();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			MugicObjectManager.SendShapes();

			SkeletonEval();
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
                float centerX = b.pos.X;
                float leftX = centerX - b.rectangle.bounds.width - b.rectangle.bounds.width / 5.0f;
                float rightX = centerX + b.rectangle.bounds.width + b.rectangle.bounds.width / 5.0f;

                float tempX = (float)rand.Next((int)Config.WindowWidth - b.rectangle.bounds.width);
                while (tempX < rightX && tempX > leftX)
                {
                    tempX = (float)rand.Next((int)Config.WindowWidth - b.rectangle.bounds.width);
                }

                int tempW = b.rectangle.bounds.width;
                int tempH = b.rectangle.bounds.height;

				buildings.Remove(b);

                buildings.Add(new Building(new Vector2(tempX, 180), new Bounds(tempW, tempH)));
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

		//KINECT METHODS
		void SkeletonEval()
		{
			using (SkeletonFrame skeletonFrameData = sensor.SkeletonStream.OpenNextFrame(40))
			{
				if (skeletonFrameData == null)
				{
					return;
				}

				var allSkeletons = new Skeleton[skeletonFrameData.SkeletonArrayLength];
				skeletonFrameData.CopySkeletonDataTo(allSkeletons);

				foreach (Skeleton sd in allSkeletons)
				{
					// If this skeleton is no longer being tracked, skip it
					if (sd.TrackingState != SkeletonTrackingState.Tracked)
					{
						continue;
					}

					// If there is not already a gesture state map for this skeleton, then create one
					if (!_gestureMaps.ContainsKey(sd.TrackingId))
					{
						var mapstate = new GestureMapState(_gestureMap);
						_gestureMaps.Add(sd.TrackingId, mapstate);
					}

					var keycode = _gestureMaps[sd.TrackingId].Evaluate(sd, false, Config.WindowWidth, Config.WindowHeight);

					if (keycode != VirtualKeyCode.NONAME)
					{
						switch (keycode)
						{
						case VirtualKeyCode.RBUTTON:
							p.DoPunch(Direction.Right);
							break;
						case VirtualKeyCode.LBUTTON:
							p.DoPunch(Direction.Left);
							break;
						 default:
							break;
						}

						_gestureMaps[sd.TrackingId].ResetAll(sd);
					}

					SkeletonPoint sp = CalculateJointPosition(sd.Joints[JointType.HipCenter]);
                    float newX = sp.X * 2.0f;
                    if (newX < 0.0f)
                        newX = 0.0f;
                    else if (newX > Config.WindowWidth)
                        newX = Config.WindowWidth;
					p.SetPosition(new Vector2(newX, p.pos.Y));

					playerId = sd.TrackingId;
				}
			}
		}

		protected SkeletonPoint CalculateJointPosition(Joint joint)
		{
			var jointx = joint.Position.X;
			var jointy = joint.Position.Y;
			var jointz = joint.Position.Z;

			if (jointz < 1)
				jointz = 1;

			var jointnormx = jointx / (jointz * 1.1f);
			var jointnormy = -(jointy / jointz * 1.1f);
			var point = new SkeletonPoint();
			point.X = (jointnormx + 0.5f) * Config.WindowWidth;
			point.Y = (jointnormy + 0.5f) * Config.WindowHeight;
			return point;
		}
		//END KINECT METHODS
	}
}
