using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.CatAndMouse
{
	/// <summary>
	/// Represents a tile in the cat and mouse world
	/// </summary>
	public enum CatAndMouseTile
	{
		Floor,
		Wall,
		Cat,
		Mouse
	}

	/// <summary>
	/// Represents a 2D point
	/// </summary>
	internal struct Point
	{
		/// <summary>
		/// The x component
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// The y component
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Creates a new point
		/// </summary>
		/// <param name="x">The x component</param>
		/// <param name="y">The y component</param>
		public Point(int x, int y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}

		public override string ToString()
		{
			return string.Format("\\{ {0}:{1}\\ }", this.X, this.Y);
		}
	}

	/// <summary>
	/// Represents a cat and mouse environment
	/// </summary>
	public sealed class CatAndMouseEnvironment : Environment<CatAndMouseEnvironment, MouseAgent, MouseState>
	{
		private readonly CatAndMouseTile[,] loadedTiles;
		private CatAndMouseTile[,] tiles;

		private Point mousePosition;
		private Point catPosition;
		private Point cheesePosition;

		/// <summary>
		/// The mouse score
		/// </summary>
		public int MouseScore { get; private set; }

		/// <summary>
		/// The cat score
		/// </summary>
		public int CatScore { get; private set; }

		private readonly double cheeseReward = 50;
		private readonly double deathPenalty = -100;

		/// <summary>
		/// Creates a new environment using the given tiles
		/// </summary>
		/// <param name="config">The config</param>
		/// <param name="tiles">The tiles</param>
		public CatAndMouseEnvironment(Configuration config, CatAndMouseTile[,] tiles)
			: base(config)
		{
			this.loadedTiles = tiles;
			this.tiles = new CatAndMouseTile[this.Width, this.Length];
		}

		/// <summary>
		/// Returns the tiles
		/// </summary>
		public CatAndMouseTile[,] Tiles
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
		protected override CatAndMouseEnvironment CurrentEnvironment
		{
			get { return this; }
		}

		/// <summary>
		/// Resets the scores
		/// </summary>
		public void ResetScores()
		{
			this.CatScore = 0;
			this.MouseScore = 0;
		}

		/// <summary>
		/// Returns the default state for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		public override MouseState GetDefaultState(MouseAgent agent)
		{
			return new MouseState(
				this.mousePosition.X,
				this.mousePosition.Y,
				this.catPosition.X,
				this.catPosition.Y,
				this.cheesePosition.X,
				this.cheesePosition.Y);
		}

		/// <summary>
		/// Returns a random free position
		/// </summary>
		private Point RandomFreePosition()
		{
			int posX = 0;
			int posY = 0;

			while (true)
			{
				posX = this.Config.Random.Next(0, this.Width);
				posY = this.Config.Random.Next(0, this.Length);

				if (this.tiles[posX, posY] == CatAndMouseTile.Floor)
				{
					break;
				}
			}

			return new Point(posX, posY);
		}

		/// <summary>
		/// Places the entities at random positions
		/// </summary>
		private void RandomPlacement()
		{
			this.mousePosition = this.RandomFreePosition();
			this.tiles[this.mousePosition.X, this.mousePosition.Y] = CatAndMouseTile.Mouse;

			this.catPosition = this.RandomFreePosition();
			this.tiles[this.catPosition.X, this.catPosition.Y] = CatAndMouseTile.Cat;

			this.cheesePosition = this.RandomFreePosition();
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

			this.RandomPlacement();
		}

		/// <summary>
		/// Indicates if the current state is terminal
		/// </summary>
		/// <returns></returns>
		private bool IsTerminal()
		{
			return this.catPosition.Equals(this.mousePosition);
		}

		/// <summary>
		/// Updates the environment
		/// </summary>
		/// <param name="episode">The current episode number</param>
		/// <returns>True if the current episode is over</returns>
		public override bool Update(int episode)
		{
			base.Update(episode);
			return this.IsTerminal();
		}

		/// <summary>
		/// Indicates if the given position is valid
		/// </summary>
		/// <param name="position">The position</param>
		/// <param name="allowedTarget">The allowed target type</param>
		private bool IsValid(Point position, CatAndMouseTile? allowedTarget = null)
		{
			if (position.X >= 0 && position.X < this.Width && position.Y >= 0 && position.Y < this.Length)
			{
				var targetType = this.tiles[position.X, position.Y];

				if (allowedTarget == null)
				{
					return targetType == CatAndMouseTile.Floor;
				}
				else
				{
					return targetType == CatAndMouseTile.Floor || targetType == allowedTarget;
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Reutrns a new position for the given position
		/// </summary>
		/// <param name="current">The position</param>
		/// <param name="target">The target</param>
		/// <param name="allowedTarget">The allowed target type</param>
		private Point GetNewPosition(Point current, Point target, CatAndMouseTile allowedTarget)
		{
			Point newPos = current;

			if (current.X != target.X)
			{
				newPos.X += (target.X - current.X) / Math.Abs(target.X - current.X);
			}

			if (current.Y != target.Y)
			{
				newPos.Y += (target.Y - current.Y) / Math.Abs(target.Y - current.Y);
			}

			//Check if the new position is valid
			if (this.IsValid(newPos, allowedTarget))
			{
				return newPos;
			}

			//Else make random move
			while (true)
			{
				newPos = current;
				newPos.X += this.Config.Random.Next(-1, 2);
				newPos.Y += this.Config.Random.Next(-1, 2);

				if (this.IsValid(newPos, allowedTarget))
				{
					return newPos;
				}
			}
		}

		/// <summary>
		/// Moves the cat
		/// </summary>
		private void MoveCat()
		{
			var prevPos = this.catPosition;
			this.catPosition = this.GetNewPosition(this.catPosition, this.mousePosition, CatAndMouseTile.Mouse);
			this.tiles[prevPos.X, prevPos.Y] = CatAndMouseTile.Floor;
			this.tiles[catPosition.X, catPosition.Y] = CatAndMouseTile.Cat;
		}

		/// <summary>
		/// Applies the given action to the given position
		/// </summary>
		/// <param name="position">The position</param>
		/// <param name="action">The action</param>
		private Point ApplyAction(Point position, MouseAction action)
		{
			Point newPos = position;

			switch (action)
			{
				case MouseAction.Up:
					newPos.Y--;
					break;
				case MouseAction.Down:
					newPos.Y++;
					break;
				case MouseAction.Right:
					newPos.X++;
					break;
				case MouseAction.Left:
					newPos.X--;
					break;
				case MouseAction.UpLeft:
					newPos.Y--;
					newPos.X--;
					break;
				case MouseAction.UpRight:
					newPos.Y--;
					newPos.X++;
					break;
				case MouseAction.DownLeft:
					newPos.Y++;
					newPos.X--;
					break;
				case MouseAction.DownRight:
					newPos.Y++;
					newPos.X++;
					break;
			}

			return newPos;
		}

		/// <summary>
		/// Calculates the reward for the mouse
		/// </summary>
		private double CaluclateReward()
		{
			if (this.mousePosition.Equals(this.cheesePosition))
			{
				return this.cheeseReward;
			}
			else if (this.mousePosition.Equals(this.catPosition))
			{
				return this.deathPenalty;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Executes the given action for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		/// <param name="actionNumber">The index of the action</param>
		/// <param name="episode">The current episode number</param>
		public override void Execute(MouseAgent agent, int actionNumber, int episode)
		{
			MouseAction action = (MouseAction)actionNumber;
			Point newPos = this.ApplyAction(this.mousePosition, action);
 
			//Check if the new position is valid
			if (this.IsValid(newPos))
			{
				//Move the mouse
				this.tiles[this.mousePosition.X, this.mousePosition.Y] = CatAndMouseTile.Floor;
				this.tiles[newPos.X, newPos.Y] = CatAndMouseTile.Mouse;
				this.mousePosition = newPos;
			}

			//Move the cat
			this.MoveCat();

			//Update the state
			MouseState newState = new MouseState(
				this.mousePosition.X,
				this.mousePosition.Y,
				this.catPosition.X,
				this.catPosition.Y,
				this.cheesePosition.X,
				this.cheesePosition.Y);
			agent.State = newState;

			//Update the score
			if (this.mousePosition.Equals(this.cheesePosition))
			{
				this.MouseScore++;
			}
			else if (this.catPosition.Equals(this.mousePosition))
			{
				this.CatScore++;
			}

			//Calculate the reward
			agent.Reward(this.CaluclateReward(), this.IsTerminal(), episode);

			//Relocate the cheese
			if (this.mousePosition.Equals(this.cheesePosition))
			{
				this.cheesePosition = this.RandomFreePosition();
			}
		}
	}
}
