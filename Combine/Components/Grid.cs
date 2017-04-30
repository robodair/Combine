﻿using System;
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
		Vector2 gridOffset = new Vector2(100, 100);

		public Grid(int size)
		{
			grid = new Sprite3[size, size];
			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int j = 0; j < grid.GetLength(1); j++)
				{
					Console.WriteLine(i + ", " + j);
					grid[i, j] = new Sprite3(true, squarePartTexture, i * (Piece.partSize + Piece.partSpacing) + gridOffset.X, j * (Piece.partSize + Piece.partSpacing) + gridOffset.Y);
					grid[i, j].setColor(Color.DimGray);
					grid[i, j].setWidthHeight(Piece.partSize, Piece.partSize);
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
					Console.WriteLine("drawing: " + i + ", " + j);
					grid[i, j].Draw(sb);
					//grid[i, j].drawInfo(sb, Color.AliceBlue, Color.Goldenrod);
				}
			}
			foreach (Sprite3 item in grid)
			{
				if (item != null)
				{

				}
			}
		}
	}
}
