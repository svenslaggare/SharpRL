using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents a strategy that just makes random moves
	/// </summary>
	public sealed class RandomStrategy : IAIStrategy
	{
		private Random random;

		/// <summary>
		/// Creates a new random strategy
		/// </summary>
		/// <param name="random">The random generator</param>
		public RandomStrategy(Random random)
		{
			this.random = random;
		}

		/// <summary>
		/// Makes a move for the given player in the given board
		/// </summary>
		/// <param name="board">The board</param>
		/// <param name="player">The player</param>
		public BoardMove MakeMove(Board board, BoardTileType player)
		{
			return board.RandomEmptyPosition(this.random) ?? new BoardMove(0, 0);
		}
	}
}
