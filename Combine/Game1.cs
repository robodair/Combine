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
		public const int HOME_SCREEN = 1;
		const int SQUARES_LEVEL = 2;
		const int TRIANGLES_LEVEL = 3;
		const int PENTAGONS_LEVEL = 4;
		public const int GAME_OVER_LEVEL = 5;
		public const int GAME_OVER_OVERLAY = 6;
		public const int HELP_LEVEL = 7;
		public const int PAUSE_LEVEL = 8;

		bool helpLevelActive = false;

		public static int lastScore = 0;
		public static int lastLevel = 0;
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
			LineBatch.init(GraphicsDevice);
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			StateManager = new RC_GameStateManager(GraphicsDevice, graphics, spriteBatch, Content);

			StateManager.AddLevel(SPLASH_SCREEN, new SplashScreen(StateManager));
			StateManager.AddLevel(HOME_SCREEN, new HomeScreen(StateManager));
			StateManager.AddLevel(SQUARES_LEVEL, new PlayLevel<SquareGrid, SquarePiece>(StateManager, "square", 6));
			StateManager.AddLevel(TRIANGLES_LEVEL, new PlayLevel<TriangleGrid, TrianglePiece>(StateManager, "triangle", 6));
			StateManager.AddLevel(PENTAGONS_LEVEL, new PlayLevel<PentagonGrid, PentagonPiece>(StateManager, "pentagon", 3));
			StateManager.AddLevel(GAME_OVER_LEVEL, new GameOverLevel(StateManager));
			StateManager.AddLevel(GAME_OVER_OVERLAY, new GameOverOverlay(StateManager));
			StateManager.AddLevel(HELP_LEVEL, new HelpScreen(StateManager));
			StateManager.AddLevel(PAUSE_LEVEL, new PauseLevel(StateManager));

			this.IsMouseVisible = true;
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			UtilTexSI.initTextures(GraphicsDevice);
			SpriteFont font = Content.Load<SpriteFont>("font/Arcon");
			RC_GameStateParent.font = font;
			SquareGrid.LoadContent(Content);
			TriangleGrid.LoadContent(Content);
			PentagonGrid.LoadContent(Content);
			Grid.LoadContent(Content);


			for (int i = 0; i < StateManager.numLevels; i++)
			{
				StateManager.getLevel(i).LoadContent();
			}

			GUI_Globals.initGUIGlobals(GraphicsDevice, font, font, Color.White);

			StateManager.setLevel(SPLASH_SCREEN);
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

			if (RC_GameStateParent.keyState.IsKeyDown(Keys.F1) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.F1))
			{
				if (!helpLevelActive)
				{
					StateManager.pushLevel(Game1.HELP_LEVEL);
					helpLevelActive = true;
				}
				else
				{
					StateManager.popLevel();
					helpLevelActive = false;
				}
			}

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
