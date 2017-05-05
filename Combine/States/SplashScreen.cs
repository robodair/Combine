using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RC_Framework;
namespace Combine
{
	public class SplashScreen : RC_GameStateParent
	{
		int counter = 0;
		Vector2 titlePos;
		Vector2 subtitlePos;
		Vector2 creatorPos;

		public SplashScreen(RC_GameStateManager lm) :
			base(lm)
		{
			titlePos = new Vector2(graphicsManager.PreferredBackBufferWidth / 3,
								   graphicsManager.PreferredBackBufferHeight / 4);
			subtitlePos = new Vector2(graphicsManager.PreferredBackBufferWidth / 3,
									  graphicsManager.PreferredBackBufferHeight / 2);
			creatorPos = new Vector2(graphicsManager.PreferredBackBufferWidth / 3,
									  graphicsManager.PreferredBackBufferHeight / 4 * 3);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.MediumSlateBlue);

			spriteBatch.DrawString(font, "Combine", titlePos, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Shape Tessellation", subtitlePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "2017 - Alisdair Robertson", creatorPos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			counter++;
			if (counter == 30)
			{
				gameStateManager.popLevel();
			}
		}
	}
}
