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
		Vector2[] RightSideItems = {new Vector2(rightSideItemsX, rightSideStep),
									new Vector2(rightSideItemsX, rightSideStep * 2),
									new Vector2(rightSideItemsX, rightSideStep * 3),
									new Vector2(rightSideItemsX, rightSideStep * 4),
									new Vector2(rightSideItemsX, rightSideStep * 5)};
		int move;
		int score;
		public PlayLevel(RC_GameStateManager lm) :
			base(lm)
		{
		}

		public override void LoadContent()
		{
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
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);

			spriteBatch.DrawString(font, "MOVE", RightSideItems[0], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, move.ToString(), RightSideItems[1], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "SCORE", RightSideItems[2], Color.Gray, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, score.ToString(), RightSideItems[3], Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
		}

	}
}
