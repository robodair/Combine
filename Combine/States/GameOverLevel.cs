using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
namespace Combine
{
	public class GameOverLevel : RC_GameStateParent
	{
		Vector2 titlePos;
		Vector2 subtitlePos;
		GUI_Control GUI;
		public GameOverLevel(RC_GameStateManager lm) :
					base(lm)
		{
			titlePos = new Vector2(graphicsManager.PreferredBackBufferWidth / 3,
								   graphicsManager.PreferredBackBufferHeight / 4);
			subtitlePos = new Vector2(graphicsManager.PreferredBackBufferWidth / 3,
									  graphicsManager.PreferredBackBufferHeight / 2 - 50);
		}

		public override void LoadContent()
		{
			Texture2D homeButton = Content.Load<Texture2D>("textures/gui/homeButton");
			Texture2D replayButton = Content.Load<Texture2D>("textures/gui/replayButton");
			GUI = new GUI_Control();
			GUI.AddControl(new ButtonSI(homeButton, Color.Goldenrod, new Vector2(300, 300)).attachLeftMouseDownCallback(homeButtonClicked));
			GUI.AddControl(new ButtonSI(replayButton, Color.Goldenrod, new Vector2(400, 300)).attachLeftMouseDownCallback(replayButtonClicked));
			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
			{
				GUI.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
			}

			GUI.Update(gameTime);
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);
			spriteBatch.DrawString(font, "Nice Work!", titlePos, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, $"Score: {Game1.lastScore}", subtitlePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
			GUI.drawSubControls(spriteBatch, false);
		}

		public void homeButtonClicked()
		{
			gameStateManager.setLevel(Game1.HOME_SCREEN);
		}

		public void replayButtonClicked()
		{
			gameStateManager.setLevel(Game1.lastLevel);
		}
	}
}
