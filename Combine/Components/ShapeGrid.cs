﻿using System;
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

		/// <summary>
		/// Sets the position.
		/// </summary>
		/// <param name="position">Position.</param>
		void setPos(Vector2 position);

		/// <summary>
		/// Gets the sprite at (x, y).
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		Sprite3 getSprite(int x, int y);

		/// <summary>
		/// Gets the part spacing.
		/// </summary>
		/// <returns>The part spacing.</returns>
		int getPartSpacing();

		/// <summary>
		/// Return true if the given pieces cannot be applied to the grid.
		/// </summary>
		/// <returns><c>true</c>, if no moves left, <c>false</c> otherwise.</returns>
		/// <param name="pieces">Pieces.</param>
		bool hasNoMovesLeft(ShapePiece[] pieces);
	}

	/// <summary>
	/// Extension of shapegrid interface for grids that have a 3D internal representation
	/// </summary>
	public interface ShapeGrid3D<PieceType> : ShapeGrid<PieceType>
	{
		/// <summary>
		/// For all items.
		/// </summary>
		/// <param name="action">Action.</param>
		void forAllItems3D(Action<int, int, int, Sprite3> action);

		/// <summary>
		/// Gets the sprite at (x, y, z).
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		Sprite3 getSprite(int x, int y, int z);
	}
}
