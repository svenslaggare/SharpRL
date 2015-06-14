using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// The winner of the game
	/// </summary>
	public enum GameWinner
	{
		Tie,
		Cross,
		Circle
	}

	/// <summary>
	/// The type of board tiles
	/// </summary>
	public enum BoardTileType : byte
	{
		Empty,
		Cross,
		Circle
	}

	/// <summary>
	/// Represents a board move
	/// </summary>
	public struct BoardMove
	{
		/// <summary>
		/// The x position of the move
		/// </summary>
		public int X { get; private set; }

		/// <summary>
		/// The y position of the move
		/// </summary>
		public int Y { get; private set; }


		/// <summary>
		/// Creates a new board move
		/// </summary>
		/// <param name="x">The x position of the move</param>
		/// <param name="y">The y position of the move</param>
		public BoardMove(int x, int y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}
	}

	/// <summary>
	/// Represents a tile in the board
	/// </summary>
	public struct BoardTile
	{
		/// <summary>
		/// The x position 
		/// </summary>
		public int X { get; private set; }

		/// <summary>
		/// The y position
		/// </summary>
		public int Y { get; private set; }

		/// <summary>
		/// The type of the tile
		/// </summary>
		public BoardTileType TileType { get; private set; }

		/// <summary>
		/// Creates a new board tile position
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="tileType">The type of the tile</param>
		public BoardTile(int x, int y, BoardTileType tileType)
			: this()
		{
			this.X = x;
			this.Y = y;
			this.TileType = tileType;
		}
	}

	/// <summary>
	/// Represents a board
	/// </summary>
	public sealed class Board
	{
		private int tiles;
		private readonly int size;

		/// <summary>
		/// The winning patterns
		/// </summary>
		private static readonly int[] winningPatterns = new int[]
		{
			 0x1C0, 0x38, 0x7,  // rows
			 0x124, 0x92, 0x49, // cols
			 0x111, 0x54        // diagonals
	    };

		/// <summary>
		/// Creates a new board
		/// </summary>
		/// <param name="size">The size of the board</param>
		public Board(int size = 3)
		{
			this.tiles = 0;
			this.size = size;
		}

		/// <summary>
		/// Returns the size of the board
		/// </summary>
		public int Size
		{
			get
			{
				return this.size;
			}
		}

		/// <summary>
		/// Returns the tiles
		/// </summary>
		public int Tiles
		{
			get { return this.tiles; }
		}

		/// <summary>
		/// Returns the tiles
		/// </summary>
		public IEnumerable<BoardTile> GetTiles()
		{
			for (int y = 0; y < this.Size; y++)
			{
				for (int x = 0; x < this.Size; x++)
				{
					yield return new BoardTile(x, y, this.GetTile(x, y));
				}
			}
		}

		/// <summary>
		/// Resets the board
		/// </summary>
		public void Reset()
		{
			this.tiles = 0;
		}

		/// <summary>
		/// Returns the tile at the given position
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		public BoardTileType GetTile(int x, int y)
		{
			var tileType = this.tiles >> (x + y * this.Size) * 2;
			return (BoardTileType)(tileType & 0x3);
		}

		/// <summary>
		/// Sets the given tile
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="tile">The tile</param>
		public void SetTile(int x, int y, BoardTileType tile)
		{
			this.tiles |= (int)tile << (x + y * this.size) * 2;
		}

		/// <summary>
		/// Indicates if the given position is empty
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		public bool IsEmpty(int x, int y)
		{
			return this.GetTile(x, y) == BoardTileType.Empty;
		}

		/// <summary>
		/// Makes the given move
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="tile">The tile to place</param>
		/// <returns>True if moved else false</returns>
		public bool Move(int x, int y, BoardTileType tile)
		{
			if (this.GetTile(x, y) == BoardTileType.Empty)
			{
				this.SetTile(x, y, tile);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns a new board with the given move
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="tile">The tile to place</param>
		/// <returns>A new board with the move</returns>
		public Board WithMove(int x, int y, BoardTileType tile)
		{
			var newBoard = new Board(this.Size);
			newBoard.tiles = this.tiles;

			if (newBoard.GetTile(x, y) == BoardTileType.Empty)
			{
				newBoard.SetTile(x, y, tile);
			}

			return newBoard;
		}

		/// <summary>
		/// Returns the moves that can be made
		/// </summary>
		public IEnumerable<BoardMove> GetMoves()
		{
			for (int y = 0; y < this.size; y++)
			{
				for (int x = 0; x < this.size; x++)
				{
					if (this.GetTile(x, y) == BoardTileType.Empty)
					{
						yield return new BoardMove(x, y);
					}
				}
			}
		}

		/// <summary>
		/// Indicates the board is full
		/// </summary>
		public bool IsFull()
		{
			for (int y = 0; y < this.size; y++)
			{
				for (int x = 0; x < this.size; x++)
				{
					if (this.GetTile(x, y) == BoardTileType.Empty)
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Returns the winner for the given tile
		/// </summary>
		/// <param name="tile">The tile</param>
		private GameWinner? WinnerForTile(BoardTileType tile)
		{
			switch (tile)
			{
				case BoardTileType.Cross:
					return GameWinner.Cross;
				case BoardTileType.Circle:
					return GameWinner.Circle;
				default:
					return null;
			}
		}

		/// <summary>
		/// Returns the horizontal winner
		/// </summary>
		private GameWinner? GetHorizontalWinner()
		{
			for (int y = 0; y < this.size; y++)
			{
				BoardTileType? tile = null;
				int count = 0;

				for (int x = 0; x < this.size; x++)
				{
					if (tile == null)
					{
						tile = this.GetTile(x, y);
						count = 1;
						continue;
					}

					if (this.GetTile(x, y) == tile)
					{
						count++;
					}
				}

				if (tile != BoardTileType.Empty && count == this.size)
				{
					return this.WinnerForTile(tile.Value);
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the vertical winner
		/// </summary>
		private GameWinner? GetVerticalWinner()
		{
			for (int x = 0; x < this.size; x++)
			{
				BoardTileType? tile = null;
				int count = 0;

				for (int y = 0; y < this.size; y++)
				{
					if (tile == null)
					{
						tile = this.GetTile(x, y);
						count = 1;
						continue;
					}

					if (this.GetTile(x, y) == tile)
					{
						count++;
					}
				}

				if (tile != BoardTileType.Empty && count == this.size)
				{
					return this.WinnerForTile(tile.Value);
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the right diagonal winner
		/// </summary>
		private GameWinner? GetRightDiagonalWinner()
		{
			BoardTileType? tile = null;
			int count = 0;

			for (int x = 0, y = 0; x < this.size; x++, y++)
			{
				if (tile == null)
				{
					tile = this.GetTile(x, y);
					count = 1;
					continue;
				}

				if (this.GetTile(x, y) == tile)
				{
					count++;
				}
			}

			if (tile != BoardTileType.Empty && count == this.size)
			{
				return this.WinnerForTile(tile.Value);
			}

			return null;
		}

		/// <summary>
		/// Returns the left diagonal winner
		/// </summary>
		private GameWinner? GetLeftDiagonalWinner()
		{
			BoardTileType? tile = null;
			int count = 0;

			for (int x = this.size - 1, y = 0; x >= 0; x--, y++)
			{
				if (tile == null)
				{
					tile = this.GetTile(x, y);
					count = 1;
					continue;
				}

				if (this.GetTile(x, y) == tile)
				{
					count++;
				}
			}

			if (tile != BoardTileType.Empty && count == this.size)
			{
				return this.WinnerForTile(tile.Value);
			}

			return null;
		}

		/// <summary>
		/// Indicates if the player has won
		/// </summary>
		/// <param name="player">The player</param>
		private bool HasWon(BoardTileType player)
		{
			int pattern = 0;
			for (int y = 0; y < this.Size; ++y)
			{
				for (int x = 0; x < this.size; ++x)
				{
					if (this.GetTile(x, y) == player)
					{
						pattern |= (1 << (y * this.size + x));
					}
				}
			}

			foreach (int winningPattern in winningPatterns)
			{
				if ((pattern & winningPattern) == winningPattern)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns the winner
		/// </summary>
		/// <returns>The winner or null if not over</returns>
		public GameWinner? GetWinner()
		{
			if (this.HasWon(BoardTileType.Cross))
			{
				return GameWinner.Cross;
			}

			if (this.HasWon(BoardTileType.Circle))
			{
				return GameWinner.Circle;
			}

			if (this.IsFull())
			{
				return GameWinner.Tie;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Indicates if the given tile and winner are equal
		/// </summary>
		/// <param name="tile">The tile</param>
		/// <param name="winner">The winner</param>
		public static bool AreEqual(BoardTileType tile, GameWinner winner)
		{
			if (tile == BoardTileType.Circle && winner == GameWinner.Circle)
			{
				return true;
			}
			else if (tile == BoardTileType.Cross && winner == GameWinner.Cross)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns a random empty position
		/// </summary>
		/// <param name="random">The random generator</param>
		/// <returns>A random empty position or null if the board is full</returns>
		public BoardMove? RandomEmptyPosition(Random random)
		{
			if (this.IsFull())
			{
				return null;
			}

			int posX = 0;
			int posY = 0;

			while (true)
			{
				posX = random.Next(0, this.Size);
				posY = random.Next(0, this.Size);

				if (this.IsEmpty(posX, posY))
				{
					break;
				}
			}

			return new BoardMove(posX, posY);
		}
	}
}
