using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL
{
	/// <summary>
	/// Represents an agent
	/// </summary>
	/// <typeparam name="TEnvironment">The type of the environment</typeparam>
	/// <typeparam name="TAgent">The type of the current agent</typeparam>
	/// <typeparam name="TState">The type of the state</typeparam>
	public interface IAgent<TEnvironment, TAgent, TState>
		where TEnvironment : IEnvironment<TEnvironment, TAgent, TState>
		where TAgent : IAgent<TEnvironment, TAgent, TState>
	{
		/// <summary>
		/// The state of the agent
		/// </summary>
		TState State { get; set; }

		/// <summary>
		/// Initializes the agent
		/// </summary>
		/// <param name="environment">The environment that the agent is added to</param>
		void Initialize(TEnvironment environment);

		/// <summary>
		/// Resets the agent to the given episode number
		/// </summary>
		/// <param name="episode">The episode number</param>
		void Reset(int episode);

		/// <summary>
		/// Updates the agent at the given time step
		/// </summary>
		/// <param name="timeStep">The current time step</param>
		/// <param name="episode">The current episode number</param>
		void Update(int timeStep, int episode);

		/// <summary>
		/// Rewards the agent after the agent has been updated
		/// </summary>
		/// <param name="reward">The reward given by the environment</param>
		/// <param name="isTerminal">Indicates the new state is terminal</param>
		/// <param name="episode">The current episode number</param>
		void Reward(double reward, bool isTerminal, int episode);
	}
}
