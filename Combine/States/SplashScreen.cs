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
		public SplashScreen(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm) :
			base(g, s, c, lm)
		{
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.MediumSlateBlue);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			counter++;
			if (counter == 240)
			{
				gameStateManager.popLevel();
			}
		}
	}
}
