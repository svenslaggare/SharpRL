using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning
{
	/// <summary>
	/// Represents a learning algorithm
	/// </summary>
	/// <typeparam name="TState">The type of the state</typeparam>
	public interface ILearningAlgorithm<TState>
	{
		/// <summary>
		/// Indicates if the learner should follow the policy
		/// </summary>
		bool FollowPolicy { get; set; }

		/// <summary>
		/// Returns the action to execute in the given state
		/// </summary>
		/// <param name="state">The state</param>
		int SelectAction(TState state);

		/// <summary>
		/// Updates the learning algorithm
		/// </summary>
		/// <param name="currentState">The current state</param>
		/// <param name="newState">The new state</param>
		/// <param name="action">The action that was executed</param>
		/// <param name="reward">The reward that was received</param>
		void Update(TState currentState, TState newState, int action, double reward);

		/// <summary>
		/// Called after the current episode is done
		/// </summary>
		/// <param name="episode">The episode number</param>
		void AfterEpisode(int episode);
	}
}
