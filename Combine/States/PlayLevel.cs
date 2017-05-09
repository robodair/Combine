using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;
using System.Timers;

namespace Combine
{
	public class PlayLevel<GridType, PieceType> : RC_GameStateParent
	{
		bool debug = false;

		const int rightSideItemsX = 720;
		const int rightSideStep = 25;
		const int piecesX = 550;

		int GridSize;

		GUI_Control GUI;
		Vector2[] RightSideItems = {new Vector2(rightSideItemsX, rightSideStep),
									new Vector2(rightSideItemsX, rightSideStep * 2),
									new Vector2(rightSideItemsX, rightSideStep * 3),
									new Vector2(rightSideItemsX, rightSideStep * 4),
									new Vector2(rightSideItemsX, rightSideStep * 9),
									new Vector2(rightSideItemsX, rightSideStep * 12)};
		ShapePiece[] pieces;
		Vector2[] PiecePositions = {new Vector2(piecesX, 25),
									new Vector2(piecesX, 175),
									new Vector2(piecesX, 325)};
		int move;
		int score;
		Song song;
		SoundEffectInstance chime;

		Boolean dragging = false;
		MouseState beginDragging;
		ShapePiece savedControl;

		ShapeGrid<PieceType> grid;
		public int MATCH_SCORE = 30;
		public String Type { get; private set; }

		Vector2 helpTextPos = new Vector2(20, 20);

		bool checkForNoMoreMoves;

		public PlayLevel(RC_GameStateManager lm, string type, int gridSize) :
			base(lm)
		{
			Type = type;
			GridSize = gridSize;
		}

		public override void LoadContent()
		{
			song = Content.Load<Song>("music/puzzle-1-a");
			chime = Content.Load<SoundEffect>("sound/success").CreateInstance();
			chime.Volume = 0.5f;
			Texture2D homeButton = Content.Load<Texture2D>("textures/gui/homeButton");
			Texture2D pauseButton = Content.Load<Texture2D>("textures/gui/pauseButton");
			GUI = new GUI_Control();
			GUI.AddControl(new ButtonSI(homeButton, Color.Goldenrod, RightSideItems[4]).attachLeftMouseDownCallback(homeButtonClicked));
			GUI.AddControl(new ButtonSI(pauseButton, Color.Goldenrod, RightSideItems[5]).attachLeftMouseDownCallback(pauseButtonClicked));
			base.LoadContent();
		}

		public override void EnterLevel(int fromLevelNum)
		{
			move = 0;
			score = 0;
			// Remove current pieces in queue
			if (pieces != null)
				foreach (ShapePiece piece in pieces) { GUI.RemoveControl((GUI_Control)piece); }
			pieces = (new ShapePiece[3]);
			grid = (ShapeGrid<PieceType>)Activator.CreateInstance(typeof(GridType), new object[] { GridSize, 150, 150, 30, true, null });
			MediaPlayer.Play(song);
			MediaPlayer.IsRepeating = true;
			dragging = false;
			checkForNoMoreMoves = false;
			base.EnterLevel(fromLevelNum);
		}

		public override void ResumeLevel()
		{
			dragging = false;
			MediaPlayer.Play(song);
			MediaPlayer.IsRepeating = true;
		}

		public override void Update(GameTime gameTime)
		{
			// Enable/disable debug drawing
			if (keyState.IsKeyDown(Keys.B) && prevKeyState.IsKeyUp(Keys.B))
			{
				debug = !debug;
			}
			((ShapeGrid<PieceType>)grid).UpdateAsLevelGrid(gameTime, (PieceType)savedControl);
			// Click UP Event
			if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
			{
				if (!dragging) // Don't send a click even if we were dragging
				{
					GUI.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
				}
				if (dragging && savedControl != null)
				{
					savedControl.endDrag();
					if (((ShapeGrid<PieceType>)grid).CaptureDroppedPiece((PieceType)savedControl))
					{
						// remove the object from the piece array
						pieces[Array.IndexOf(pieces, savedControl)] = null;
						GUI.RemoveControl((GUI_Control)savedControl);
						move++;
						int points = ((ShapeGrid<PieceType>)grid).RemoveCompletedShapes() * MATCH_SCORE;
						score += points;
						savedControl = null;
						checkForNoMoreMoves = true;
						if (points > 0)
						{
							chime.Play();
						}
					}
				}
				dragging = false; // cancel dragging
			}
			GUI.Update(gameTime);

			// Save state in case there's a drag
			if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
			{
				beginDragging = currentMouseState; // save the place where the mouse was when we began dragging
				GUI_Control control = GUI.getControlUnder(currentMouseState.X, currentMouseState.Y);
				if (control != null && control.GetType() == typeof(PieceType)) // save the GUI item we're over
				{
					savedControl = (ShapePiece)control;
				}
				else
				{
					savedControl = null;
				}
			}

			// Actually move the dragged piece with the mouse
			if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
			{
				// if we've moved more than a few pixels we are dragging, send a drag event to the gui item
				if (Math.Abs(beginDragging.X - currentMouseState.X) > 5 || Math.Abs(beginDragging.Y - currentMouseState.Y) > 5)
				{
					if (savedControl != null)
					{
						dragging = true;
						savedControl.beginDrag(currentMouseState.X, currentMouseState.Y); // the piece ignores subsequent calls to this
						savedControl.setDragPos(currentMouseState.X, currentMouseState.Y);
					}
				}
			}

			// if the piece slots are vacant, move pieces and create new ones to fill the blank slot
			if (pieces[2] == null && pieces[1] != null && pieces[1].inPosition)
			{
				// Move pieces[1] down to pieces [2]
				pieces[2] = pieces[1];
				pieces[1] = null;
				pieces[2].SetTargetPosition(PiecePositions[2]);
			}
			if (pieces[1] == null && pieces[0] != null && pieces[0].inPosition)
			{
				// Move pieces[0] down to pieces [1]
				pieces[1] = pieces[0];
				pieces[0] = null;
				pieces[1].SetTargetPosition(PiecePositions[1]);
			}
			if (pieces[0] == null)
			{
				// Spawn a new piece and make it slide in
				pieces[0] = (ShapePiece)Activator.CreateInstance(typeof(PieceType), piecesX, 30);
				pieces[0].SetTargetPosition(PiecePositions[0]);
				((GUI_Control)pieces[0]).attachLeftMouseDownCallback(pieces[0].RotateRight);
				GUI.AddControl(((GUI_Control)pieces[0]));
			}
			foreach (ShapePiece piece in pieces)
			{
				if (piece != null)
					piece.Update(gameTime);
			}


			// The final thing we do is check if any of the pieces can fit into the grid
			if (checkForNoMoreMoves == true && grid.hasNoMovesLeft(pieces))
			{
				checkForNoMoreMoves = false;
				Console.WriteLine("NO MOVES LEFT");
				// Set a timer to move to the game over screen
				Timer GameOverScreenTimer = new Timer(4000);
				GameOverScreenTimer.Elapsed += delegate
								{
									gameStateManager.setLevel(Game1.GAME_OVER_LEVEL);
								};
				GameOverScreenTimer.AutoReset = false;
				GameOverScreenTimer.Enabled = true;

				// Show the "No More Moves popup"
				Timer GameOverOverlayTimer = new Timer(300);
				GameOverOverlayTimer.Elapsed += delegate
								{
									gameStateManager.setLevel(Game1.GAME_OVER_OVERLAY);
								};
				GameOverOverlayTimer.AutoReset = false;
				GameOverOverlayTimer.Enabled = true;
				Game1.lastScore = score;
				Game1.lastLevel = gameStateManager.getCurrentLevelNum();
			}
			else
			{
				checkForNoMoreMoves = false;
			}

			if (keyState.IsKeyDown(Keys.P) && prevKeyState.IsKeyUp(Keys.P))
			{
				gameStateManager.pushLevel(Game1.PAUSE_LEVEL);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);

			grid.Draw(spriteBatch, debug);

			spriteBatch.DrawString(font, "MOVE", RightSideItems[0], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, move.ToString(), RightSideItems[1], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "SCORE", RightSideItems[2], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, score.ToString(), RightSideItems[3], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "(P) Pause" + Environment.NewLine + "(F1) Help" + Environment.NewLine + "(B) BB drawing", helpTextPos, Color.Lavender, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			GUI.drawSubControls(spriteBatch, debug); // This draws the pieces too
		}

		public void homeButtonClicked()
		{
			gameStateManager.setLevel(Game1.HOME_SCREEN);
		}

		public void pauseButtonClicked()
		{
			gameStateManager.pushLevel(Game1.PAUSE_LEVEL);
		}
	}
}
