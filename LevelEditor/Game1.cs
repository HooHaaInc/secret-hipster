#region Using Statements
using System;
using Gtk;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using TileEngine;

#endregion

namespace LevelEditor
{
	/// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


		public class GtkThread{
			private Game1 game;
			private Thread theThread;

			private MapEditor toolsWindow;
			private MapEditor ToolsWindow{
				get{ return toolsWindow; }
			}


			public bool IsInitialized { get; private set;}
			private bool wantsToStop = false;

			public GtkThread(Game1 game){
				this.game = game;
				IsInitialized = false;
			}

			public void Start(){
				theThread = new Thread (new ThreadStart (RunLoop));
				theThread.Start ();
			}

			public void Stop(){
				wantsToStop = true;
			}

			public void RunLoop(){
				while(true){
					if(!wantsToStop){
						if (!IsInitialized) {
							Gtk.Application.Init ();
							toolsWindow = new MapEditor(game);
							toolsWindow.Show();
							toolsWindow.Initialize ();
							IsInitialized = true;
						}

						Gtk.Application.RunIteration (false);
					}else{
						if(IsInitialized){
							Gtk.Application.Quit ();
							IsInitialized = false;
						}
						break;
					}
					//Thread.Sleep (20);
				}
			}
		}

		GtkThread gtkThread;
						
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			/*graphics.PreparingDeviceSettings += 
				new EventHandler<PreparingDeviceSettingsEventArgs> (
				graphics_PreparingDeviceSettings);
*/

			gtkThread = new GtkThread (this);
			gtkThread.Start ();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			base.Initialize ();


			while (!gtkThread.IsInitialized)
				;

			//gtkThread.ToolsWindow.SetProperties(...);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				Exit();
			}
            // TODO: Add your update logic here			
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           	graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
		
            //TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }

		protected override void OnExiting(object sender, EventArgs args){
			gtkThread.Stop ();
			while (gtkThread.IsInitialized)
				;
			base.OnExiting (sender, args);
		}
    }
}

