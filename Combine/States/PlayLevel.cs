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
			base.EnterLevel(fromLevelNum);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

	}
}
