using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RC_Framework;

namespace Combine
{
	public class HomeScreen : RC_GameStateParent
	{
		public HomeScreen(RC_GameStateManager lm) :
			base(lm)
		{
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);
		}
	}
}
