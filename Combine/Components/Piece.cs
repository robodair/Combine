using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
namespace Combine
{
	public class Piece
	{
		Vector2 Position;
		Vector2 TargetPosition;
		public bool inPosition { get; set; }
		int moveSpeed = 5;
		const int partSize = 30;
		const int partSpacing = 5;

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
			// set the part hotspot offsets
			switch (layout)
			{
				case 0:
					// I block
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 2, Position.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 3, Position.Y);
					break;
				case 1:
					// O block
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y + partSize + partSpacing);
					break;
				case 2:
					// T Block
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 2, Position.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y + partSize + partSpacing);
					break;
				case 3:
					// J block
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 2, Position.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 2, Position.Y + partSize + partSpacing);
					break;
				case 4:
					// L block
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 2, Position.Y);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y + partSize + partSpacing);
					break;
				case 5:
					// S block
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 2, Position.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y + partSize + partSpacing);
					break;
				case 6:
					// Z block
					parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y);
					parts[1] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y);
					parts[2] = new Sprite3(true, UtilTexSI.texWhite, Position.X + partSize + partSpacing, Position.Y + partSize + partSpacing);
					parts[3] = new Sprite3(true, UtilTexSI.texWhite, Position.X + (partSize + partSpacing) * 2, Position.Y + partSize + partSpacing);
					break;
			}

			// Color the parts
			foreach (Sprite3 part in parts)
			{
				part.setWidthHeight(partSize, partSize);
				part.setColor(Color.Yellow);

			}
		}

		public void SetTargetPosition(Vector2 targetPosition)
		{
			inPosition = false;
			TargetPosition = targetPosition;
		}

		public void Update(GameTime gameTime)
		{
			// Move to the target position if not already there
			if (!Position.Equals(TargetPosition))
			{
				Vector2 movement = Vector2.Normalize(TargetPosition - Position) * moveSpeed;
				if (Math.Abs(Position.X - TargetPosition.X) < 1)
				{
					Position.X = TargetPosition.X;
				}
				if (Math.Abs(Position.Y - TargetPosition.Y) < 1)
				{
					Position.Y = TargetPosition.Y;
				}

				foreach (Sprite3 part in parts)
				{
					part.moveByDeltaX(movement.X);
					part.moveByDeltaY(movement.Y);
				}

				Position += movement;
			}
			else
			{
				inPosition = true;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Sprite3 part in parts)
			{
				part.Draw(sb);
				part.drawInfo(sb, Color.AliceBlue, Color.Goldenrod);
			}
		}
	}
}
