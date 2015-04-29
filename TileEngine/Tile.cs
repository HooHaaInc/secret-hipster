using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
	public static class Tile
	{
		static public Texture2D TileSetTexture;

		public static Rectangle GetSourcerRectangle(int tileIndex){
			return new Rectangle (tileIndex * 17+1, 1, 16, 16);
		}
	}
}

