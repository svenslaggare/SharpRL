using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning
{
	/// <summary>
	/// Decays the given value
	/// </summary>
	/// <param name="value">The current value</param>
	/// <param name="episode">The episode</param>
	/// <returns>The decayed value</returns>
	public delegate double Decay(double value, int episode);

	/// <summary>
	/// Represents an action selection policy
	/// </summary>
	public interface IActionSelectionPolicy
	{
		/// <summary>
		/// Updates the selection policy
		/// </summary>
		/// <param name="episode">The episode number</param>
		void Update(int episode);

		/// <summary>
		/// Selects an action from the given Q-value
		/// </summary>
		/// <param name="value">The Q-value</param>
		/// <returns>The index of the action to execute</returns>
		int Select(QValue value);
	}
}
