using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents an AI strategy
	/// </summary>
	public interface IAIStrategy
	{
		/// <summary>
		/// Makes a move in the given board
		/// </summary>
		/// <param name="board">The board</param>
		/// <param name="player">The AI player</param>
		BoardMove MakeMove(Board board, BoardTileType player);
	}
}
