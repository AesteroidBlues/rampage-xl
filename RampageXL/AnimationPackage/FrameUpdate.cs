using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RampageXL.Shape;

namespace RampageXL.AnimationPackage
{
    class FrameUpdate
    {
        public Rectangle draw {get; set;}
        public Rectangle clear {get; set;}
        
        public FrameUpdate(Rectangle draw, Rectangle clear)
        {
            this.draw = draw;
            this.clear = clear;
        }
    }
}
