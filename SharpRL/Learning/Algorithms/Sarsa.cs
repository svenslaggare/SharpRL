using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Learning.Algorithms
{
	/// <summary>
	/// Represents the SARSA algorithm
	/// </summary>
	/// <typeparam name="TState">The type of the state</typeparam>
	public sealed class Sarsa<TState> : ILearningAlgorithm<TState>
	{
		private readonly Random random;
		private readonly IActionSelectionPolicy selectionPolicy;
		private readonly IQValueTable<TState> qValueTable;
		private readonly double alpha;
		private readonly double gamma;

		private TState possibleState;
		private int possibleAction;

		/// <summary>
		/// Creates a new SARSA-learner
		/// </summary>
		/// <param name="qValueTable">The Q-value table</param>
		/// <param name="selectionPolicy">The selection policy</param>
		/// <param name="alpha">The learning rate</param>
		/// <param name="gamma">The discount factor</param>
		/// <param name="random">The random generator</param>
		public Sarsa(QValueTable<TState> qValueTable, IActionSelectionPolicy selectionPolicy, double alpha, double gamma, Random random)
		{
			if (alpha < 0)
			{
				throw new ArgumentException("Expected alpha to be >= 0.", "alpha");
			}

			if (gamma < 0)
			{
				throw new ArgumentException("Expected gamma to be >= 0.", "gamma");
			}

			this.qValueTable = qValueTable;
			this.selectionPolicy = selectionPolicy;
			this.alpha = alpha;
			this.gamma = gamma;
			this.random = random;
		}

		/// <summary>
		/// Creates a new SARSA-learner using the given selection policy
		/// </summary>
		/// <param name="numActions">The number of actions</param>
		/// <param name="selectionPolicy">The selection policy</param>
		/// <param name="alpha">The learning rate</param>
		/// <param name="gamma">The discount factor</param>
		/// <param name="random">The random generator</param>
		public static Sarsa<TState> New(int numActions, IActionSelectionPolicy selectionPolicy, double alpha, double gamma, Random random)
		{
			return new Sarsa<TState>(new QValueTable<TState>(numActions), selectionPolicy, alpha, gamma, random);
		}

		/// <summary>
		/// Indicates if the learner should follow the policy
		/// </summary>
		public bool FollowPolicy { get; set; }

		/// <summary>
		/// Returns the action to execute in the given state
		/// </summary>
		/// <param name="state">The state</param>
		public int SelectAction(TState state)
		{
			if (!this.FollowPolicy)
			{
				if (this.possibleState == null || !this.possibleState.Equals(state))
				{
					return this.selectionPolicy.Select(this.qValueTable[state]);
				}
				else
				{
					return this.possibleAction;
				}
			}
			else
			{
				return PolicyHelpers.SelectMax(this.qValueTable[state], this.random);
			}
		}

		/// <summary>
		/// Updates the learning algorithm
		/// </summary>
		/// <param name="currentState">The current state</param>
		/// <param name="newState">The new state</param>
		/// <param name="action">The action that was executed</param>
		/// <param name="reward">The reward that was received</param>
		public void Update(TState currentState, TState newState, int action, double reward)
		{
			this.possibleState = newState;
			this.possibleAction = this.SelectAction(newState);

			double nextValue = this.qValueTable[newState][this.possibleAction];
			double oldValue = this.qValueTable[currentState][action];
			double newValue = oldValue + (this.alpha + (reward + (gamma + nextValue) - oldValue));

			this.qValueTable[currentState, action] = newValue;
		}

		/// <summary>
		/// Called after the current episode is done
		/// </summary>
		/// <param name="episode">The episode number</param>
		public void AfterEpisode(int episode)
		{
			this.selectionPolicy.Update(episode);
		}
	}
}
