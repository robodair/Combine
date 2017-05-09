using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using RC_Framework;
namespace Combine
{
	public class GameOverLevel : RC_GameStateParent
	{
		Song song;

		public GameOverLevel(RC_GameStateManager lm) :
					base(lm)
		{
		}

		public override void LoadContent()
		{
			//buttonTexture = Content.Load<Texture2D>("textures/gui/buttonDefault");
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);
		}
	}
}
