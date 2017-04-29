using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;

namespace Combine
{
	public class HomeScreen : RC_GameStateParent
	{
		Texture2D buttonTexture;
		GUI_Control HomeMenu;
		public HomeScreen(RC_GameStateManager lm) :
			base(lm)
		{
		}

		public override void LoadContent()
		{
			buttonTexture = Content.Load<Texture2D>("textures/gui/buttonDefault");
		}

		public override void EnterLevel(int fromLevelNum)
		{
			HomeMenu = new GUI_Control();
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(200, 100)).attachLeftMouseDownCallback(Level1ButtonClick));
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(200, 200)).attachLeftMouseDownCallback(Level2ButtonClick));
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(200, 300)).attachLeftMouseDownCallback(Level2ButtonClick));
		}

		public override void Update(GameTime gameTime)
		{
			HomeMenu.Update(gameTime);
			if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
			{
				HomeMenu.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);

			HomeMenu.drawSubControls(spriteBatch);
		}

		public void Level1ButtonClick()
		{
			Console.WriteLine("Level 1 Button Clicked");
		}

		public void Level2ButtonClick()
		{
			Console.WriteLine("Level 2 Button Clicked");
		}

		public void Level3ButtonClick()
		{
			Console.WriteLine("Level 3 Button Clicked");
		}
	}
}
