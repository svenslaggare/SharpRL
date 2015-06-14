using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.Maze
{
	/// <summary>
	/// Represents the state of a maze agent
	/// </summary>
	public struct MazeState
	{
		private readonly int positionX;
		private readonly int positionY;

		/// <summary>
		/// Creates a new maze state
		/// </summary>
		/// <param name="positionX">The x position</param>
		/// <param name="positionY">The y position</param>
		public MazeState(int positionX, int positionY)
		{
			this.positionX = positionX;
			this.positionY = positionY;
		}

		/// <summary>
		/// Returns the x position
		/// </summary>
		public int PositionX
		{
			get { return this.positionX; }
		}

		/// <summary>
		/// Returns the y position
		/// </summary>
		public int PositionY
		{
			get { return this.positionY; }
		}

		/// <summary>
		/// Adds the given amount to the current state and returns a new state
		/// </summary>
		/// <param name="x">The amount to add to the x position</param>
		/// <param name="y">The amount to add to the y position</param>
		/// <returns>The new state with the modified values</returns>
		public MazeState Add(int x = 0, int y = 0)
		{
			return new MazeState(this.positionX + x, this.positionY + y);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MazeState))
			{
				return false;
			}

			MazeState other = (MazeState)obj;

			return 
				this.positionX == other.positionX
				&& this.positionY == other.PositionY;
		}

		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			result = prime * result + this.positionX;
			result = prime * result + this.positionY;
			return result;
		}
	}
}
