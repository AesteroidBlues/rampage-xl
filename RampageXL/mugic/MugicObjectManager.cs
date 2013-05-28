using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RampageXL.Shape;

namespace RampageXL.Mugic
{
	class MugicObjectManager
	{
		private static int _nextId = 0;
		public static int NextId
		{
			get {
				int id = _nextId;
				_nextId += 1;
				return id;
			}
		}

		private static List<Drawable> shapes = new List<Drawable>();
		public static void Register(Drawable d)
		{
			shapes.Add(d);
		}

		public static void SendShapes()
		{
			foreach (Drawable d in shapes)
			{
				MugicPacket p = d.GetUpdate();
				if (p != null)
				{
					MugicConnection.EnueuePacket(p);
				}
			}
			MugicConnection.SendUpdate();
		}
	}
}
