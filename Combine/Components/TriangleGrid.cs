﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using System.Collections.Generic;

namespace Combine
{
	public class TriangleGrid : ShapeGrid<TrianglePiece>
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
		/// Initializes a new instance of the <see cref="T:Combine.TriangleGrid"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="offset_x">Offset x.</param>
		/// <param name="offset_y">Offset y.</param>
		/// <param name="partSize">Part size.</param>
		/// <param name="spriteActive">Whether sprites are visible or invisible when created or now</param>
		/// <param name="defaultColor">Part spacing.</param>
		public TriangleGrid(int size, int offset_x, int offset_y, int partSize, bool spriteActive = true, Color? defaultColor = null)
		{
			DefaultColor = defaultColor ?? Color.DimGray;
			PartSize = partSize;
			PartSpacing = partSize / 7;
			OffsetX = offset_x;
			OffsetY = offset_y;
			Size = size;
			Rand = new Random();
			Sprites = new Sprite3[Size * 2, Size];
			particleEffects = new RC_RenderableList();
			forAllItems(delegate (int x, int y, Sprite3 sprite)
			{
				Vector2 spritePos = getPosForSprite(x, y);

				// Initialise a sprite
				Sprites[x, y] = new Sprite3(true, TrianglePiece.Texture, spritePos.X, spritePos.Y);
				Sprites[x, y].setActive(spriteActive);
				Sprites[x, y].setColor(DefaultColor);
				Sprites[x, y].setWidthHeight(partSize, partSize);
				Sprites[x, y].setBBandHSFractionOfTexCentered(1);
				Sprites[x, y].setBB(PartSize * 0.25f, 0, PartSize * 0.6f, PartSize);
				if (!validTriangle(x, y))
				{
					Sprites[x, y].setActive(false);
				}

			});

			Align(); // this will ensure all the triangles point the right way
		}

		Vector2 getPosForSprite(int x, int y)
		{
			int rowOffset = (Sprites.GetLength(1) - y) * (PartSize / 2 + PartSpacing);
			return new Vector2(x * ((PartSize / 2) + PartSpacing) + OffsetX + rowOffset,
							   y * (PartSize + PartSpacing) + OffsetY);
		}

		/// <summary>
		/// Checks if a triangle is valid for the hexagon grid
		/// </summary>
		/// <returns><c>true</c>, if triangle was valided, <c>false</c> otherwise.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		private bool validTriangle(int x, int y)
		{
			if (y >= Size / 2)
			{
				// In bottom half
				int fromMiddle = y - Size / 2;
				int leftOffset = fromMiddle * 2 + 1;
				return x >= leftOffset;
			}
			else
			{
				// In top half
				int rightOffset = Size + y * 2;
				return x <= rightOffset;
			}
		}

		private void Align()
		{
			forAllItems(delegate (int x, int y, Sprite3 sprite)
			{
				if (x % 2 != 0)
				{
					sprite.setDisplayAngleDegrees(180);
					return;
				}
				sprite.setDisplayAngleDegrees(0);
			});
		}

		/// <summary>
		/// Loads the content.
		/// </summary>
		/// <param name="c">Content Manager.</param>
		public static void LoadContent(ContentManager c)
		{
			TrianglePiece.LoadContent(c);
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
					action(x, y, Sprites[x, y]);
				}
			}
		}

		/// <summary>
		/// Rotates the Grid right One step (60 Degrees) ONLY WORKS FOR A PIECE, mappings are hard coded for a y = 2x
		/// </summary>
		public void RotateRight()
		{
			Sprite3[,] newSprites = new Sprite3[Size * 2, Size];

			newSprites[0, 0] = Sprites[1, 1];
			newSprites[1, 0] = Sprites[0, 0];
			newSprites[2, 0] = Sprites[1, 0];
			newSprites[1, 1] = Sprites[2, 1];
			newSprites[2, 1] = Sprites[3, 1];
			newSprites[3, 1] = Sprites[2, 0];
			// not used for a rotation as a rotation is only the internal pieces
			newSprites[3, 0] = Sprites[3, 0];
			newSprites[0, 1] = Sprites[0, 1];

			Sprites = newSprites;

			// Update sprite positions
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// new sprite position
				s.setPos(getPosForSprite(x, y));
			});
			Align();
		}

		/// <summary>
		/// Flip this instance.
		/// </summary>
		public void Flip()
		{
			Sprite3[,] newSprites = new Sprite3[Size * 2, Size];

			newSprites[0, 0] = Sprites[1, 0];
			newSprites[1, 0] = Sprites[0, 0];
			newSprites[2, 0] = Sprites[1, 1];
			newSprites[3, 0] = Sprites[0, 1];
			newSprites[0, 1] = Sprites[3, 0];
			newSprites[1, 1] = Sprites[2, 0];
			newSprites[2, 1] = Sprites[3, 1];
			newSprites[3, 1] = Sprites[2, 1];

			Sprites = newSprites;

			// Update sprite positions
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// new sprite position
				s.setPos(getPosForSprite(x, y));
			});
			Align();
		}

		/// <summary>
		/// Gets the part spacing.
		/// </summary>
		/// <returns>The part spacing.</returns>
		public int getPartSpacing()
		{
			return PartSpacing;
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
		public void UpdateAsLevelGrid(GameTime gameTime, TrianglePiece piece)
		{
			particleEffects.Update(gameTime);

			List<Sprite3> spritesToHighlight = new List<Sprite3>();
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// Clear the display colours of all items that don't have an assigned color
				if (s.getActive() && !s.varBool0) // varBool0 indicates a dropped color - do not clear in this case
				{
					s.setColor(DefaultColor);
				}

				// Select squares to highlight
				if (piece != null)
				{
					piece.PieceGrid.forAllItems(delegate (int _x, int _y, Sprite3 part)
					{
						if (TrianglesOverlap(s, part))
						{
							spritesToHighlight.Add(s);
						}
					});
				}
			});
			// highlight the squares if it's the whole piece we're highlighting
			if (piece != null && spritesToHighlight.Count == piece.NumParts)
			{
				foreach (Sprite3 s in spritesToHighlight)
				{
					s.setColor(Util.lighterOrDarker(piece.PartColor, 0.8f));
				}
			}
		}

		/// <summary>
		/// If the given triangles overlap (one could be dropped into the other)
		/// </summary>
		/// <returns><c>true</c>, if overlap was trianglesed, <c>false</c> otherwise.</returns>
		/// <param name="gridTriangle">Grid triangle.</param>
		/// <param name="droppedTriangle">Dropped triangle.</param>
		private bool TrianglesOverlap(Sprite3 gridTriangle, Sprite3 droppedTriangle)
		{

			bool active = (droppedTriangle.getActive() && gridTriangle.getActive());
			bool filled = gridTriangle.varBool0; // grid triangle can't already be filled

			// same rotation
			bool rotationMatch = Math.Abs(gridTriangle.getDisplayAngleRadians() - droppedTriangle.getDisplayAngleRadians()) < 0.1;
			bool overlap = gridTriangle.getBoundingBoxAA().Contains(droppedTriangle.getBoundingBoxAA().Location);
			return active && !filled && rotationMatch && overlap;
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
		public bool CaptureDroppedPiece(TrianglePiece piece)
		{
			// Collect a list of squares that match
			List<Sprite3> matchedTriangles = new List<Sprite3>();
			if (piece != null)
			{
				forAllItems(delegate (int x, int y, Sprite3 s)
				{
					// Highlight the squares under the given piece (if they don't have a color)
					if (piece != null)
					{
						piece.PieceGrid.forAllItems(delegate (int _x, int _y, Sprite3 part)
						{
							if (TrianglesOverlap(s, part))
							{
								matchedTriangles.Add(s);
							}
						});
					}
				});
			}
			Console.WriteLine("Dropped: " + matchedTriangles.Count);
			// If all parts have a square, apply the colors and return true
			if (matchedTriangles.Count == piece.NumParts)
			{
				foreach (Sprite3 s in matchedTriangles)
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
				s.setPos(getPosForSprite(x, y));
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
