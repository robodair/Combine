using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
namespace Combine
{
	public class PlayLevel : RC_GameStateParent
	{
		const int rightSideItemsX = 720;
		const int rightSideStep = 25;

		GUI_Control GUI;
		Vector2[] RightSideItems = {new Vector2(rightSideItemsX, rightSideStep),
									new Vector2(rightSideItemsX, rightSideStep * 2),
									new Vector2(rightSideItemsX, rightSideStep * 3),
									new Vector2(rightSideItemsX, rightSideStep * 4),
									new Vector2(rightSideItemsX, rightSideStep * 9),
									new Vector2(rightSideItemsX, rightSideStep * 12)};
		int move;
		int score;
		public PlayLevel(RC_GameStateManager lm) :
			base(lm)
		{
		}

		public override void LoadContent()
		{
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
			base.EnterLevel(fromLevelNum);
		}

		public override void Update(GameTime gameTime)
		{
			GUI.Update(gameTime);
			if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
			{
				GUI.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
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
