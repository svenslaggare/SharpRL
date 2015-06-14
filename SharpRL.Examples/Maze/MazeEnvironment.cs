using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.Maze
{
	/// <summary>
	/// Represents a maze tile
	/// </summary>
	public enum MazeTile
	{
		Floor,
		Wall,
		Agent,
		Goal
	}

	/// <summary>
	/// Represents a maze environment
	/// </summary>
	public sealed class MazeEnvironment : Environment<MazeEnvironment, MazeAgent, MazeState>
	{
		private readonly MazeTile[,] loadedTiles;
		private MazeTile[,] tiles;

		/// <summary>
		/// Creates a new environment using the given tiles
		/// </summary>
		/// <param name="config">The config</param>
		/// <param name="tiles">The tiles</param>
		public MazeEnvironment(Configuration config, MazeTile[,] tiles)
			: base(config)
		{
			this.loadedTiles = tiles;
			this.tiles = new MazeTile[this.Width, this.Length];
		}

		/// <summary>
		/// Returns the tiles
		/// </summary>
		public MazeTile[,] Tiles
		{
			get { return this.loadedTiles; }
		}

		/// <summary>
		/// Returns the width of the world
		/// </summary>
		public int Width
		{
			get { return this.loadedTiles.GetLength(0); }
		}

		/// <summary>
		/// Returns the length of the world
		/// </summary>
		public int Length
		{
			get { return this.loadedTiles.GetLength(1); }
		}

		/// <summary>
		/// Returns the current environment
		/// </summary>
		protected override MazeEnvironment CurrentEnvironment
		{
			get { return this; }
		}

		/// <summary>
		/// Returns the default state for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		public override MazeState GetDefaultState(MazeAgent agent)
		{
			for (int y = 0; y < this.Length; y++)
			{
				for (int x = 0; x < this.Width; x++)
				{
					if (this.loadedTiles[x, y] == MazeTile.Agent)
					{
						return new MazeState(x, y);
					}
				}
			}

			return new MazeState();
		}

		/// <summary>
		/// Resets the environment to the given episode
		/// </summary>
		/// <param name="episode">The episode number</param>
		public override void Reset(int episode)
		{
			base.Reset(episode);

			for (int y = 0; y < this.Length; y++)
			{
				for (int x = 0; x < this.Width; x++)
				{
					this.tiles[x, y] = this.loadedTiles[x, y];
				}
			}
		}

		/// <summary>
		/// Updates the environment
		/// </summary>
		/// <param name="episode">The current episode number</param>
		/// <returns>True if the current episode is over</returns>
		public override bool Update(int episode)
		{
			base.Update(episode);

			foreach (var agent in this.Agents)
			{
				if (this.loadedTiles[agent.State.PositionX, agent.State.PositionY] != MazeTile.Goal)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Calculates the reward
		/// </summary>
		/// <param name="isTerminal">Indicates if the state is terminal</param>
		private double CalculateReward(bool isTerminal)
		{
			if (isTerminal)
			{
				return 100;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Executes the given action for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		/// <param name="actionNumber">The index of the action</param>
		/// <param name="episode">The current episode number</param>
		public override void Execute(MazeAgent agent, int actionNumber, int episode)
		{
			MazeAction action = (MazeAction)actionNumber;

			int newPosX = agent.State.PositionX;
			int newPosY = agent.State.PositionY;

			switch (action)
			{
				case MazeAction.Up:
					newPosY--;
					break;
				case MazeAction.Down:
					newPosY++;
					break;
				case MazeAction.Right:
					newPosX++;
					break;
				case MazeAction.Left:
					newPosX--;
					break;
			}

			//Check if the new position is valid
			bool valid = true;
			if ((newPosX >= 0 && newPosX < this.Width) && (newPosY >= 0 && newPosY < this.Length))
			{
				var newTileType = this.tiles[newPosX, newPosY];

				if (newTileType != MazeTile.Wall)
				{
					var newState = new MazeState(newPosX, newPosY);
					bool isTerminal = newTileType == MazeTile.Goal;

					//Update the world & agent
					this.tiles[agent.State.PositionX, agent.State.PositionY] = MazeTile.Floor;
					this.tiles[newPosX, newPosY] = MazeTile.Agent;
					agent.State = newState;

					//Calculate the reward
					agent.Reward(this.CalculateReward(isTerminal), isTerminal, episode);
				}
				else
				{
					valid = false;
				}
			}
			else
			{
				valid = false;
			}

			if (!valid)
			{
				agent.State = agent.State;
				agent.Reward(this.CalculateReward(false), false, episode);
			}
		}
	}
}
