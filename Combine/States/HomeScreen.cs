using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;

namespace Combine
{
	public class HomeScreen : RC_GameStateParent
	{
		Texture2D buttonTexture;
		GUI_Control HomeMenu;
		Song song;
		public HomeScreen(RC_GameStateManager lm) :
			base(lm)
		{
		}

		public override void LoadContent()
		{
			song = Content.Load<Song>("music/puzzle-1-b");
			buttonTexture = Content.Load<Texture2D>("textures/gui/buttonDefault");
			HomeMenu = new GUI_Control();
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(200, 100)).attachLeftMouseDownCallback(Level1ButtonClick));
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(200, 200)).attachLeftMouseDownCallback(Level2ButtonClick));
			HomeMenu.AddControl(new ButtonSI(buttonTexture, Color.Blue, new Vector2(200, 300)).attachLeftMouseDownCallback(Level2ButtonClick));

		}

		public override void EnterLevel(int fromLevelNum)
		{
			MediaPlayer.Play(song);
			MediaPlayer.IsRepeating = true;
			Console.WriteLine("Enter Home");
		}

		public override void ResumeLevel()
		{
			MediaPlayer.Resume();
			Console.WriteLine("Resume Home");
		}

		public override void SuspendLevel()
		{
			MediaPlayer.Pause();
			Console.WriteLine("Suspend Home");
		}

		public override void ExitLevel()
		{
			MediaPlayer.Stop();
			Console.WriteLine("Exit Home");
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
			gameStateManager.setLevel(2);
		}

		public void Level2ButtonClick()
		{
			Console.WriteLine("Level 2 Button Clicked");
			gameStateManager.setLevel(3);
		}

		public void Level3ButtonClick()
		{
			Console.WriteLine("Level 3 Button Clicked");
			gameStateManager.setLevel(4);
		}
	}
}
