using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

using RampageXL.Shape;

namespace RampageXL.AnimationPackage
{
	/// <summary>
	/// An enumeration of animation modes.
	/// LOOP - loops through frames, mode specified in LoopMode
	/// GOTO - run animation until specified frame.
	///        If above specified frame, goes to the specified frame and stops.
	/// PAUSE - hold on the current frame
	/// </summary>
	enum AnimationMode
	{ 
		LOOP, 
		GOTO,
		PAUSE,
		PLAYONCE
	}
	/// <summary>
	/// Enumeration of loop modes
	/// DIRECTION - follow current direction
	/// PINGPONG - forwards then reverse
	/// RANDOM - randomly chooses a frame
	/// </summary>
	enum LoopMode
	{
		DIRECTION,
		PINGPONG,
		RANDOM
	}

	public delegate void AnimationDoneCallback();

	class Animation
	{
		public List<Rectangle> frames { get; set; }
		public AnimationMode animationMode { get; set; }
		public LoopMode loopMode { get; set; }
		private int direction {get; set;}
		public int framerate { get; set; }
		public int currentFrame { get; set; }
		public int goToFrame { get; set; }

		int loopFrom;
		int loopTo;

		private FrameUpdate fu;

		private Random random = new Random();
		private int currentTick = 0;
		AnimationDoneCallback callback;

		public Animation(int framerate, AnimationMode animMode)
		{
			this.frames = new List<Rectangle>();
			this.framerate = framerate;
			this.animationMode = animMode;
			this.loopMode = LoopMode.DIRECTION;
			SetDirectionForwards();
			loopFrom = 0;
		}

		public Animation(int framerate, AnimationMode animMode, LoopMode loopMode)
		{
			this.frames = new List<Rectangle>();
			this.framerate = framerate;
			this.animationMode = animMode;
			this.loopMode = loopMode;
			SetDirectionForwards();
			loopFrom = 0;
		}

		public Animation(List<Rectangle> frames, int framerate)
		{
			this.frames = frames;
			this.framerate = framerate;
			this.animationMode = AnimationMode.LOOP;
			this.loopMode = LoopMode.DIRECTION;
			SetDirectionForwards();
			loopFrom = 0;
			loopTo = frames.Count - 1;
		}

		public Animation(List<Rectangle> frames, int framerate, AnimationMode animMode)
		{
			this.frames = frames;
			this.framerate = framerate;
			this.animationMode = animMode;
			this.loopMode = LoopMode.DIRECTION;
			SetDirectionForwards();
			loopFrom = 0;
			loopTo = frames.Count - 1;
		}
		
		public Animation(List<Rectangle> frames, int framerate, AnimationMode animMode, int goToFrame)
		{
			this.frames = frames;
			this.framerate = framerate;
			this.animationMode = animMode;
			this.loopMode = LoopMode.DIRECTION;
			SetDirectionForwards();
			loopFrom = 0;
			loopTo = frames.Count - 1;
			this.goToFrame = goToFrame;
		}

		public Animation(List<Rectangle> frames, int framerate, AnimationMode animMode, LoopMode loopMode)
		{
			this.frames = frames;
			this.framerate = framerate;
			this.animationMode = animMode;
			this.loopMode = loopMode;
			loopFrom = 0;
			loopTo = frames.Count - 1;
			SetDirectionForwards();
		}

		public Animation(List<Rectangle> frames, int framerate, AnimationMode animMode, LoopMode loopMode, int loopFrom, int loopTo)
		{
			this.frames = frames;
			this.framerate = framerate;
			this.animationMode = animMode;
			this.loopMode = loopMode;
			this.loopFrom = loopFrom;
			this.loopTo = loopTo;
			SetDirectionForwards();
		}

		public void AddFrame(Image newFrame)
		{
			Rectangle newRect = new Rectangle(0, 0, newFrame.width, newFrame.height);
			newRect.setTexture(newFrame.name);
			frames.Add(newRect);
			loopTo = frames.Count - 1;
		}

		public void AddFrame(Image newFrame, int w, int h)
		{
			Rectangle newRect = new Rectangle(0, 0, w, h);
			newRect.setTexture(newFrame.name);
			frames.Add(newRect);
			loopTo = frames.Count - 1;
		}

		public void SetPlayOnceDoneCallback(AnimationDoneCallback c)
		{
			this.callback = c;
		}

		/// <summary>
		/// Updates the animation
		/// </summary>
		/// <returns>The frame to be drawn</returns>
		public void Update()
		{
			int oldFrame = currentFrame;
			if (animationMode != AnimationMode.PAUSE)
			{
				if (++currentTick >= framerate)
				{
					currentTick = 0;
					currentFrame += direction;
					if (animationMode == AnimationMode.GOTO)
					{
						if (direction > 0 && currentFrame >= goToFrame)
						{
							currentFrame = goToFrame;
							animationMode = AnimationMode.PAUSE;
						}
						if (direction > 0 && currentFrame <= goToFrame)
						{
							currentFrame = goToFrame;
							animationMode = AnimationMode.PAUSE;
						}
					}
					if (animationMode == AnimationMode.PLAYONCE)
					{
						currentFrame = 0;
						callback.Invoke();
					}
					if (animationMode == AnimationMode.LOOP)
					{
						if (loopMode == LoopMode.DIRECTION)
						{
							if (direction > 0 && currentFrame > loopTo)
							{
								currentFrame = loopFrom;
							}
							if (direction < 0 && currentFrame < loopTo)
							{
								currentFrame = loopFrom;
							}
						}
						if (loopMode == LoopMode.PINGPONG)
						{
							if (direction > 0 && currentFrame > loopTo)
							{
								currentFrame = loopTo;
								direction = -1;
							}
							if (direction < 0 && currentFrame < loopFrom)
							{
								currentFrame = loopFrom;
								direction = 1;
							}
						}
						if (loopMode == LoopMode.RANDOM)
						{
							currentFrame = random.Next(frames.Count - 1);
						}
					}
				}
			}
			Rectangle currentFrameRect = frames[currentFrame];
			Rectangle oldFrameRect = frames[oldFrame];
			if (oldFrame == currentFrame)
			{
				oldFrameRect = null;
			}
			else
			{
				currentFrameRect.MarkDirty();
				currentFrameRect.Unhide();
				oldFrameRect.MarkDirty();
				oldFrameRect.Hide();
			}

			fu = new FrameUpdate(currentFrameRect, oldFrameRect);
		}

		public void Draw(Vector2 pos)
		{
			if (fu.draw != null)
			{
				Rectangle current = fu.draw;

				current.setPosition(pos);
				current.Draw();
			}
		}

		public void SetLoopThroughAll()
		{
			loopFrom = 0;
			loopTo = frames.Count - 1;
		}

		public void SetLoopBounds(int from, int to)
		{
			loopFrom = from;
			loopTo = to;
		}

		public void SetDirectionForwards()
		{
			direction = 1;
		}

		public void SetDirectionReverse()
		{
			direction = -1;
		}
		public void resetAnimation()
		{
			currentFrame = 0;
		}

		public void setGoToMode(int goToFrame)
		{
			animationMode = AnimationMode.GOTO;
			goToFrame = goToFrame;
		}
	}
}
