using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
					else
					{
						grid[i, j].setColor(colorGrid[i, j]);
					}
				}
			}

			// Highlight the squares under the given piece (if they don't have a color)
			if (piece != null)
			{
				for (int i = 0; i < grid.GetLength(0); i++)
				{
					for (int j = 0; j < grid.GetLength(1); j++)
					{
						// if the HS of any of the piece components is inside this sprite, it highlights
						foreach (Sprite3 part in piece.parts)
						{
							if (grid[i, j].Contains((int)part.getPosX(), (int)part.getPosY()) && colorGrid[i, j] == Color.Transparent)
							{
								grid[i, j].colour = Util.lighterOrDarker(part.colour, 0.8f);
							}
						}
					}
				}
			}

		}

		public int checkForFullSquares()
		{
			HashSet<int[]> squaresToClear = new HashSet<int[]>();
			int numMatches = 0;
			// check every square to see if the corresponding next 3 are the same color
			for (int i = 0; i < grid.GetLength(0) - 1; i++)
			{
				for (int j = 0; j < grid.GetLength(1) - 1; j++)
				{
					if (colorGrid[i, j] != Color.Transparent && // Transparent means no color
					    colorGrid[i, j] == colorGrid[i + 1, j] &&
						colorGrid[i, j] == colorGrid[i, j + 1] &&
						colorGrid[i, j] == colorGrid[i + 1, j + 1])
					{
						// add the squares to a set to be cleared of colour
						numMatches++;
						squaresToClear.Add(new int[] { i, j });
						squaresToClear.Add(new int[] { i + 1, j });
						squaresToClear.Add(new int[] { i, j + 1 });
						squaresToClear.Add(new int[] { i + 1, j + 1 });
					}
				}
			}

			foreach (int[] square in squaresToClear)
			{
				// reset the colours
				colorGrid[square[0], square[1]] = Color.Transparent;
				grid[square[0], square[1]].colour = Color.DimGray;
			}
			Console.WriteLine("Found " + numMatches + " matches");
			// TODO: Schedule animations
			return numMatches;
		}

		public bool CaptureDroppedObject(Piece savedControl)
		{
			// Collect a list of squares that match
			List<int[]> matchedSquares = new List<int[]>();
			if (savedControl != null)
			{
				for (int i = 0; i < grid.GetLength(0); i++)
				{
					for (int j = 0; j < grid.GetLength(1); j++)
					{
						// if the HS of any of the piece components is inside this sprite, it highlights
						foreach (Sprite3 part in savedControl.parts)
						{
							if (grid[i, j].Contains((int)part.getPosX(), (int)part.getPosY()) && colorGrid[i, j] == Color.Transparent)
							{
								matchedSquares.Add(new int[] { i, j });
							}
						}
					}
				}
			}
			// If all parts have a square, apply the colors and return true
			if (matchedSquares.Count == savedControl.parts.Length)
			{
				foreach (int[] position in matchedSquares)
				{
					grid[position[0], position[1]].colour = savedControl.partColor;
					colorGrid[position[0], position[1]] = savedControl.partColor;
				}
				return true;
			}
			return false;
		}
	}
}
