using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;
namespace Combine
{
	public class GameOverOverlay : RC_GameStateParent
	{
		Rectangle pauseBackground;
		Vector2 textPos;
		Song gameOverSong;

		public GameOverOverlay(RC_GameStateManager lm) :
							base(lm)
		{
			pauseBackground = new Rectangle(graphicsManager.PreferredBackBufferWidth / 2 - 100,
										   graphicsManager.PreferredBackBufferHeight / 2 - 100,
											200, 100);
			textPos = new Vector2(graphicsManager.PreferredBackBufferWidth / 2 - 80,
									   graphicsManager.PreferredBackBufferHeight / 2 - 60);
		}

		public override void EnterLevel(int fromLevelNum)
		{
			// Play the game over song
			MediaPlayer.Play(gameOverSong);
			base.EnterLevel(fromLevelNum);
		}

		public override void LoadContent()
		{
			gameOverSong = Content.Load<Song>("music/doodle");
			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Draw(UtilTexSI.texWhite, pauseBackground, Color.Lavender);
			spriteBatch.DrawString(font, "Game Over", textPos, Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
		}

	}
}
