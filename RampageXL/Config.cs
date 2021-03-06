﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

namespace RampageXL
{
	struct Config
	{
		//////////////////////////////
		/// WINDOW PROPERTIES
		//////////////////////////////
		public static int WindowWidth           = 1280;
		public static int WindowHeight          = 360;
		public static int WallScalar            = 1360; // Value to scale dimensions up by for the Hiperwall
		public static GraphicsMode GraphicsMode = GraphicsMode.Default;
		public static String Title              = "Rampage XL";

		//////////////////////////////
		/// NETWORK PROPERTIES
		//////////////////////////////
		public static String CalVRIP			= "67.58.41.6";
		public static int CalVRPort				= 19997;
		public static int Max_Packet_Size		= 1024;
	};
}
