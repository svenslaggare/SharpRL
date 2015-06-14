using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents a Tic-tac-toe game
	/// </summary>
	public partial class TicTacToeGame : Form
	{
		private PictureBox[,] tiles;
		private GameMode gameMode = GameMode.RL;
		private RLGame rlGame;
		private Game minimaxGame;
		private bool isOver = false;

		/// <summary>
		/// The game mode
		/// </summary>
		public enum GameMode { RL, Minimax }

		/// <summary>
		/// Represents a RL based game
		/// </summary>
		private class RLGame
		{
			public BoardAgent Agent { get; set; }
			public BoardEnvironment Environment { get; set; }
		}

		/// <summary>
		/// Creates a new Tic-tac-toe game
		/// </summary>
		public TicTacToeGame()
		{
			InitializeComponent();
			this.tiles = new PictureBox[3, 3];

			this.tiles[0, 0] = this.pos11Box;
			this.tiles[0, 1] = this.pos12Box;
			this.tiles[0, 2] = this.pos13Box;

			this.tiles[1, 0] = this.pos21Box;
			this.tiles[1, 1] = this.pos22Box;
			this.tiles[1, 2] = this.pos23Box;

			this.tiles[2, 0] = this.pos31Box;
			this.tiles[2, 1] = this.pos32Box;
			this.tiles[2, 2] = this.pos33Box;

			switch (this.gameMode)
			{
				case GameMode.RL:
					this.CreateRLGame();
					break;
				case GameMode.Minimax:
					this.CreateMinimaxGame();
					break;
			}

			this.StartGame();

			this.ResetButton.Click += (sender, e) =>
			{
				this.StartGame();
			};

			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					int posX = x;
					int posY = y;

					this.tiles[x, y].Click += (sender, e) =>
					{
						if (!this.isOver)
						{
							switch (this.gameMode)
							{
								case GameMode.RL:
									this.MakeRLMove(posX, posY);
									break;
								case GameMode.Minimax:
									this.MakeMinimaxMove(posX, posY);
									break;
							}
						}
					};
				}
			}
		}

		/// <summary>
		/// Creates a RL based game
		/// </summary>
		private void CreateRLGame()
		{
			this.rlGame = new RLGame();
			this.rlGame.Environment = new BoardEnvironment(new Configuration(50000));
			this.rlGame.Environment.TrainingStrategy = new MinimaxStrategy();
			this.rlGame.Agent = new BoardAgent(BoardTileType.Circle);
			this.rlGame.Environment.AddAgent(this.rlGame.Agent);
			this.rlGame.Environment.Initialize();

			this.Train(this.rlGame.Environment);

			this.rlGame.Agent.Learner.FollowPolicy = true;
			this.rlGame.Environment.TrainingStrategy = null;
		}

		/// <summary>
		/// Creates a minimax based game
		/// </summary>
		private void CreateMinimaxGame()
		{
			this.minimaxGame = new Game();
		}

		/// <summary>
		/// Starts the game
		/// </summary>
		private void StartGame()
		{
			this.isOver = false;
			this.StatusLabel.Text = "Playing.";

			switch (this.gameMode)
			{
				case GameMode.RL:
					this.rlGame.Environment.Reset(0);
					this.rlGame.Environment.Update(0);
					break;
				case GameMode.Minimax:
					this.minimaxGame = new Game();
					break;
			}

			this.DrawWorld();
		}

		/// <summary>
		/// Makes a move in a RL based game
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		private void MakeRLMove(int x, int y)
		{
			if (this.rlGame.Environment.Place(x, y, BoardTileType.Cross))
			{
				var winner = this.rlGame.Environment.IsOver();

				//Only make a move if the player did not win
				if (winner == null)
				{
					if (this.rlGame.Environment.Update(0))
					{
						winner = this.rlGame.Environment.IsOver();
					}
				}

				if (winner != null)
				{
					this.GameOver(winner.Value);
				}

				this.DrawWorld();
			}
		}

		/// <summary>
		/// Makes a move in a minimax based game
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		private void MakeMinimaxMove(int x, int y)
		{
			if (this.minimaxGame.Move(x, y))
			{
				var winner = this.minimaxGame.GetWinner();

				//Only make a move if the player did not win
				if (winner == null)
				{
					this.minimaxGame.ComputerMove();
					winner = this.minimaxGame.GetWinner();
				}

				if (winner != null)
				{
					this.GameOver(winner.Value);
				}

				this.DrawWorld();
			}
		}

		/// <summary>
		/// Marks that the game is over
		/// </summary>
		/// <param name="winner">The winner</param>
		private void GameOver(GameWinner winner)
		{
			this.isOver = true;

			switch (winner)
			{
				case GameWinner.Tie:
					this.StatusLabel.Text = "The game is over. It's a tie.";
					break;
				case GameWinner.Cross:
					this.StatusLabel.Text = "The game is over. Cross wins.";
					break;
				case GameWinner.Circle:
					this.StatusLabel.Text = "The game is over. Circle wins.";
					break;
			}
		}

		/// <summary>
		/// Trains the environment
		/// </summary>
		/// <param name="environment">The environment</param>
		private void Train(BoardEnvironment environment)
		{
			for (int episode = 0; episode < environment.Config.MaxEpisodes; episode++)
			{
				environment.Reset(episode);

				while (true)
				{
					if (environment.Update(episode))
					{
						break;
					}
				}
			}
		}

		/// <summary>
		/// Draws the world
		/// </summary>
		private void DrawWorld()
		{
			switch (this.gameMode)
			{
				case GameMode.Minimax:
					this.DrawWorld(this.minimaxGame.Board);
					break;
				case GameMode.RL:
					this.DrawWorld(this.rlGame.Environment.Board);
					break;
			}
		}

		/// <summary>
		/// Draws the world
		/// </summary>
		/// <param name="board">The board</param>
		private void DrawWorld(Board board)
		{
			foreach (var tile in board.GetTiles())
			{
				var pictureBox = this.tiles[tile.X, tile.Y];

				switch (tile.TileType)
				{
					case BoardTileType.Empty:
						pictureBox.Image = new Bitmap(Properties.Resources.Empty);
						break;
					case BoardTileType.Cross:
						pictureBox.Image = new Bitmap(Properties.Resources.Cross
							);
						break;
					case BoardTileType.Circle:
						pictureBox.Image = new Bitmap(Properties.Resources.Circle);
						break;
				}
			}
		}
	}
}
