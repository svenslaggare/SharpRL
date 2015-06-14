using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning.SelectionPolicy
{
	/// <summary>
	/// Contains helper methods for selection policies
	/// </summary>
	public static class PolicyHelpers
	{
		/// <summary>
		/// Selects the best action from the given Q-value. 
		/// If there are more than one best value, a random one is choosen uniformly.
		/// </summary>
		/// <param name="value">The Q-value</param>
		/// <param name="random">The random generator</param>
		/// <returns>The index of the action</returns>
		public static int SelectMax(QValue value, Random random)
		{
			var best = new List<int>();
			var bestValue = double.MinValue;

			for (int i = 0; i < value.Count; i++)
			{
				double actionValue = value[i];

				if (actionValue > bestValue)
				{
					best.Clear();
					bestValue = actionValue;
					best.Add(i);
				}
				else if (actionValue == bestValue)
				{
					best.Add(i);
				}
			}

			return best[random.Next(0, best.Count)];
		}

		/// <summary>
		/// Selects a random action
		/// </summary>
		/// <param name="value">The Q-value</param>
		/// <param name="random">The random generator</param>
		/// <returns>The index of the action</returns>
		public static int SelectRandom(QValue value, Random random)
		{
			return random.Next(0, value.Count);
		}
	}
}
