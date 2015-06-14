using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRL.Examples.TicTacToe;

namespace SharpRL.Examples.Test.TicTacToe
{
	/// <summary>
	/// Tests the board
	/// </summary>
	[TestClass]
	public class TestBoard
	{
		/// <summary>
		/// Tests setting and getting tiles
		/// </summary>
		[TestMethod]
		public void TestSetAndGet()
		{
			var board = new Board();
			board.SetTile(1, 1, BoardTileType.Cross);
			Assert.AreEqual(BoardTileType.Cross, board.GetTile(1, 1));
		}

		[TestMethod]
		public void TestAll()
		{
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					var board = new Board();
					board.SetTile(x, y, BoardTileType.Cross);
					Assert.AreEqual(BoardTileType.Cross, board.GetTile(x, y));
				}
			}

			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					var board = new Board();
					board.SetTile(x, y, BoardTileType.Circle);
					Assert.AreEqual(BoardTileType.Circle, board.GetTile(x, y));
				}
			}
		}
	}
}
