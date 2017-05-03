using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Combine
{
	public interface ShapePiece
	{
		int NumParts { get; }
		Color PartColor { get; }
		bool inPosition { get; }

		/// <summary>
		/// Sets the target position.
		/// </summary>
		/// <param name="targetPosition">Target position.</param>
		void SetTargetPosition(Vector2 targetPosition);

		/// <summary>
		/// Update.
		/// </summary>
		/// <returns>The update.</returns>
		/// <param name="gameTime">Game time.</param>
		void Update(GameTime gameTime);

		/// <summary>
		/// Begins the drag.
		/// Wrapper for moveBy
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		void beginDrag(float x, float y);

		/// <summary>
		/// Ends the drag.
		/// </summary>
		void endDrag();

		/// <summary>
		/// Sets the drag position.
		/// Wrapper for moveBy
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		void setDragPos(float x, float y);

		/// <summary>
		/// Move all parts by a vector
		/// </summary>
		/// <param name="movement">Movement.</param>
		void moveBy(Vector2 movement);

		/// <summary>
		/// Sets the position.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		void setPos(float x, float y);

		/// <summary>
		/// Draw the specified sb and debug.
		/// </summary>
		/// <returns>The draw.</returns>
		/// <param name="sb">Sb.</param>
		void Draw(SpriteBatch sb);

		/// <summary>
		/// Rotate this instance.
		/// </summary>
		void RotateRight();
	}
}
