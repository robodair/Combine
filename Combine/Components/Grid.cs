using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using Microsoft.Xna.Framework;

namespace Combine
{
	public class Grid
	{
		static Texture2D squarePartTexture;
		Sprite3[,] grid;
		Color[,] colorGrid;
		Vector2 gridOffset = new Vector2(100, 100);

		public Grid(int size)
		{
			grid = new Sprite3[size, size];
			colorGrid = new Color[size, size];
			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int j = 0; j < grid.GetLength(1); j++)
				{
					Console.WriteLine(i + ", " + j);
					grid[i, j] = new Sprite3(true, squarePartTexture, i * (Piece.partSize + Piece.partSpacing) + gridOffset.X, j * (Piece.partSize + Piece.partSpacing) + gridOffset.Y);
					grid[i, j].setColor(Color.DimGray);
					grid[i, j].setWidthHeight(Piece.partSize, Piece.partSize);
					colorGrid[i, j] = Color.Transparent;
				}
			}
		}

		public static void LoadContent(ContentManager c)
		{
			squarePartTexture = c.Load<Texture2D>("textures/pieces/square");
		}

		public void Draw(SpriteBatch sb)
		{

			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int j = 0; j < grid.GetLength(1); j++)
				{
					grid[i, j].Draw(sb);
					//grid[i, j].drawInfo(sb, Color.AliceBlue, Color.Goldenrod);
				}
			}
		}

		public void Update(GameTime gameTime, Piece piece)
		{
			// Clear the display colours of all items that don't have an assigned color
			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int j = 0; j < grid.GetLength(1); j++)
				{
					if (colorGrid[i, j] == Color.Transparent)
					{
						grid[i, j].setColor(Color.DimGray);
					}
				}
			}

			// Highlight the squares under the given piece (if they don't have a color)
			if (piece != null)
			{
				foreach (Sprite3 item in grid)
				{
					// if the HS of any of the piece components is inside this sprite, it highlights
					foreach (Sprite3 part in piece.parts)
					{
						if (item.Contains((int)part.getPosX(), (int)part.getPosY()))
						{
							item.colour = Util.lighterOrDarker(part.colour, 0.8f);
						}
					}
				}
			}

			// Detect combinations that mean there has been a win, make a list of them, display animations and remove the pieces
		}
	}
}
