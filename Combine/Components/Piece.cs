using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
namespace Combine
{
	public class Piece : GUI_Control
	{
		Vector2 Position;
		Vector2 TargetPosition;
		Vector2 PartsOffset; // Offset of the parts from the piece position
		public bool inPosition { get; set; }
		int moveSpeed = 5;
		public const int partSize = 30;
		public const int partSpacing = 5;
		public Color color;
		int rotationStep;
		public bool dragging { get; set; }
		Vector2 dragOffset;

		static Texture2D squarePartTexture;

		Sprite3[] parts;

		Random rand;

		public Piece(float x, String type)
		{
			rand = new Random();
			inPosition = false;
			Position = new Vector2(x, -50); // start above the game
			createPieces(type);
		}

		private void createPieces(String type)
		{
			switch (type)
			{
				case "square":
					rotationStep = 90;
					parts = new Sprite3[4];
					createTetrominoParts(rand.Next(0, 7)); // New square piece (Tetromino)
					break;
				case "triangle":
					throw new NotImplementedException();
					break;
				case "pentagon":
					throw new NotImplementedException();
					break;
			}
		}

		private void createTetrominoParts(int layout)
		{
			Vector2 PartsOrigin;
			// default part offset
			PartsOffset = new Vector2(partSize + partSpacing * 0.5f, partSize * 1.5f + partSpacing);
			PartsOrigin = Position + PartsOffset;

			// set the part hotspot offsets
			switch (layout)
			{
				case 0:
					// I block
					PartsOffset = new Vector2(partSize * 0.5f, (partSize + partSpacing) * 2f);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 3, PartsOrigin.Y);
					break;
				case 1:
					// O block
					PartsOffset = new Vector2(partSize * 1.5f + partSpacing, partSize * 1.5f + partSpacing);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 2:
					// T Block
					parts[0] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 3:
					// J block
					parts[0] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 4:
					// L block
					parts[0] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 5:
					// S block
					parts[0] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[1] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[2] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 6:
					// Z block
					parts[0] = new Sprite3(true, squarePartTexture, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, squarePartTexture, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, squarePartTexture, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y + partSize + partSpacing);
					break;
			}

			// Color the parts
			switch (rand.Next(0, 5)) // 5 different colours for square pieces
			{
				case 0:
					color = Color.Chartreuse; // Green
					break;
				case 1:
					color = Color.Yellow;
					break;
				case 2:
					color = Color.DodgerBlue;
					break;
				case 3:
					color = Color.Crimson; // Red
					break;
				case 4:
					color = Color.DarkOrchid; // Purple
					break;
			}

			foreach (Sprite3 part in parts)
			{
				part.setBBandHSFractionOfTexCentered(1);
				part.setWidthHeight(partSize, partSize);
				part.setColor(color);

			}

			// Set the bounds for the piece
			bounds = new Rectangle(Position.ToPoint(), new Point(partSize * 4 + partSpacing * 3, partSize * 4 + partSpacing * 3));
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
			foreach (Sprite3 part in parts)
			{
				part.moveByDeltaX(movement.X);
				part.moveByDeltaY(movement.Y);
			}

			Position += movement;
			base.setPos(Position.X, Position.Y);
		}

		public override void Draw(SpriteBatch sb)
		{
			foreach (Sprite3 part in parts)
			{
				part.Draw(sb);
				//part.drawInfo(sb, Color.AliceBlue, Color.Goldenrod);
			}

			// Debug drawing
			//LineBatch.drawLineRectangle(sb, bounds, Color.Goldenrod);
			//LineBatch.drawCross(sb, Position.X, Position.Y, 5, Color.GhostWhite, Color.GhostWhite);
			base.Draw(sb);
		}

		public void Rotate()
		{
			// execute a rotation (rotate all the parts around the center of the bounds)
			Console.WriteLine("Rotate!!");
			foreach (Sprite3 part in parts)
			{
				part.setPos(Util.rotatePointDeg(part.getPos(), bounds.Center.ToVector2(), rotationStep));
			}
		}

		public static void LoadContent(ContentManager c)
		{
			squarePartTexture = c.Load<Texture2D>("textures/pieces/square");
		}
	}
}
