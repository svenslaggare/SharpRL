using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents the state of the board
	/// </summary>
	public struct BoardState
	{
		private readonly int tiles;

		/// <summary>
		/// Creates a new board state
		/// </summary>
		/// <param name="board">The board</param>
		public BoardState(Board board)
		{
			this.tiles = board.Tiles;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = (BoardState)obj;
			return this.tiles == other.tiles;
		}

		public override int GetHashCode()
		{
			return this.tiles;
		}
	}
}
