using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TileEngine;

namespace BountyHunter
{
	public class Gemstone : GameObject
	{
		#region Constructor
		public Gemstone (ContentManager Content, int cellX, int cellY)
		{	
			worldLocation.X = TileMap.TileWidth * cellX;
			worldLocation.Y = TileMap.TileHeight * cellY;
			frameWidth = TileMap.TileWidth;
			frameHeight = TileMap.TileHeight;
			animations.Add ("idle",
			                new AnimationStrip (Content.Load<Texture2D> (@"gem"),
			                                   16, "idle"));
			animations ["idle"].LoopAnimation = true;
			animations ["idle"].FrameLength = 0.15f;
			PlayAnimation ("idle");
			drawDepth = 0.875f;
			collisionRectangle = new Rectangle (0, 0, 16, 16);
			enabled = true;
		}
		#endregion
	}
}

