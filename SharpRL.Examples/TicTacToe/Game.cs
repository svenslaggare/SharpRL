using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents a game
	/// </summary>
	public sealed class Game
	{
		private readonly Board board;
		private BoardTileType turnPlayer = BoardTileType.Cross;
		private IAIStrategy strategy = new MinimaxStrategy();

		/// <summary>
		/// Creates a new game
		/// </summary>
		public Game()
		{
			this.board = new Board();
		}

		/// <summary>
		/// Returns the winner
		/// </summary>
		public GameWinner? GetWinner()
		{
			return this.board.GetWinner();
		}

		/// <summary>
		/// Returns the board
		/// </summary>
		public Board Board
		{
			get { return this.board; }
		}

		/// <summary>
		/// Advances the turn to the next player
		/// </summary>
		private void Next()
		{
			this.turnPlayer = this.NextPlayer(this.turnPlayer);
		}

		/// <summary>
		/// Makes a move at the given position
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <returns>True if the move was successful</returns>
		public bool Move(int x, int y)
		{
			if (this.board.Move(x, y, this.turnPlayer))
			{
				this.Next();
				return true;
			}
			else
			{
				return false;
			}
		}

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
		/// Makes move for the computer
		/// </summary>
		public void ComputerMove()
		{
			var move = this.strategy.MakeMove(this.board, this.turnPlayer);
			this.Move(move.X, move.Y);
		}
	}
}
