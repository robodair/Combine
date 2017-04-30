﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;
namespace Combine
{
	public class PlayLevel : RC_GameStateParent
	{
		const int rightSideItemsX = 720;
		const int rightSideStep = 25;
		const int piecesX = 550;

		GUI_Control GUI;
		Vector2[] RightSideItems = {new Vector2(rightSideItemsX, rightSideStep),
									new Vector2(rightSideItemsX, rightSideStep * 2),
									new Vector2(rightSideItemsX, rightSideStep * 3),
									new Vector2(rightSideItemsX, rightSideStep * 4),
									new Vector2(rightSideItemsX, rightSideStep * 9),
									new Vector2(rightSideItemsX, rightSideStep * 12)};
		Piece[] pieces;
		Vector2[] PiecePositions = {new Vector2(piecesX, 25),
									new Vector2(piecesX, 175),
									new Vector2(piecesX, 325)};
		int move;
		int score;
		Song song;

		Boolean dragging = false;
		MouseState beginDragging;
		Piece savedControl;

		Grid grid;

		public PlayLevel(RC_GameStateManager lm) :
			base(lm)
		{
		}

		public override void LoadContent()
		{
			song = Content.Load<Song>("music/puzzle-1-a");
			Texture2D homeButton = Content.Load<Texture2D>("textures/gui/homeButton");
			Texture2D pauseButton = Content.Load<Texture2D>("textures/gui/pauseButton");
			GUI = new GUI_Control();
			GUI.AddControl(new ButtonSI(homeButton, Color.Goldenrod, RightSideItems[4]).attachLeftMouseDownCallback(homeButtonClicked));
			GUI.AddControl(new ButtonSI(pauseButton, Color.Goldenrod, RightSideItems[5]).attachLeftMouseDownCallback(pauseButtonClicked));
			pieces = new Piece[3];
			grid = new Grid(8);
			base.LoadContent();
		}

		public override void EnterLevel(int fromLevelNum)
		{
			move = 0;
			score = 0;
			// Remove current pieces in queue
			foreach (Piece piece in pieces){GUI.RemoveControl(piece);}
			pieces = new Piece[3];
			grid = new Grid(8);
			MediaPlayer.Play(song);
			MediaPlayer.IsRepeating = true;
			dragging = false;
			Console.WriteLine("Enter Play");
			base.EnterLevel(fromLevelNum);
		}

		public override void ResumeLevel()
		{
			dragging = false;
			MediaPlayer.Resume();
			Console.WriteLine("Resume Play");
		}

		public override void SuspendLevel()
		{
			MediaPlayer.Pause();
			Console.WriteLine("Suspend Play");
		}

		public override void ExitLevel()
		{
			MediaPlayer.Stop();
			Console.WriteLine("Exit Play");
		}

		public override void Update(GameTime gameTime)
		{
			GUI.Update(gameTime);
			// Click UP Event
			if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
			{
				if (!dragging) // Don't send a click even if we were dragging
				{
					GUI.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
				}
				if (savedControl != null)
				{
					savedControl.dragging = false;
				}
				dragging = false; // cancel dragging
			}

			// Save state in case there's a drag
			if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
			{
				beginDragging = currentMouseState; // save the place where the mouse was when we began dragging
				GUI_Control control = GUI.getControlUnder(currentMouseState.X, currentMouseState.Y);
				if (control != null && control.GetType() == typeof(Piece)) // save the GUI item we're over
				{
					savedControl = (Piece)control;
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
				if (Math.Abs(beginDragging.X - currentMouseState.X) > 5 || Math.Abs(beginDragging.Y - currentMouseState.Y) > 5){
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
				// Spawn a new piece and make it slide in TODO: Use the level number ot work out what type
				pieces[0] = new Piece(piecesX, "square");
				pieces[0].SetTargetPosition(PiecePositions[0]);
				pieces[0].attachLeftMouseDownCallback(pieces[0].Rotate);
				GUI.AddControl(pieces[0]);
				Console.WriteLine("Piece Created!!");
			}
			foreach (Piece piece in pieces)
			{
				if (piece != null)
					piece.Update(gameTime);
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);

			grid.Draw(spriteBatch);

			spriteBatch.DrawString(font, "MOVE", RightSideItems[0], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, move.ToString(), RightSideItems[1], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "SCORE", RightSideItems[2], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, score.ToString(), RightSideItems[3], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);

			GUI.drawSubControls(spriteBatch); // This draws the pieces too
		}

		public void homeButtonClicked()
		{
			Console.WriteLine("Home Button Clicked");
			gameStateManager.setLevel(1);
		}

		public void pauseButtonClicked()
		{
			Console.WriteLine("Pause Button Clicked");
			//gameStateManager.setLevel(); //TODO: Some other pause state
		}
	}
}
