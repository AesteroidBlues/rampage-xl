using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RampageXL.Shape
{
	class Bounds
	{
		private int _w;
		public int width {
			set { this._w = value; }
			get {return this._w;}
		}
		private int _h;
		public int height {
			set { this._h = value; }
			get { return this._h; }
		}

		public int halfWidth
		{
			get { return (int)this._w / 2; }
		}

		public int halfHeight
		{
			get { return (int)this._h / 2; }
		}

		public Bounds(int w, int h)
		{
			_w = w;
			_h = h;
		}

		public override string ToString()
		{
			return "{width:" + _w + ", height:" + _h + "}";
		}
	}
}
