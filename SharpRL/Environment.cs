using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL
{
	/// <summary>
	/// Represents a base implementation of the IEnvironment interface
	/// </summary>
	/// <typeparam name="TEnvironment">The type of the environment</typeparam>
	/// <typeparam name="TAgent">The type of the agent</typeparam>
	/// <typeparam name="TState">The type of the state</typeparam>
	public abstract class Environment<TEnvironment, TAgent, TState> : IEnvironment<TEnvironment, TAgent, TState>
		where TEnvironment : IEnvironment<TEnvironment, TAgent, TState>
		where TAgent : IAgent<TEnvironment, TAgent, TState>
	{
		private readonly Configuration config;
		private readonly IList<TAgent> agents = new List<TAgent>();
		private int timeStep = 0;

		/// <summary>
		/// Creates a new environment
		/// </summary>
		/// <param name="config">The config</param>
		public Environment(Configuration config)
		{
			this.config = config;
		}

		/// <summary>
		/// Returns the configuration
		/// </summary>
		public Configuration Config
		{
			get
			{
				return this.config;
			}
		}

		/// <summary>
		/// Returns the time step
		/// </summary>
		protected int TimeStep
		{
			get { return this.timeStep; }
		}

		/// <summary>
		/// Returns the agents
		/// </summary>
		protected IList<TAgent> Agents
		{
			get { return this.agents; }
		}

		/// <summary>
		/// Returns the current environment
		/// </summary>
		/// <remarks>This method should return 'this'.</remarks>
		protected abstract TEnvironment CurrentEnvironment
		{
			get;
		}

		/// <summary>
		/// Returns the default state for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		public abstract TState GetDefaultState(TAgent agent);

		/// <summary>
		/// Adds the given agent to the environment
		/// </summary>
		/// <param name="agent">The agent</param>
		public void AddAgent(TAgent agent)
		{
			this.agents.Add(agent);
		}

		/// <summary>
		/// Initializes the environment
		/// </summary>
		public virtual void Initialize()
		{
			foreach (var agent in this.agents)
			{
				agent.Initialize(this.CurrentEnvironment);
			}
		}

		/// <summary>
		/// Resets the environment to the given episode
		/// </summary>
		/// <param name="episode">The episode number</param>
		public virtual void Reset(int episode)
		{
			this.timeStep = 0;

			foreach (var agent in this.agents)
			{
				agent.Reset(episode);
			}
		}

		/// <summary>
		/// Updates the environment
		/// </summary>
		/// <param name="episode">The current episode number</param>
		/// <returns>True if the current episode is over</returns>
		public virtual bool Update(int episode)
		{
			foreach (var agent in this.agents)
			{
				agent.Update(this.timeStep, episode);
			}

			this.timeStep++;
			return false;
		}


		/// <summary>
		/// Executes the given action for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		/// <param name="actionNumber">The index of the action</param>
		/// <param name="episode">The current episode number</param>
		public abstract void Execute(TAgent agent, int actionNumber, int episode);
	}
}
