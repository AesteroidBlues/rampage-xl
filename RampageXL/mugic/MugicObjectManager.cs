using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
