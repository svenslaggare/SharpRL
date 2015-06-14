using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.CatAndMouse
{
	/// <summary>
	/// Represents the state of a mouse agent
	/// </summary>
	public struct MouseState
	{
		private readonly int positionX;
		private readonly int positionY;
		private readonly int catPositionX;
		private readonly int catPositionY;
		private readonly int cheesePositionX;
		private readonly int cheesePositionY;

		/// <summary>
		/// Creates a new mouse state
		/// </summary>
		/// <param name="positionX">The x position of the mouse</param>
		/// <param name="positionY">The y position of the mouse</param>
		/// <param name="catPositionX">The x position of the cat</param>
		/// <param name="catPositionX">The y position of the cat</param>
		/// <param name="cheesePositionX">The x position of the cheese</param>
		/// <param name="cheesePositionY">The y position of the cheese</param>
		public MouseState(int positionX, int positionY, int catPositionX, int catPositionY, int cheesePositionX, int cheesePositionY)
		{
			this.positionX = positionX;
			this.positionY = positionY;
			this.catPositionX = catPositionX;
			this.catPositionY = catPositionY;
			this.cheesePositionX = cheesePositionX;
			this.cheesePositionY = cheesePositionY;
		}

		/// <summary>
		/// Returns the x position of the mouse
		/// </summary>
		public int PositionX
		{
			get { return this.positionX; }
		}

		/// <summary>
		/// Returns the y position of the mouse
		/// </summary>
		public int PositionY
		{
			get { return this.positionY; }
		}

		/// <summary>
		/// Returns the x position of the cat
		/// </summary>
		public int CatPositionX
		{
			get { return this.catPositionX; }
		}

		/// <summary>
		/// Returns the y position of the cat
		/// </summary>
		public int CatPositionY
		{
			get { return this.catPositionY; }
		}

		/// <summary>
		/// Returns the x position of the chees
		/// </summary>
		public int CheesePositionX
		{
			get { return this.cheesePositionX; }
		}

		/// <summary>
		/// Returns the y position of the cheese
		/// </summary>
		public int CheesePositionY
		{
			get { return this.cheesePositionY; }
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MouseState))
			{
				return false;
			}

			MouseState other = (MouseState)obj;

			return
				(this.positionX == other.positionX && this.positionY == other.PositionY)
				&& (this.catPositionX == other.catPositionX && this.catPositionY == other.catPositionY)
				&& (this.cheesePositionX == other.cheesePositionX && this.cheesePositionY == other.cheesePositionY);
		}

		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			result = prime * result + this.positionX;
			result = prime * result + this.positionY;
			result = prime * result + this.catPositionX;
			result = prime * result + this.catPositionY;
			result = prime * result + this.cheesePositionX;
			result = prime * result + this.cheesePositionY;
			return result;
		}
	}
}
