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
		public bool inPosition { get; set;}
		int moveSpeed = 5;

		Sprite3[] parts;

		public Piece(float x)
		{
			inPosition = false;
			Position = new Vector2(x, -50); // start above the game
			parts = new Sprite3[1]; // will be variable length later
			parts[0] = new Sprite3(true, UtilTexSI.texWhite, Position.X, Position.Y);
			parts[0].setWidthHeight(10, 10);
			parts[0].setHSoffset(Vector2.Zero);
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
