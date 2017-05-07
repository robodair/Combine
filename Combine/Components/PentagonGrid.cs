using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using System.Collections.Generic;

namespace Combine
{
	public class PentagonGrid : ShapeGrid3D<PentagonPiece>
	{
		Color DefaultColor;
		Sprite3[,,] Sprites;        // The array of sprites
		int Size;                   // The size of the grid
		Random Rand;                // Random used for decision making
		int PartSpacing;            // Default part spacing (between the outer edges of parts)
		int PartSize;               // Width/Height of parts
		int OffsetX;
		int OffsetY;
		RC_RenderableList particleEffects; // List of particle emitters

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Combine.PentagonGrid"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="offset_x">Offset x.</param>
		/// <param name="offset_y">Offset y.</param>
		/// <param name="partSize">Part size.</param>
		/// <param name="spriteActive">Whether sprites are visible or invisible when created or now</param>
		/// <param name="defaultColor">Part spacing.</param>
		public PentagonGrid(int size, int offset_x, int offset_y, int partSize, bool spriteActive = true, Color? defaultColor = null)
		{
			DefaultColor = defaultColor ?? Color.DimGray;
			PartSize = partSize;
			PartSpacing = PartSize / 6;
			OffsetX = offset_x;
			OffsetY = offset_y;
			Size = size;
			Rand = new Random();
			Sprites = new Sprite3[Size, Size + 2, 4];
			particleEffects = new RC_RenderableList();
			forAllItems3D(delegate (int x, int y, int z, Sprite3 sprite)
			{
				Vector2 spritePos = getPosForSprite(x, y, z);
				// Initialise a sprite
				Sprites[x, y, z] = new Sprite3(true, PentagonPiece.Texture, spritePos.X, spritePos.Y);
				Sprites[x, y, z].setActive(spriteActive);
				Sprites[x, y, z].setColor(DefaultColor);
				Sprites[x, y, z].setWidthHeight(partSize, partSize);
				Sprites[x, y, z].setBBandHSFractionOfTexCentered(0.6f);
			});
			Align(); // this will ensure all the pentagons point the right way
		}

		private void Align()
		{
			forAllItems3D(delegate (int x, int y, int z, Sprite3 sprite)
			{
				switch (z)
				{
					case 0:
						sprite.setDisplayAngleDegrees(90);
						break;
					case 1:
						sprite.setDisplayAngleDegrees(0);
						break;
					case 2:
						sprite.setDisplayAngleDegrees(270);
						break;
					case 3:
						sprite.setDisplayAngleDegrees(180);
						break;
				}
			});
		}

		Vector2 getPosForSprite(int x, int y, int z)
		{
			int rowOffset = 0;
			if (y % 2 != 0)
			{
				// even rows are indented by partsize + partspacing
				rowOffset = PartSize + PartSpacing;
			}
			Vector2 basePosition = new Vector2(x * (PartSize + PartSpacing) * 2 + OffsetX + rowOffset,
											   y * (PartSize + PartSpacing) + OffsetY);
			switch (z)
			{
				case 0:
					break;
				case 1:
					basePosition.X += PartSize / 2 + PartSpacing;
					basePosition.Y -= PartSize / 2;
					break;
				case 2:
					basePosition.X += PartSize * 1.35f;
					break;
				case 3:
					basePosition.X += PartSize / 2 + PartSpacing;
					basePosition.Y += PartSize / 2;
					break;
			}
			return basePosition;
		}

		/// <summary>
		/// Loads the content.
		/// </summary>
		/// <param name="c">Content Manager.</param>
		public static void LoadContent(ContentManager c)
		{
			PentagonPiece.LoadContent(c);
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
					action(x, y, null); // There are 4 sprite3's at this location. We can't pass all of them.
				}
			}
		}

		/// <summary>
		/// For all the items in the grid, call the method given, with the coordinates of the item
		/// </summary>
		/// <param name="action">Call this method.</param>
		public void forAllItems3D(Action<int, int, int, Sprite3> action)
		{
			for (int x = 0; x < Sprites.GetLength(0); x++)
			{
				for (int y = 0; y < Sprites.GetLength(1); y++)
				{
					for (int z = 0; z < 4; z++)
					{
						action(x, y, z, Sprites[x, y, z]); // There are 4 sprite3's at this location. We can't pass all of them.
					}
				}
			}
		}

		/// <summary>
		/// Rotates the Grid right One step (90 Degrees)
		/// </summary>
		public void RotateRight()
		{
			Sprite3[,,] newSprites = new Sprite3[Size, Size + 2, 4];
			// there are 4 possible rotations for each pentagon, possibly manual mapping is the only sane way to do this

		}

		/// <summary>
		/// Draw the specified sb and debug.
		/// </summary>
		/// <param name="sb">SpriteBatch.</param>
		/// <param name="debug">If set to <c>true</c> draw debug.</param>
		public void Draw(SpriteBatch sb, bool debug)
		{
			forAllItems3D(delegate (int x, int y, int z, Sprite3 s)
						{
							s.Draw(sb);
							if (debug)
							{
								//s.drawInfo(sb, Color.AliceBlue, Color.Goldenrod);
							}
						});
			particleEffects.Draw(sb);
		}

		/// <summary>
		/// Updates as a level grid.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="piece">Piece.</param>
		public void UpdateAsLevelGrid(GameTime gameTime, PentagonPiece piece)
		{
			particleEffects.Update(gameTime);
			forAllItems3D(delegate (int x, int y, int z, Sprite3 s)
			{
				// Clear the display colours of all items that don't have an assigned color
				if (s.getActive() && !s.varBool0) // varBool0 indicates a dropped color - do not clear in this case
				{
					s.setColor(DefaultColor);
				}

				// Highlight the pentagons under the given piece (if they don't have a color)
				if (piece != null)
				{
					piece.PieceGrid.forAllItems3D(delegate (int _x, int _y, int _z, Sprite3 part)
					{
						if (PentagonsOverlap(s, part))
						{
							s.setColor(Util.lighterOrDarker(part.colour, 0.8f));
						}
					});
				}
			});
		}

		/// <summary>
		/// Checks for completed shapes and replaces them with particle effects
		/// </summary>
		/// <returns>The number completed pentagonss.</returns>
		public int RemoveCompletedShapes()
		{
			HashSet<Sprite3> pentagonsToClear = new HashSet<Sprite3>();
			int numMatches = 0;

			// for each pentagon check color of: the next door one, the one below, and the one diagonally below
			// and add them to the list plus create particle effects for them
			forAllItems(delegate (int x, int y, Sprite3 s)
			{
				// check the block at [x, y]

				// check the block that [x, y, 3] is the top of.
			});

			// Spawn particle systems and reset colors TODO: Make the effect nicer
			foreach (Sprite3 s in pentagonsToClear)
			{
				// reset the colours
				s.setColor(DefaultColor);
				s.varBool0 = false; // varBool0 is whether the color is perm
									// Create a particle system for the pentagon
									// create a new particle system for each of the pentagons
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
		public bool CaptureDroppedPiece(PentagonPiece piece)
		{
			// Collect a list of pentagons that match
			List<Sprite3> matchedPentagons = new List<Sprite3>();
			if (piece != null)
			{
				forAllItems3D(delegate (int x, int y, int z, Sprite3 s)
				{
					// Highlight the pentagons under the given piece (if they don't have a color)
					if (piece != null)
					{
						piece.PieceGrid.forAllItems3D(delegate (int _x, int _y, int _z, Sprite3 part)
						{
							if (PentagonsOverlap(s, part))
							{
								matchedPentagons.Add(s);
							}
						});
					}
				});
			}

			// If all parts have a pentagon, apply the colors and return true
			if (matchedPentagons.Count == piece.NumParts)
			{
				foreach (Sprite3 s in matchedPentagons)
				{
					s.setColor(piece.PartColor);
					s.varBool0 = true; // set the flag that this part has been filled
				}
				return true;
			}
			return false;
		}

		private bool PentagonsOverlap(Sprite3 gridSprite, Sprite3 droppedSprite)
		{
			bool active = (droppedSprite.getActive() && gridSprite.getActive());
			bool filled = gridSprite.varBool0; // grid triangle can't already be filled

			// same rotation
			bool rotationMatch = Math.Abs(gridSprite.getDisplayAngleRadians() - droppedSprite.getDisplayAngleRadians()) < 0.1;
			bool overlap = gridSprite.getBoundingBoxAA().Contains(droppedSprite.getBoundingBoxAA().Location);
			return active && !filled && rotationMatch && overlap;
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
			forAllItems3D(delegate (int x, int y, int z, Sprite3 s)
						{
							// new sprite positions and rotations
							s.setPos(getPosForSprite(x, y, z));
						});
		}

		/// <summary>
		/// Gets the sprite at (x, y).
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Sprite3 getSprite(int x, int y)
		{
			return Sprites[x, y, 0];
		}

		/// <summary>
		/// Gets the sprite at (x, y, z).
		/// </summary>
		/// <returns>The sprite.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		public Sprite3 getSprite(int x, int y, int z)
		{
			return Sprites[x, y, z];
		}

		/// <summary>
		/// Gets the part spacing.
		/// </summary>
		/// <returns>The part spacing.</returns>
		public int getPartSpacing()
		{
			return PartSpacing;
		}
	}
}
