using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RC_Framework;

namespace Combine
{
	public interface ShapeGrid<PieceType>
	{
		/// <summary>
		/// Rotates the grid right one step.
		/// </summary>
		void RotateRight();

		/// <summary>
		/// Draw the specified sb and debug.
		/// </summary>
		/// <returns>The draw.</returns>
		/// <param name="sb">Sb.</param>
		/// <param name="debug">If set to <c>true</c> draw debug.</param>
		void Draw(SpriteBatch sb, bool debug);

		/// <summary>
		/// Updates as level grid.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="piece">Piece.</param>
		void UpdateAsLevelGrid(GameTime gameTime, PieceType piece);

		/// <summary>
		/// Checks for completed squares and replaces them with particle effects
		/// </summary>
		/// <returns>The number completed squares.</returns>
		int RemoveCompletedShapes();

		/// <summary>
		/// Captures the dropped object.
		/// </summary>
		/// <returns><c>true</c>, if dropped object was captured, <c>false</c> otherwise.</returns>
		/// <param name="piece">Piece</param>
		bool CaptureDroppedPiece(PieceType piece);

		/// <summary>
		/// For all items.
		/// </summary>
		/// <param name="action">Action.</param>
		void forAllItems(Action<int, int, Sprite3> action);

		void setPos(Vector2 position);
	}
}
