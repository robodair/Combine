using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
namespace Combine
{
	public class GameOverLevel : RC_GameStateParent
	{
		Vector2 titlePos;
		Vector2 subtitlePos;
		GUI_Control GUI;
		RC_RenderableList particleEffects; // List of particle emitters
		Random Rand;
		public GameOverLevel(RC_GameStateManager lm) :
					base(lm)
		{
			titlePos = new Vector2(graphicsManager.PreferredBackBufferWidth / 3,
								   graphicsManager.PreferredBackBufferHeight / 4);
			subtitlePos = new Vector2(graphicsManager.PreferredBackBufferWidth / 3,
									  graphicsManager.PreferredBackBufferHeight / 2 - 20);
			particleEffects = new RC_RenderableList();
		}

		public override void LoadContent()
		{
			Rand = new Random();
			Texture2D homeButton = Content.Load<Texture2D>("textures/gui/homeButton");
			Texture2D replayButton = Content.Load<Texture2D>("textures/gui/replayButton");
			GUI = new GUI_Control();
			GUI.AddControl(new ButtonSI(homeButton, Color.Goldenrod, new Vector2(300, 300)).attachLeftMouseDownCallback(homeButtonClicked));
			GUI.AddControl(new ButtonSI(replayButton, Color.Goldenrod, new Vector2(400, 300)).attachLeftMouseDownCallback(replayButtonClicked));

			// Add A Square Particle emitter
			ParticleSystem squareParticles = new ParticleSystem(
				new Vector2(100, -100), 500, 3000, Rand.Next());
			squareParticles.setMandatory1(SquarePiece.Texture, new Vector2(32, 32), new Vector2(1, 1), Color.DarkOrchid, Color.RoyalBlue);
			squareParticles.setMandatory2(3000, 3, 00, 30, 10);
			squareParticles.setMandatory3(2000, new Rectangle(0, -100, 800, 700));
			squareParticles.setMandatory4(new Vector2(0, 0), new Vector2(1, 2), new Vector2(-4, 4));
			squareParticles.activate();
			particleEffects.addReuse(squareParticles);

			// Add A Triangle Particle emitter
			ParticleSystem triangleParticles = new ParticleSystem(
				new Vector2(graphicsManager.PreferredBackBufferWidth / 3, -100), 500, 3000, Rand.Next());
			triangleParticles.setMandatory1(TrianglePiece.Texture, new Vector2(32, 32), new Vector2(1, 1), Color.Chartreuse, Color.RoyalBlue);
			triangleParticles.setMandatory2(3000, 3, 00, 30, 300);
			triangleParticles.setMandatory3(2000, new Rectangle(0, -100, 800, 700));
			triangleParticles.setMandatory4(new Vector2(0, 0), new Vector2(1, 2), new Vector2(-8, 4));
			triangleParticles.activate();
			particleEffects.addReuse(triangleParticles);

			// Add A Pentagon Particle emitter
			ParticleSystem pentagonParticles = new ParticleSystem(
				new Vector2((graphicsManager.PreferredBackBufferWidth / 3) * 2, -100), 500, 3000, Rand.Next());
			pentagonParticles.setMandatory1(PentagonPiece.Texture, new Vector2(32, 32), new Vector2(1, 1), Color.Crimson, Color.RoyalBlue);
			pentagonParticles.setMandatory2(3000, 3, 00, 30, 300);
			pentagonParticles.setMandatory3(2000, new Rectangle(0, -100, 800, 700));
			pentagonParticles.setMandatory4(new Vector2(0, 0), new Vector2(1, 2), new Vector2(-8, 4));
			pentagonParticles.activate();
			particleEffects.addReuse(pentagonParticles);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
			{
				GUI.MouseDownEventLeft(currentMouseState.X, currentMouseState.Y);
			}
			particleEffects.Update(gameTime);
			GUI.Update(gameTime);
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			graphicsDevice.Clear(Color.DarkSlateBlue);
			particleEffects.Draw(spriteBatch);
			spriteBatch.DrawString(font, "Nice Work!", titlePos, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, $"Score: {Game1.lastScore}", subtitlePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
			GUI.drawSubControls(spriteBatch, false);
		}

		public void homeButtonClicked()
		{
			gameStateManager.setLevel(Game1.HOME_SCREEN);
		}

		public void replayButtonClicked()
		{
			gameStateManager.setLevel(Game1.lastLevel);
		}
	}
}
