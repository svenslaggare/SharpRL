using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL
{
	/// <summary>
	/// Represents an environment
	/// </summary>
	/// <typeparam name="TEnvironment">The type of the environment</typeparam>
	/// <typeparam name="TAgent">The type of the agent</typeparam>
	/// <typeparam name="TState">The type of the state</typeparam>
	public interface IEnvironment<TEnvironment, TAgent, TState>
		where TEnvironment : IEnvironment<TEnvironment, TAgent, TState>
		where TAgent : IAgent<TEnvironment, TAgent, TState>
	{
		/// <summary>
		/// Returns the configuration
		/// </summary>
		Configuration Config { get; }

		/// <summary>
		/// Adds the given agent to the environment
		/// </summary>
		/// <param name="agent">The agent</param>
		void AddAgent(TAgent agent);

		/// <summary>
		/// Initializes the environment
		/// </summary>
		void Initialize();

		/// <summary>
		/// Returns the default state for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		TState GetDefaultState(TAgent agent);

		/// <summary>
		/// Resets the environment to the given episode
		/// </summary>
		/// <param name="episode">The episode number</param>
		void Reset(int episode);

		/// <summary>
		/// Updates the environment
		/// </summary>
		/// <param name="episode">The current episode number</param>
		/// <returns>True if current episode is over</returns>
		bool Update(int episode);

		/// <summary>
		/// Executes the given action for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		/// <param name="action">The index of action to execute</param>
		/// <param name="episode">The current episode number</param>
		void Execute(TAgent agent, int action, int episode);
	}
}
