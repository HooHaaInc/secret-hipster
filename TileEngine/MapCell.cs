using System;

namespace TileEngine
{
	public class MapCell
	{
		public int TileID{ get; set; }

		public MapCell (int tileID)
		{
			TileID = tileID;
		}
	}
}

