using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TileEngine;

namespace BountyHunter
{
	public class Player : GameObject
	{
		private Vector2 fallSpeed = new Vector2(0, 20);
		private float moveScale = 180.0f;
		private bool dead = false;

		public bool Dead {
			get { return dead; }
		}

		#region Constructor
		public Player (ContentManager content)
		{
			animations.Add ("idle",
			                new AnimationStrip (content.Load<Texture2D> (@"idle"),
			                                   50, "idle"));
			animations ["idle"].LoopAnimation = true;
			animations.Add ("run",
			                new AnimationStrip (content.Load<Texture2D> (@"run"),
			                                    50, "run"));
			animations ["run"].LoopAnimation = true;
			animations.Add ("jump",
			                new AnimationStrip (content.Load<Texture2D> (@"jump"),
			                                   50, "jump"));
			animations ["jump"].LoopAnimation = false;
			animations ["jump"].FrameLength = 0.08f;
			animations ["jump"].NextAnimation = "idle";
			animations.Add ("die",
			                new AnimationStrip (content.Load<Texture2D> (@"die"),
			                                   100, "die"));
			animations ["die"].LoopAnimation = false;
			frameWidth = 48;
			frameHeight = 48;
			collisionRectangle = new Rectangle (16, 7, 21, 43);
			drawDepth = 0.825f;
			codeBasedBlocks = false;
			PlayAnimation ("idle");
		}
		#endregion
	}
}

