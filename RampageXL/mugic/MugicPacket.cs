using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RampageXL.Mugic
{
	public enum MugicCommand
	{
		Rectangle,
		Line,
		Circle,
		Update
	}

	public enum MugicParam
	{
		X,
		Y,
		Z,
		Width,
		Height
	}

	class MugicPacket
	{
		MugicCommand command;
		int objectId;
		IList<Param> parameters = new List<Param>();

		public void Command(MugicCommand c, int oid)
		{
			command = c;
			objectId = oid;
		}

		public void Parameter(MugicParam p, Object v)
		{
			parameters.Add(new Param(p, v));
		}

		public override string ToString()
		{
			string ret = "";
			
			ret += Command2String(command);
			foreach (Param p in parameters)
			{
				ret += p.ToString();
			}

			ret += ";";

			return ret;
		}

		private class Param
		{
			private MugicParam p;
			private Object v;

			public Param(MugicParam par, Object val) 
			{
				p = par;
				v = val;
			}

			public override string ToString()
			{
				return Param2String(p) + "=" + v.ToString();
			}
		}

		// I wish I was smart enough to not need this function :(
		protected static String Command2String(MugicCommand c)
		{
			switch (c)
			{
				case MugicCommand.Rectangle: return "rectangle ";
				case MugicCommand.Circle   : return "circle ";
				case MugicCommand.Line     : return "line ";
				case MugicCommand.Update   : return "update ";
				default                    : return "ERROR ";
			}
		}

		// Or this one :(
		protected static String Param2String(MugicParam p)
		{
			switch (p)
			{
				case MugicParam.X     : return "x";
				case MugicParam.Y     : return "y";
				case MugicParam.Z     : return "z";
				case MugicParam.Width : return "width";
				case MugicParam.Height: return "height";
				default               : return "ERROR";
			}
		}
	}
}
