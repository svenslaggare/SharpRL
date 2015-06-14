using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning
{
	/// <summary>
	/// Represents a Q-value table
	/// </summary>
	/// <typeparam name="TState">The type of the state</typeparam>
	public interface IQValueTable<TState>
	{
		/// <summary>
		/// Sets the Q-value for the given action in the given state
		/// </summary>
		/// <param name="state">The state</param>
		/// <param name="actionNumber">The action number</param>
		double this[TState state, int actionNumber]
		{
			set;
		}

		/// <summary>
		/// Returns the Q-values for the given state
		/// </summary>
		/// <param name="state">The state</param>
		QValue this[TState state]
		{
			get;
		}

		/// <summary>
		/// Indicates if the table contains the given state
		/// </summary>
		/// <param name="state">The state</param>
		bool Contains(TState state);
	}
}
