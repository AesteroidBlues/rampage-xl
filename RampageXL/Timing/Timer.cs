using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RampageXL.Timing
{
    class Timer
    {
        int framesElapsed = 0;
        int timerLength;
        bool timeUp;
        public bool pauseTimer { get; set; }

        public Timer(int timerLengthInFrames)
        {
            timeUp = true;
            timerLength = timerLengthInFrames;
            pauseTimer = true;
        }


        public void Update()
        {
            if (!pauseTimer)
            {
                if (++framesElapsed >= timerLength)
                {
                    timeUp = true;
                    pauseTimer = true;
                }
            }
        }

        public bool timeIsUp()
        {
            return timeUp;
        }

        public void startTimer()
        {
            timeUp = false;
            pauseTimer = false;
            framesElapsed = 0;
        }
    }
}
