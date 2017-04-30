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
		const int partSize = 30;
		const int partSpacing = 5;
		public Color color;

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
			// set the part hotspot offsets
			switch (layout)
			{
				case 0:
					// I block
					PartsOffset = new Vector2(0, (partSize + partSpacing) * 1.5f);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 3, PartsOrigin.Y);
					break;
				case 1:
					// O block
					PartsOffset = new Vector2(partSize + partSpacing, partSize + partSpacing);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 2:
					// T Block
					PartsOffset = new Vector2((partSize + partSpacing) * 0.5f, partSize + partSpacing);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 3:
					// J block
					PartsOffset = new Vector2((partSize + partSpacing) * 0.5f, partSize + partSpacing);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 4:
					// L block
					PartsOffset = new Vector2((partSize + partSpacing) * 0.5f, partSize + partSpacing);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y + partSize + partSpacing);
					break;
				case 5:
					// S block
					PartsOffset = new Vector2((partSize + partSpacing) * 0.5f, partSize + partSpacing);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, Position.Y + partSize + partSpacing);
					break;
				case 6:
					// Z block
					PartsOffset = new Vector2((partSize + partSpacing) * 0.5f, partSize + partSpacing);
					PartsOrigin = Position + PartsOffset;
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X, PartsOrigin.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + partSize + partSpacing, PartsOrigin.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, PartsOrigin.X + (partSize + partSpacing) * 2, PartsOrigin.Y + partSize + partSpacing);
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
					Position.X = TargetPosition.X;
				}
				if (Math.Abs(Position.Y - TargetPosition.Y) < moveSpeed)
				{
					Position.Y = TargetPosition.Y;
				}

				foreach (Sprite3 part in parts)
				{
					part.moveByDeltaX(movement.X);
					part.moveByDeltaY(movement.Y);
				}

				Position += movement;
				setPos(Position.X, Position.Y);
			}
			else
			{
				inPosition = true;
			}
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch sb)
		{
			foreach (Sprite3 part in parts)
			{
				part.Draw(sb);
				part.drawInfo(sb, Color.AliceBlue, Color.Goldenrod);
			}

			// Debug drawing
			LineBatch.drawLineRectangle(sb, bounds, Color.Goldenrod);
			LineBatch.drawCross(sb, Position.X, Position.Y, 5, Color.GhostWhite, Color.GhostWhite);
			base.Draw(sb);
		}

		public void Rotate()
		{
			// execute a rotation (rotate all the parts areound the center of the bounds)
			Console.WriteLine("Rotate!!");

		}
	}
}
