using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;
namespace Combine
{
	public class HelpScreen : RC_GameStateParent
	{
		Vector2 textPos;
		Song gameOverSong;

		String HelpText = $@"Help for Combine:
================={Environment.NewLine}
Combine is a sweet and simple game.{Environment.NewLine}
Use your mouse to drag and drop pieces on to
the grid to make different shapes!{Environment.NewLine}
Making shapes gets you more points,
see what you can get your highscore to for the different grid types.{ Environment.NewLine }
F1 to exit help
================={Environment.NewLine}";

		public HelpScreen(RC_GameStateManager lm) :
							base(lm)
		{
			textPos = new Vector2(80, 60);
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
			graphicsDevice.Clear(Color.Lavender);
			spriteBatch.DrawString(font, HelpText, textPos, Color.DarkSlateGray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
		}

	}
}
