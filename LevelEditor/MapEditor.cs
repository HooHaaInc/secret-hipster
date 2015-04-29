using System;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TileEngine;

namespace LevelEditor
{
	public partial class MapEditor : Gtk.Window
	{
		private Game1 game;

		public MapEditor (Game1 game) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.game = game;
			this.Deletable = false;
			this.Build ();
		}

		public void Initialize(){
			LoadImageList ();
		}

		protected void exitToolStripMenuItem_Click (object sender, EventArgs e)
		{
			game.Exit ();
		}

		private void LoadImageList(){
			string filePath = @"Content/platformTiles.png";
			Bitmap tileSheet = new Bitmap (filePath);
			//int tileCount = 0;
			for(int y =0; y<tileSheet.Height/TileMap.TileHeight; ++y){
				for (int x=0; x<tileSheet.Width/TileMap.TileWidth; ++x) {
					Bitmap newbitmap = tileSheet.Clone (
						new System.Drawing.Rectangle (
						x * TileMap.TileWidth,
						y * TileMap.TileHeight,
						TileMap.TileWidth,
						TileMap.TileHeight),
						System.Drawing.Imaging.PixelFormat.DontCare);
					Gtk.Image image = new Gtk.Image (ImageToPixbuf (newbitmap));
					//vbox1.Add (image);
					//vbox1. += image;
				}
			}
		}

		private Gdk.Pixbuf ImageToPixbuf(System.Drawing.Image image){
			using(System.IO.MemoryStream stream = new System.IO.MemoryStream()){
				image.Save (stream, System.Drawing.Imaging.ImageFormat.Bmp);
				stream.Position = 0;
				return new Gdk.Pixbuf (stream);
			}
		}
	}
	
}

