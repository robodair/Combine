using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RC_Framework;

namespace Combine
{
	public class HomeScreen : RC_GameStateParent
	{
		public HomeScreen(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm) :
			base(g, s, c, lm)
		{
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);
		}
	}
}
