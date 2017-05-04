using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using System.Collections.Generic;

namespace Combine
{
	public class SquareGrid : ShapeGrid<SquarePiece>
	{
		Color DefaultColor;
		Sprite3[,] Sprites;         // The array of sprites
		int Size;                   // The size of the grid
		Random Rand;                // Random used for decision making
		int PartSpacing;            // Default part spacing (between the outer edges of parts)
		int PartSize;               // Width/Height of parts
		int OffsetX;
		int OffsetY;
		RC_RenderableList particleEffects; // List of particle emitters

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Combine.SquareGrid"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="offset_x">Offset x.</param>
		/// <param name="offset_y">Offset y.</param>
		/// <param name="partSize">Part size.</param>
		/// <param name="partSpacing">Part spacing.</param>
		/// <param name="spriteActive">Whether sprites are visible or invisible when created or now</param>
		/// <param name="defaultColor">Part spacing.</param>
		public SquareGrid(int size, int offset_x, int offset_y, int partSize, int partSpacing, bool spriteActive = true, Color? defaultColor = null)
		{
			DefaultColor = defaultColor ?? Color.DimGray;
			PartSize = partSize;
			PartSpacing = partSpacing;
			OffsetX = offset_x;
			OffsetY = offset_y;
			Size = size;
			Rand = new Random();
			Sprites = new Sprite3[Size, Size];
			particleEffects = new RC_RenderableList();
			forAllItems(delegate (int x, int y, Sprite3 sprite)
			{
				// Initialise a sprite
				Sprites[x, y] = new Sprite3(true, SquarePiece.Texture,
											x * (partSize + partSpacing) + offset_x,
											y * (partSize + partSpacing) + offset_y);
				Sprites[x, y].setActive(spriteActive);
				Sprites[x, y].setColor(DefaultColor);
				Sprites[x, y].setWidthHeight(partSize, partSize);
				Sprites[x, y].setBBandHSFractionOfTexCentered(1);
			});
		}

		/// <summary>
		/// Loads the content.
		/// </summary>
		/// <param name="c">Content Manager.</param>
		public static void LoadContent(ContentManager c)
		{
			SquarePiece.LoadContent(c);
		}

		/// <summary>
		/// For all the items in the grid, call the method given, with the coordinates of the item
		/// </summary>
		/// <param name="action">Call this method.</param>
		public void forAllItems(Action<int, int, Sprite3> action)
		{
			for (int x = 0; x < Sprites.GetLength(0); x++)
			{
				for (int y = 0; y < Sprites.GetLength(1); y++)
				{
					action(y, x, Sprites[y, x]);
				}
			}
		}

		/// <summary>
		/// Rotates the Grid right One step (90 Degrees)
		/// </summary>
		public void RotateRight()
		{
			Sprite3[,] newSprites = new Sprite3[Size, Size];

			// Transpose grid into the new array
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// In a transpose element at row y column x in the original is placed at row x column y of the transpose
				// https://chortle.ccsu.edu/VectorLessons/vmch13/vmch13_14.html
				newSprites[y, x] = s;
			});
			Sprites = newSprites;

			// Reverse rows
			// http://stackoverflow.com/questions/21023348/fast-algorithm-in-java-to-reverse-an-array
			for (int y = 0; y < Size; y++)
			{ // row
				for (int x = 0; x < (Size / 2); x++) // half the columns
				{
					Sprite3 temp = Sprites[x, y];
					Sprites[x, y] = Sprites[Size - x - 1, y];
					Sprites[Size - x - 1, y] = temp;
				}
			}

			// For a square grid, we also want to shift the piece left and up to the top every rotation
			int shiftX = Sprites.GetLength(0);
			int shiftY = Sprites.GetLength(1);
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				if (s.getActive())
				{
					if (x < shiftX) shiftX = x;
					if (y < shiftY) shiftY = y;
				}
			});

			// Shift the array
			Sprite3[,] alignedSprites = new Sprite3[Size, Size];
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				x -= shiftX;
				y -= shiftY;
				if (x < 0) x = Sprites.GetLength(0) + x;
				if (y < 0) y = Sprites.GetLength(1) + y;
				alignedSprites[x, y] = s;
			});
			Sprites = alignedSprites;

			// Update sprite positions
			// Transpose grid into the new array
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// new sprite positions (and rotations, if this wasn't a square grid!)
				s.setPos(x * (PartSize + PartSpacing) + OffsetX, y * (PartSize + PartSpacing) + OffsetY);
			});

		}

		/// <summary>
		/// Draw the specified sb and debug.
		/// </summary>
		/// <param name="sb">SpriteBatch.</param>
		/// <param name="debug">If set to <c>true</c> draw debug.</param>
		public void Draw(SpriteBatch sb, bool debug)
		{
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				s.Draw(sb);
				if (debug)
				{
					s.drawInfo(sb, Color.AliceBlue, Color.Goldenrod);
				}
			});
			particleEffects.Draw(sb);
		}

		/// <summary>
		/// Updates as a level grid.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="piece">Piece.</param>
		public void UpdateAsLevelGrid(GameTime gameTime, SquarePiece piece)
		{
			particleEffects.Update(gameTime);
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// Clear the display colours of all items that don't have an assigned color
				if (s.getActive() && !s.varBool0) // varBool0 indicates a dropped color - do not clear in this case
				{
					s.setColor(DefaultColor);
				}

				// Highlight the squares under the given piece (if they don't have a color)
				if (piece != null)
				{
					piece.PieceGrid.forAllItems(delegate (int _x, int _y, Sprite3 part)
					{
						if (part.getActive()
							&& s.Contains((int)part.getPosX(), (int)part.getPosY())
							&& !s.varBool0) // varBool0 means the space is already filled
						{
							s.setColor(Util.lighterOrDarker(part.colour, 0.8f));
						}
					});
				}
			});
		}

		/// <summary>
		/// Checks for completed squares and replaces them with particle effects
		/// </summary>
		/// <returns>The number completed squares.</returns>
		public int RemoveCompletedShapes()
		{
			HashSet<Sprite3> squaresToClear = new HashSet<Sprite3>();
			int numMatches = 0;

			// for each square check color of: the next door one, the one below, and the one diagonally below
			// and add them to the list plus create particle effects for them
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				if (x < Size - 1 && y < Size - 1
					&& s.getActive()
					&& s.varBool0   // Sprite must be marked as dropped
					&& s.getColor() == Sprites[x + 1, y].getColor()
					&& s.getColor() == Sprites[x + 1, y + 1].getColor()
					&& s.getColor() == Sprites[x, y + 1].getColor()
				)
				{
					numMatches++;
					squaresToClear.Add(s);
					squaresToClear.Add(Sprites[x + 1, y]);
					squaresToClear.Add(Sprites[x + 1, y + 1]);
					squaresToClear.Add(Sprites[x, y + 1]);
				}
			});

			// Spawn particle systems and reset colors TODO: Make the effect nicer
			foreach (Sprite3 s in squaresToClear)
			{
				// reset the colours
				s.setColor(DefaultColor);
				s.varBool0 = false; // varBool0 is whether the color is perm
									// Create a particle system for the square
									// create a new particle system for each of the squares
				ParticleSystem p = new ParticleSystem(s.getPos(), 100, 90, Rand.Next());
				p.setMandatory1(UtilTexSI.texWhite, new Vector2(8, 8), new Vector2(1, 1), Color.White, Color.White);
				p.setMandatory2(20, 20, 0, 0, 0);
				p.setMandatory3(30, new Rectangle(0, 0, 800, 600));
				p.setMandatory4(new Vector2(0, 0), new Vector2(1, 1), new Vector2(10, 10));
				p.activate();
				particleEffects.addReuse(p);
			}
			return numMatches;
		}

		/// <summary>
		/// Captures the dropped object.
		/// </summary>
		/// <returns><c>true</c>, if dropped object was captured, <c>false</c> otherwise.</returns>
		/// <param name="piece">Piece</param>
		public bool CaptureDroppedPiece(SquarePiece piece) // TODO change to SquarePiece
		{
			// Collect a list of squares that match
			List<Sprite3> matchedSquares = new List<Sprite3>();
			if (piece != null)
			{
				forAllItems(delegate (int x, int y, Sprite3 s)
				{
					// Highlight the squares under the given piece (if they don't have a color)
					if (piece != null)
					{
						piece.PieceGrid.forAllItems(delegate (int _x, int _y, Sprite3 part)
						{
							if (s.getActive() && part.getActive()
								&& !s.varBool0
								&& s.Contains((int)part.getPosX(), (int)part.getPosY()))
							{
								matchedSquares.Add(s);
							}
						});
					}
				});
			}
			Console.WriteLine("Dropped: " + matchedSquares.Count);
			// If all parts have a square, apply the colors and return true
			if (matchedSquares.Count == piece.NumParts)
			{
				foreach (Sprite3 s in matchedSquares)
				{
					s.setColor(piece.PartColor);
					s.varBool0 = true; // set the flag that this part has been filled
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Sets the position, ignores pervious offset.
		/// </summary>
		/// <param name="position">Position.</param>
		public void setPos(Vector2 position)
		{
			OffsetX = (int)position.X;
			OffsetY = (int)position.Y;
			// Update sprite positions
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// new sprite positions (and rotations, if this wasn't a square grid!)
				s.setPos(x * (PartSize + PartSpacing) + OffsetX, y * (PartSize + PartSpacing) + OffsetY);
			});
		}

		/// <summary>
		/// Gets the sprite at (x, y).
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Sprite3 getSprite(int x, int y)
		{
			return Sprites[x, y];
		}
	}
}
