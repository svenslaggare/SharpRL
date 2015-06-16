using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents a board environment
	/// </summary>
	public sealed class BoardEnvironment : Environment<BoardEnvironment, BoardAgent, BoardState>
	{
		private readonly Board board;
		private bool isOver = false;

		/// <summary>
		/// The strategy that the agent will be trained against
		/// </summary>
		public IAIStrategy TrainingStrategy { get; set; }

		/// <summary>
		/// The number of cross wins
		/// </summary>
		public int CrossWins { get; private set; }

		/// <summary>
		/// The number of circle wins
		/// </summary>
		public int CircleWins { get; private set; }

		/// <summary>
		/// The number of ties
		/// </summary>
		public int Ties { get; private set; }

		/// <summary>
		/// Creates a new environment
		/// </summary>
		/// <param name="config">The config</param>
		/// <param name="size">The size of the board</param>
		public BoardEnvironment(Configuration config, int size = 3)
			: base(config)
		{
			this.board = new Board(size);
			this.TrainingStrategy = new RandomStrategy(config.Random);
		}

		/// <summary>
		/// Returns the size of the board
		/// </summary>
		public int Size
		{
			get { return this.board.Size; }
		}

		/// <summary>
		/// Returns the current environment
		/// </summary>
		protected override BoardEnvironment CurrentEnvironment
		{
			get { return this; }
		}

		/// <summary>
		/// Returns the board
		/// </summary>
		public Board Board
		{
			get { return this.board; }
		}

		/// <summary>
		/// Returns the default state for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		public override BoardState GetDefaultState(BoardAgent agent)
		{
			return new BoardState(this.board);
		}

		/// <summary>
		/// Resets the scores
		/// </summary>
		public void ResetScores()
		{
			this.CrossWins = 0;
			this.CircleWins = 0;
			this.Ties = 0;
		}

		/// <summary>
		/// Removes the given agent
		/// </summary>
		/// <param name="agent">The agent to remove</param>
		public void RemoveAgent(BoardAgent agent)
		{
			this.Agents.Remove(agent);
		}

		/// <summary>
		/// Resets the environment to the given episode
		/// </summary>
		/// <param name="episode">The episode number</param>
		public override void Reset(int episode)
		{
			base.Reset(episode);
			this.board.Reset();
			this.isOver = false;
		}

		/// <summary>
		/// Updates the environment
		/// </summary>
		/// <param name="episode">The current episode number</param>
		/// <returns>True if the current episode is over</returns>
		public override bool Update(int episode)
		{
			base.Update(episode);
			return this.board.GetWinner().HasValue;
		}

		/// <summary>
		/// Returns the move position for the given action
		/// </summary>
		/// <param name="actionNumber">The action number</param>
		private BoardMove GetMove(int actionNumber)
		{
			return new BoardMove(actionNumber % this.Size, actionNumber / this.Size);
		}

		/// <summary>
		/// Calculates the reward
		/// </summary>
		/// <param name="winner">The winner or null</param>
		/// <param name="agent">The agent</param>
		private double CalculateReward(GameWinner? winner, BoardAgent agent)
		{
			if (winner.HasValue)
			{
				if (winner == GameWinner.Tie)
				{
					//Tie
					return -50;
				}
				else
				{
					if (Board.AreEqual(agent.PlacementTile, winner.Value))
					{
						//Win
						return 50;
					}
					else
					{
						//Lose
						return -50;
					}
				}
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Places the given tile at the given position
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="tile">The tile to place</param>
		/// <returns>True if placed else false</returns>
		public bool Place(int x, int y, BoardTileType tile)
		{
			if (this.board.Move(x, y, tile))
			{
				foreach (var agent in this.Agents)
				{
					agent.State = new BoardState(this.board);
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Checks if the game is over
		/// </summary>
		/// <returns>The winner or null</returns>
		public GameWinner? IsOver()
		{
			var winner = this.board.GetWinner();

			if (winner != null)
			{
				return winner;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Executes the given action for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		/// <param name="actionNumber">The index of the action</param>
		/// <param name="episode">The current episode number</param>
		public override void Execute(BoardAgent agent, int actionNumber, int episode)
		{
			if (!this.isOver)
			{
				var actionPosition = this.GetMove(actionNumber);
				GameWinner? winner = null;
				bool madeInvalidMode = false;

				if (!this.board.Move(actionPosition.X, actionPosition.Y, agent.PlacementTile))
				{
					//If the agent tried to make an invalid move, make a random move. 
					var randPosition = this.board.RandomEmptyPosition(this.Config.Random);

					if (randPosition != null)
					{
						this.board.Move(randPosition.Value.X, randPosition.Value.Y, agent.PlacementTile);
						madeInvalidMode = true;
					}
				}

				winner = this.board.GetWinner();

				//Only make a move if the agent didn't win.
				if (winner == null)
				{
					if (this.TrainingStrategy != null && !this.board.IsFull())
					{
						var opponentPlayer = agent.PlacementTile == BoardTileType.Circle
							? BoardTileType.Cross : BoardTileType.Circle;

						var move = this.TrainingStrategy.MakeMove(
							this.board,
							opponentPlayer);

						this.board.Move(move.X, move.Y, opponentPlayer);
						winner = this.board.GetWinner();
					}
				}

				//Update the state
				agent.State = new BoardState(this.board);

				if (!madeInvalidMode)
				{
					agent.Reward(this.CalculateReward(winner, agent), winner.HasValue, episode);
				}

				if (winner != null)
				{
					this.isOver = true;
					switch (winner.Value)
					{
						case GameWinner.Tie:
							this.Ties++;
							break;
						case GameWinner.Cross:
							this.CrossWins++;
							break;
						case GameWinner.Circle:
							this.CircleWins++;
							break;
					}
				}
			}
		}
	}
}
