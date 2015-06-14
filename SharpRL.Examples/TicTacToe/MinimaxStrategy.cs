using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents a strategy using the Minimax algorithm
	/// </summary>
	public sealed class MinimaxStrategy : IAIStrategy
	{
		/// <summary>
		/// Returns the next player
		/// </summary>
		/// <param name="player">The current player</param>
		private BoardTileType NextPlayer(BoardTileType player)
		{
			if (player == BoardTileType.Cross)
			{
				return BoardTileType.Circle;
			}
			else
			{
				return BoardTileType.Cross;
			}
		}

		/// <summary>
		/// Finds index for the element with the best value
		/// </summary>
		/// <typeparam name="T">The type of the element</typeparam>
		/// <param name="list">The list</param>
		/// <param name="compare">The function that desides if an element is better than the other</param>
		private static int FindBestIndex<T>(IList<T> list, Func<T, T, bool> compare)
		{
			if (list.Count == 0)
			{
				throw new ArgumentException("The list must have atleast one element.");
			}

			int bestIndex = 0;

			for (int i = 1; i < list.Count; i++)
			{
				if (compare(list[i], list[bestIndex]))
				{
					bestIndex = i;
				}
			}

			return bestIndex;
		}

		/// <summary>
		/// Calculates the score
		/// </summary>
		/// <param name="player">The player</param>
		/// <param name="winner">The winner</param>
		private int Score(BoardTileType player, GameWinner winner)
		{
			if (winner == GameWinner.Tie)
			{
				return 0;
			}
			else if (Board.AreEqual(player, winner))
			{
				return 10;
			}
			else
			{
				return -10;
			}
		}

		/// <summary>
		/// Calculates the best move for the given player
		/// </summary>
		/// <param name="player">The player</param>
		/// <param name="board">The board</param>
		/// <param name="turnPlayer">The current player in the turn</param>
		/// <param name="move">The move to make</param>
		/// <returns>The score</returns>
		private int Minimax(BoardTileType player, Board board, BoardTileType turnPlayer, out BoardMove move)
		{
			var winner = board.GetWinner();
			if (winner != null)
			{
				move = new BoardMove(-1, -1);
				return this.Score(player, winner.Value);
			}

			var scores = new List<int>();
			var moves = new List<BoardMove>();

			//Go through all possible games from the current board
			foreach (var possibleMove in board.GetMoves())
			{
				var possibleBoard = board.WithMove(possibleMove.X, possibleMove.Y, turnPlayer);
				scores.Add(this.Minimax(player, possibleBoard, this.NextPlayer(turnPlayer), out move));
				moves.Add(possibleMove);
			}

			if (player == turnPlayer)
			{
				//Max calculation
				var maxIndex = FindBestIndex(scores, (x, y) => x > y);
				move = moves[maxIndex];
				return scores[maxIndex];
			}
			else
			{
				//Min calculation
				var minIndex = FindBestIndex(scores, (x, y) => x < y);
				move = moves[minIndex];
				return scores[minIndex];
			}
		}

		/// <summary>
		/// Makes a move for the given player in the given board
		/// </summary>
		/// <param name="board">The board</param>
		/// <param name="player">The player</param>
		public BoardMove MakeMove(Board board, BoardTileType player)
		{
			var move = new BoardMove(-1, -1);
			this.Minimax(player, board, player, out move);
			return move;
		}
	}
}
