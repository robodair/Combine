using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;

namespace Combine
{
	public class HomeScreen : RC_GameStateParent
	{
		Texture2D buttonTexture;
		GUI_Control HomeMenu;
		Song song;
		Vector2 helpTextPos = new Vector2(20, 20);
		Vector2 squareScoresStart = new Vector2(400, 100);
		Vector2 triangleScoresStart = new Vector2(500, 100);
		Vector2 pentagonScoresStart = new Vector2(600, 100);
		Vector2 scoresOffset = new Vector2(0, 30);
		public HomeScreen(RC_GameStateManager lm) :
			base(lm)
		{
		}

		public override void LoadContent()
		{
			song = Content.Load<Song>("music/puzzle-1-b");
			buttonTexture = Content.Load<Texture2D>("textures/gui/buttonDefault");
			HomeMenu = new GUI_Control();
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(100, 100)
											 ).setText(" Square Level", font, 0.35f, Color.MediumPurple
											).attachLeftMouseDownCallback(Level1ButtonClick));
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(100, 200)
											 ).setText("Triangle Level", font, 0.35f, Color.MediumPurple
											).attachLeftMouseDownCallback(Level2ButtonClick));
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(100, 300)
											 ).setText("Pentagon Level", font, 0.35f, Color.MediumPurple
											).attachLeftMouseDownCallback(Level3ButtonClick));

		}

		public override void EnterLevel(int fromLevelNum)
		{
			MediaPlayer.Play(song);
			MediaPlayer.IsRepeating = true;
		}

		public override void ResumeLevel()
		{
			MediaPlayer.Play(song);
			MediaPlayer.IsRepeating = true;
		}


		public override void Update(GameTime gameTime)
		{
			HomeMenu.Update(gameTime);
			if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
			{
				HomeMenu.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);
			spriteBatch.DrawString(font, "(F1) Help", helpTextPos, Color.Lavender, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			HomeMenu.drawSubControls(spriteBatch, false);

			// Draw the high score table directly
			int scorePosition = 0;
			foreach (int score in Game1.squareLevelScores)
			{
				spriteBatch.DrawString(font, $"{score}", squareScoresStart + scoresOffset * scorePosition, Color.Lavender, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
				scorePosition++;
			}

			scorePosition = 0;
			foreach (int score in Game1.triangleLevelScores)
			{
				spriteBatch.DrawString(font, $"{score}", triangleScoresStart + scoresOffset * scorePosition, Color.Lavender, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
				scorePosition++;
			}

			scorePosition = 0;
			foreach (int score in Game1.pentagonLevelScores)
			{
				spriteBatch.DrawString(font, $"{score}", pentagonScoresStart + scoresOffset * scorePosition, Color.Lavender, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
				scorePosition++;
			}
		}

		public void Level1ButtonClick()
		{
			gameStateManager.setLevel(2);
		}

		public void Level2ButtonClick()
		{
			gameStateManager.setLevel(3);
		}

		public void Level3ButtonClick()
		{
			gameStateManager.setLevel(4);
		}
	}
}
