using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RampageXL.Mugic;

namespace RampageXL.Shape
{
	abstract class Drawable
	{
		protected int objectId;
		private bool wasCreated = false;

		public abstract void Draw();
		public abstract MugicPacket GetUpdate();

		protected void InitPacket(MugicCommand s, MugicPacket p)
		{
			if (!wasCreated)
			{
				objectId = MugicObjectManager.NextId;
				p.Command(s, objectId);
				wasCreated = true;
			}
			else
			{
				p.Command(MugicCommand.Update, objectId);
			}
		}
	}
}
