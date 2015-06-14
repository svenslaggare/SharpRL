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
	public sealed class QValueTable<TState> : IQValueTable<TState>
	{
		private readonly IDictionary<TState, QValue> values = new Dictionary<TState, QValue>();
		private readonly int numActions;
		private readonly double defaultValue;

		/// <summary>
		/// Creates a new Q-value table
		/// </summary>
		/// <param name="numActions">The number of actions</param>
		/// <param name="defaultValue">The default value for entries in the table</param>
		public QValueTable(int numActions, double defaultValue = 0)
		{
			this.numActions = numActions;
			this.defaultValue = defaultValue;
		}

		/// <summary>
		/// Creates a new Q-value
		/// </summary>
		/// <param name="state">The state to create for</param>
		/// <returns>The created value</returns>
		private QValue CreateValue(TState state)
		{
			var values = new double[this.numActions];

			for (int i = 0; i < values.Length; i++)
			{
				values[i] = this.defaultValue;
			}

			var qValue = new QValue(values);
			this.values.Add(state, qValue);
			return qValue;
		}

		/// <summary>
		/// Sets the Q-value for the given action in the given state
		/// </summary>
		/// <param name="state">The state</param>
		/// <param name="action">The index of the action</param>
		public double this[TState state, int action]
		{
			set
			{
				var qValue = this[state];

				if (action < 0 || action >= this.numActions)
				{
					throw new ArgumentOutOfRangeException("action");
				}

				qValue[action] = value;
			}
		}

		/// <summary>
		/// Returns the Q-value for the given state
		/// </summary>
		/// <param name="state"></param>
		public QValue this[TState state]
		{
			get
			{
				QValue qValue;
				if (!this.values.TryGetValue(state, out qValue))
				{
					return this.CreateValue(state);
				}

				return qValue;
			}
		}

		/// <summary>
		/// Indicates if the table contains the given state
		/// </summary>
		/// <param name="state">The state</param>
		public bool Contains(TState state)
		{
			return values.ContainsKey(state);
		}
	}
}
