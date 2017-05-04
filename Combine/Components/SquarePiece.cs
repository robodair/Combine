using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using Microsoft.Xna.Framework;
namespace Combine
{
	public class SquarePiece : GUI_Control, ShapePiece
	{
		static public Texture2D Texture { get; private set; }

		public SquareGrid PieceGrid { get; private set; } // A grid to store the components of this piece in. 
		public int NumParts { get; private set; }
		public Color PartColor { get; private set; }

		// DO NOT call the level methods on this grid
		Random Rand;
		public bool inPosition { get; private set; }
		Vector2 Position;
		Vector2 TargetPosition;
		int moveSpeed = 5;
		bool dragging;
		Vector2 dragOffset;
		int PartSpacing;            // Default part spacing (between the outer edges of parts)
		int PartSize;               // Width/Height of parts


		public SquarePiece(float x, int partSize, int partSpacing)
		{
			PartSize = partSize;
			PartSpacing = partSpacing;
			Rand = new Random();
			inPosition = false;
			Position = new Vector2(x, -50); // start above the game
			createTetrominoPart(); // New square piece (Tetromino)
			NumParts = 4;
		}

		private void createTetrominoPart()
		{
			switch (Rand.Next(0, 5)) // 5 different colours for square pieces
			{
				case 0:
					PartColor = Color.Chartreuse; // Green
					break;
				case 1:
					PartColor = Color.Yellow;
					break;
				case 2:
					PartColor = Color.DodgerBlue;
					break;
				case 3:
					PartColor = Color.Crimson; // Red
					break;
				case 4:
					PartColor = Color.DarkOrchid; // Purple
					break;
			}

			PieceGrid = new SquareGrid(4, 0, 0, 30, 5, false, PartColor);

			// Setup active sprites for the grid
			switch (Rand.Next(0, 7))
			{
				case 0:
					// I block
					PieceGrid.getSprite(0, 0).setActive(true);
					PieceGrid.getSprite(1, 0).setActive(true);
					PieceGrid.getSprite(2, 0).setActive(true);
					PieceGrid.getSprite(3, 0).setActive(true);
					break;
				case 1:
					// O block
					PieceGrid.getSprite(0, 0).setActive(true);
					PieceGrid.getSprite(1, 0).setActive(true);
					PieceGrid.getSprite(0, 1).setActive(true);
					PieceGrid.getSprite(1, 1).setActive(true);
					break;
				case 2:
					// T Block
					PieceGrid.getSprite(0, 0).setActive(true);
					PieceGrid.getSprite(1, 0).setActive(true);
					PieceGrid.getSprite(2, 0).setActive(true);
					PieceGrid.getSprite(1, 1).setActive(true);
					break;
				case 3:
					// J block
					PieceGrid.getSprite(1, 0).setActive(true);
					PieceGrid.getSprite(1, 1).setActive(true);
					PieceGrid.getSprite(1, 2).setActive(true);
					PieceGrid.getSprite(0, 2).setActive(true);
					break;
				case 4:
					// L block
					PieceGrid.getSprite(0, 0).setActive(true);
					PieceGrid.getSprite(1, 0).setActive(true);
					PieceGrid.getSprite(2, 0).setActive(true);
					PieceGrid.getSprite(2, 1).setActive(true);
					break;
				case 5:
					// S block
					PieceGrid.getSprite(1, 0).setActive(true);
					PieceGrid.getSprite(2, 0).setActive(true);
					PieceGrid.getSprite(0, 1).setActive(true);
					PieceGrid.getSprite(1, 1).setActive(true);
					break;
				case 6:
					// Z block
					PieceGrid.getSprite(0, 0).setActive(true);
					PieceGrid.getSprite(1, 0).setActive(true);
					PieceGrid.getSprite(1, 1).setActive(true);
					PieceGrid.getSprite(2, 1).setActive(true);
					break;
			}

			// Set the bounds for the piece
			bounds = new Rectangle(Position.ToPoint(),
								   new Point(PartSize * 4 + PartSpacing * 3, PartSize * 4 + PartSpacing * 3));
		}

		public static void LoadContent(ContentManager c)
		{
			if (Texture == null)
			{
				Texture = c.Load<Texture2D>("textures/pieces/square");
			}
		}

		public void SetTargetPosition(Vector2 targetPosition)
		{
			inPosition = false;
			TargetPosition = targetPosition;
		}

		public override void Update(GameTime gameTime)
		{
			// Move to the target position if not already there
			if (!Position.Equals(TargetPosition))
			{
				Vector2 movement = Vector2.Normalize(TargetPosition - Position) * moveSpeed;
				if (Math.Abs(Position.X - TargetPosition.X) < moveSpeed)
				{
					movement.X = TargetPosition.X - Position.X;
				}
				if (Math.Abs(Position.Y - TargetPosition.Y) < moveSpeed)
				{
					movement.Y = TargetPosition.Y - Position.Y;
				}

				moveBy(movement);
			}
			else
			{
				inPosition = true;
			}
			base.Update(gameTime);
		}

		public void beginDrag(float x, float y)
		{
			if (!dragging)
			{
				dragging = true;
				dragOffset = new Vector2(x, y) - Position;
			}
		}

		public void setDragPos(float x, float y)
		{
			Vector2 movement = new Vector2(x, y) - Position - dragOffset;
			moveBy(movement);
		}

		public override void setPos(float x, float y)
		{
			Vector2 movement = new Vector2(x, y) - Position;
			moveBy(movement);
		}

		public void moveBy(Vector2 movement)
		{
			Position += movement;
			PieceGrid.setPos(Position);
			base.setPos(Position.X - (PartSize / 2), Position.Y - (PartSize / 2));
			// TODO possibly offset the piece based on rotation?
		}

		public override void Draw(SpriteBatch sb)
		{
			PieceGrid.Draw(sb, true);

			// Debug drawing
			LineBatch.drawLineRectangle(sb, bounds, Color.Goldenrod);
			LineBatch.drawCross(sb, Position.X, Position.Y, 5, Color.GhostWhite, Color.GhostWhite);
			base.Draw(sb);
		}

		public void RotateRight()
		{
			PieceGrid.RotateRight();
		}

		public void endDrag()
		{
			dragging = false;
		}
	}
}
