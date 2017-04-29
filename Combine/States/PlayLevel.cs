using System;
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
		const int piecesX = 620;

		GUI_Control GUI;
		Vector2[] RightSideItems = {new Vector2(rightSideItemsX, rightSideStep),
									new Vector2(rightSideItemsX, rightSideStep * 2),
									new Vector2(rightSideItemsX, rightSideStep * 3),
									new Vector2(rightSideItemsX, rightSideStep * 4),
									new Vector2(rightSideItemsX, rightSideStep * 9),
									new Vector2(rightSideItemsX, rightSideStep * 12)};
		Piece[] pieces;
		Vector2[] PiecePositions = {new Vector2(piecesX, graphicsManager.PreferredBackBufferHeight / 4),
									new Vector2(piecesX, (graphicsManager.PreferredBackBufferHeight / 4) * 2),
									new Vector2(piecesX, (graphicsManager.PreferredBackBufferHeight / 4) * 3)};
		int move;
		int score;
		Song song;

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
			base.LoadContent();
		}

		public override void EnterLevel(int fromLevelNum)
		{
			move = 0;
			score = 0;
			pieces = new Piece[3];
			MediaPlayer.Play(song);
			MediaPlayer.IsRepeating = true;
			Console.WriteLine("Enter Play");
			base.EnterLevel(fromLevelNum);
		}

		public override void ResumeLevel()
		{
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
			if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
			{
				GUI.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
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
				pieces[0] = new Piece(piecesX);
				pieces[0].SetTargetPosition(PiecePositions[0]);
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

			spriteBatch.DrawString(font, "MOVE", RightSideItems[0], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, move.ToString(), RightSideItems[1], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "SCORE", RightSideItems[2], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, score.ToString(), RightSideItems[3], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);

			GUI.drawSubControls(spriteBatch);

			foreach (Piece piece in pieces)
			{
				if (piece != null)
					piece.Draw(spriteBatch);
			}
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
