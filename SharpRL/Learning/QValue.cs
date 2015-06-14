using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning
{
	/// <summary>
	/// Represents a Q-value
	/// </summary>
	public struct QValue
	{
		private readonly double[] values;

		/// <summary>
		/// Creates a new Q-value
		/// </summary>
		/// <param name="values">The values</param>
		public QValue(double[] values)
		{
			this.values = values;
		}

		/// <summary>
		/// Returns the number of values
		/// </summary>
		public int Count
		{
			get { return this.values.Length; }
		}

		/// <summary>
		/// The value at the given index
		/// </summary>
		/// <param name="index">The index</param>
		public double this[int index]
		{
			get { return this.values[index]; }
			set { this.values[index] = value; }
		}
	}
}
