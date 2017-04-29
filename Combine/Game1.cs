using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RC_Framework;

namespace Combine
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		RC_GameStateManager StateManager;

		const int SPLASH_SCREEN = 0;
		const int HOME_SCREEN = 1;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			StateManager = new RC_GameStateManager(GraphicsDevice, graphics, spriteBatch, Content);

			StateManager.AddLevel(SPLASH_SCREEN, new SplashScreen(StateManager));
			StateManager.AddLevel(HOME_SCREEN, new HomeScreen(StateManager));

			StateManager.setLevel(HOME_SCREEN);
			StateManager.pushLevel(SPLASH_SCREEN);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			RC_GameStateParent.font = Content.Load<SpriteFont>("font/Arcon");
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			RC_GameStateParent.getKeyboardAndMouse();

			if (RC_GameStateParent.keyState.IsKeyDown(Keys.Escape))
                Exit();

			StateManager.getCurrentLevel().Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			StateManager.getCurrentLevel().Draw(gameTime);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
