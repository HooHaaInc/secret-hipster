#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using TileEngine;

#endregion

namespace BountyHunter
{
	/// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
		#region Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Player player;
		enum GameState { TitleScreen, Playing, PlayerDead, GameOver };
		GameState gameState = GameState.TitleScreen;
		Vector2 gameOverPosition = new Vector2(350, 300);
		Vector2 livesPosition = new Vector2(600, 580);
		SpriteFont pericles8;

		Texture2D titleScreen;

		float deathTimer = 0.0f;
		float deathDelay = 5.0f;
		#endregion

		#region Constructor

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";	            
			//graphics.IsFullScreen = true;	
        }

		#endregion

		#region Essential methods

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
			//Camera.init (new Vector2 (5, 5), new Vector2 (myMap.MapWidth, myMap.MapHeight));ç
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			graphics.ApplyChanges ();
				
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
			base.LoadContent ();
            spriteBatch = new SpriteBatch(GraphicsDevice);
			TileMap.Initialize (
				Content.Load <Texture2D> (@"plat"));
			//TileMap.spriteFont = Content.Load <SpriteFont> (@"font");

			//pericles8 = Content.Load <Microsoft.Xna.Framework.Graphics.SpriteFont> (@"font");
			titleScreen = Content.Load <Texture2D> (@"TitleScreen");

			Camera.WorldRectangle = new Rectangle (0, 0, 160 * 16, 12 * 16);
			Camera.Position = Vector2.Zero;
			Camera.ViewPortWidth = 800;
			Camera.ViewPortHeight = 600;
			player = new Player (Content);
			LevelManager.Initialize (Content, player);

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState (PlayerIndex.One).IsKeyDown (Keys.Escape))
			{
				Exit();
			}
            // TODO: Add your update logic here		
			KeyboardState keystate = Keyboard.GetState ();
			//GamePadState gamepadState = GamePad.GetState (PlayerIndex.One);
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if(gameState == GameState.TitleScreen){
				if(keystate.IsKeyDown (Keys.Space) ){
					StartNewGame ();
					gameState = GameState.Playing;
				}
			}else if(gameState == GameState.Playing){
				player.Update (gameTime);
				LevelManager.Update (gameTime);
				if(player.Dead){
					if(player.LivesRemaining > 0){
						gameState = GameState.PlayerDead;
						deathTimer = 0.0f;
					}else{
						gameState = GameState.GameOver;
						deathTimer = 0.0f;
					}
				}
			}else if(gameState == GameState.PlayerDead){
				player.Update (gameTime);
				LevelManager.Update (gameTime);
				deathTimer += elapsed;
				if(deathTimer > deathDelay){
					player.WorldLocation = Vector2.Zero;
					LevelManager.ReloadLevel ();
					player.Revive ();
					gameState = GameState.Playing;
				}
			}else if(gameState == GameState.GameOver){
				deathTimer += elapsed;
				if (deathTimer > deathDelay)
					gameState = GameState.TitleScreen;
			}
			base.Update (gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear (Color.Black);
            //TODO: Add your drawing code here
			spriteBatch.Begin (
				SpriteSortMode.BackToFront,
				BlendState.AlphaBlend);

			if (gameState == GameState.TitleScreen)
				spriteBatch.Draw (titleScreen, Vector2.Zero, Color.White);
			else{
				TileMap.Draw (spriteBatch);
				player.Draw (spriteBatch);
				LevelManager.Draw (spriteBatch);
				/*spriteBatch.DrawString (
					pericles8,
					"Lives: " + player.LivesRemaining,
					livesPosition,
					Color.White);*/
				//if (gameState == GameState.GameOver)					
					/*spriteBatch.DrawString (
						pericles8,
						"G A M E   O V E R !",
						gameOverPosition,
						Color.White);*/
			}
			spriteBatch.End ();
            base.Draw(gameTime);
        }

		#endregion

		#region Helper Methods
		private void StartNewGame(){
			player.Revive ();
			player.LivesRemaining = 3;
			player.WorldLocation = Vector2.Zero;
			LevelManager.LoadLevel (5);
		}
		#endregion
    }
}

