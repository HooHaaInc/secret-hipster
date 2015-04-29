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
		private int score = 0;
		private int livesRemaining = 3;
		private Rectangle hitBox = new Rectangle(16, 36, 21, 12);
		private Texture2D rect;

		public int LivesRemaining {
			get { return livesRemaining; }
			set { livesRemaining = value; }
		}

		public int Score{
			get { return score; }
			set { score = value; }
		}

		public bool Dead {
			get { return dead; }
		}

		public Rectangle HitBox{
			get { return new Rectangle(
				(int)worldLocation.X + hitBox.X,
				(int)worldLocation.Y + hitBox.Y,
				hitBox.Width,
				hitBox.Height); 
			}
		}

		public override Rectangle CollisionRectangle{
			get{ return new Rectangle (
					(int)worldLocation.X + collisionRectangle.X,
					(int)worldLocation.Y + collisionRectangle.Y,
					collisionRectangle.Width,
					collisionRectangle.Height + 12
				);}
		}

		public Rectangle VulnerableRectangle{
			get{
				return base.CollisionRectangle;
			}
		}

		#region Constructor
		public Player (ContentManager content)
		{
			rect = content.Load < Texture2D >(@"rect");

			animations.Add ("idle",
			                new AnimationStrip (content.Load<Texture2D> (@"idle"),
			                                   50, "idle"));
			animations ["idle"].LoopAnimation = true;
			animations ["idle"].FrameLength = 0.2f;
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
			                new AnimationStrip (content.Load<Texture2D> (@"dead"),
			                                   100, "die"));
			animations ["die"].LoopAnimation = false;
			frameWidth = 48;
			frameHeight = 48;
			collisionRectangle = new Rectangle (16, 7, 21, 28);
			drawDepth = 0.825f;
			codeBasedBlocks = false;
			PlayAnimation ("idle");
		}
		#endregion

		#region Public Methods
		public override void Update(GameTime gameTime){
			if (!Dead) {
				string newAnimation = "idle";
				velocity = new Vector2 (0, velocity.Y);
				//GamePadState gamePad = GamePad.GetState (PlayerIndex.One);
				KeyboardState keyState = Keyboard.GetState ();
				if (keyState.IsKeyDown (Keys.A) || keyState.IsKeyDown (Keys.Left)) {
					flipped = true;
					newAnimation = "run";
					velocity = new Vector2 (-moveScale, velocity.Y);

				}
				if (keyState.IsKeyDown (Keys.D) || keyState.IsKeyDown (Keys.Right)) {
					flipped = false;
					newAnimation = "run";
					velocity = new Vector2 (moveScale, velocity.Y);

				}if(keyState.IsKeyDown (Keys.Space) && onGround){
					Jump ();
					newAnimation = "jump";
				}if(keyState.IsKeyDown (Keys.Up) || keyState.IsKeyDown(Keys.W)){
					checkLevelTransition ();
				}
				if (currentAnimation == "jump")
					newAnimation = "jump";
				if (newAnimation != currentAnimation)
					PlayAnimation (newAnimation);
			}
			velocity += fallSpeed;
			if (velocity.Y > 600)
				velocity.Y = 600;
			repositionCamera ();
			base.Update (gameTime);
		}

		public override void Draw (SpriteBatch spriteBatch)
		{
			base.Draw (spriteBatch);
			//Debug c:
			/*spriteBatch.Draw (
				rect,
				VulnerableRectangle,
				Color.Green);
			spriteBatch.Draw (
				rect,
				HitBox,
				Color.Red);*/
		}

		public void Jump(){
			velocity.Y = -500;
		}

		public void Kill(){
			PlayAnimation ("die");
			--livesRemaining;
			velocity.X = 0;
			dead = true;
		}

		public void Revive(){
			PlayAnimation ("idle");
			dead = false;
		}

		#endregion

		#region Helper Methods
		private void repositionCamera(){
			int screenLocX = (int)Camera.WorldToScreen (worldLocation).X;
			if (screenLocX > 500)
				Camera.Move (new Vector2 (screenLocX - 500, 0));
			else if(screenLocX < 200)
				Camera.Move (new Vector2 (screenLocX - 200, 0));
		}

		private void checkLevelTransition(){
			Vector2 centerCell = TileMap.GetCellAtPixel (WorldBottomCenter);
			if(TileMap.CellCodeValue (centerCell).StartsWith ("T_")){
				string[] code = TileMap.CellCodeValue (centerCell).Split ('_');
				if (code.Length != 4)
					return;
				LevelManager.LoadLevel (int.Parse (code [1]));
				worldLocation = new Vector2 (
					int.Parse (code [2]) * TileMap.TileWidth,
					int.Parse (code [3]) * TileMap.TileHeight);
				LevelManager.RespawnLocation = worldLocation;
				velocity = Vector2.Zero;
			}
		}
		#endregion
	}
}

