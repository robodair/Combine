using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
namespace Combine
{
	public class PauseLevel : RC_GameStateParent
	{
		Vector2 pauseTextPos;
		Vector2 resumeTextPos;

		public PauseLevel(RC_GameStateManager lm) :
					base(lm)
		{
			pauseTextPos = new Vector2(graphicsManager.PreferredBackBufferWidth / 2 - 150,
									   graphicsManager.PreferredBackBufferHeight / 2 - 60);
			resumeTextPos = pauseTextPos;
			resumeTextPos.Y += 80;
		}

		public override void Update(GameTime gameTime)
		{
			if (keyState.IsKeyDown(Keys.P) && prevKeyState.IsKeyUp(Keys.P))
			{
				gameStateManager.popLevel();
			}
			base.Update(gameTime);
		}


		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.Lavender);
			spriteBatch.DrawString(font, "Paused", pauseTextPos, Color.DarkSlateGray);
			spriteBatch.DrawString(font, "(P) To Resume" + Environment.NewLine + "(F1) for help", resumeTextPos,
			                       Color.DarkSlateGray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
		}
	}
}
