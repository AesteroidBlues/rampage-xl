using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using GestureFramework;

using WindowsInput;

namespace RampageXL
{
	class XLK
	{
		private static KinectSensorChooser _chooser;
		private static Bitmap _bitmap;

		private static GestureMap _gestureMap;
		private static Dictionary<int, GestureMapState> _gestureMaps;
		private const string GestureFileName = "gestures.xml";

		public int PlayerId;

		public static void Init()
		{
			// First Load the XML File that contains the application configuration
			_gestureMap = new GestureMap();
			_gestureMap.LoadGesturesFromXml(GestureFileName);

			_chooser = new KinectSensorChooser();
			_chooser.KinectChanged += ChooserSensorChanged;
			_chooser.Start();

			// Instantiate the in memory representation of the gesture state for each player
			_gestureMaps = new Dictionary<int, GestureMapState>();
		}

		private static void ChooserSensorChanged(object sender, KinectChangedEventArgs e) 
		{
			//As of 6/12/2013 @ 10:30AM, the program never enters here

			var old = e.OldSensor;
			StopKinect(old);

			var newsensor = e.NewSensor;
			if (newsensor == null)
			{
				return;
			}

			newsensor.SkeletonStream.Enable();
			//newsensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
			//newsensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
			newsensor.AllFramesReady += SensorAllFramesReady;


			try
			{
				newsensor.Start();
			}
			catch (System.IO.IOException)
			{
				//maybe another app is using Kinect
				_chooser.TryResolveConflict();
			}
		}

		private static void StopKinect(KinectSensor sensor)
		{
			if (sensor != null)
			{
				if (sensor.IsRunning)
				{
					sensor.Stop();
					sensor.AudioSource.Stop();
				}
			}
		}

		void SensorAllFramesReady(object sender, AllFramesReadyEventArgs e)
		{
			if (_gestureMap.MessagesWaiting)
			{
				_gestureMap.MessagesWaiting = false;
			}

			//SensorDepthFrameReady(e);
			SensorSkeletonFrameReady(e);
			//video.Image = _bitmap;
		}

		void SensorSkeletonFrameReady(AllFramesReadyEventArgs e)
		{
			using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
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

					var keycode = _gestureMaps[sd.TrackingId].Evaluate(sd, false, _bitmap.Width, _bitmap.Height);
					GetWaitingMessages(_gestureMaps);

					if (keycode != VirtualKeyCode.NONAME)
					{
						//Another method
						//InputSimulator.SimulateKeyPress(keycode);

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

					p.SetPosition(new Vector2(sd.Joints[JointType.HipCenter].Position.X, p.pos.Y));

					// This break prevents multiple player data from being confused during evaluation.
					// If one were going to dis-allow multiple players, this trackingId would facilitate
					// that feat.
					PlayerId = sd.TrackingId;

					if (_bitmap != null)
						_bitmap = AddSkeletonToDepthBitmap(sd, _bitmap, true);
				}
			}
		}

		protected SkeletonPoint CalculateJointPosition(Bitmap bitmap, Joint joint)
		{
			var jointx = joint.Position.X;
			var jointy = joint.Position.Y;
			var jointz = joint.Position.Z;

			if (jointz < 1)
				jointz = 1;

			var jointnormx = jointx / (jointz * 1.1f);
			var jointnormy = -(jointy / jointz * 1.1f);
			var point = new SkeletonPoint();
			point.X = (jointnormx + 0.5f) * bitmap.Width;
			point.Y = (jointnormy + 0.5f) * bitmap.Height;
			return point;
		}
	}
}
