using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning;

namespace SharpRL
{
	/// <summary>
	/// Represents a base implementation of the Agent interface
	/// </summary>
	/// <typeparam name="TEnvironment">The type of the environment</typeparam>
	/// <typeparam name="TAgent">The type of the current agent</typeparam>
	/// <typeparam name="TState">The type of the state</typeparam>
	public abstract class Agent<TEnvironment, TAgent, TState> : IAgent<TEnvironment, TAgent, TState>
		where TEnvironment : IEnvironment<TEnvironment, TAgent, TState>
		where TAgent : IAgent<TEnvironment, TAgent, TState>
	{
		private TState prevState;
		private TState state;
		private int action;
		private ILearningAlgorithm<TState> learner;
		private TEnvironment environment;

		/// <summary>
		/// The state of the agent
		/// </summary>
		public TState State
		{
			get { return this.state; }
			set
			{
				this.prevState = this.state;
				this.state = value;
			}
		}

		/// <summary>
		/// Returns the previous state
		/// </summary>
		public TState PrevState
		{
			get { return this.prevState; }
		}

		/// <summary>
		/// Returns the learner
		/// </summary>
		public ILearningAlgorithm<TState> Learner
		{
			get { return this.learner; }
		}

		/// <summary>
		/// Returns the last action that was taken
		/// </summary>
		protected int Action
		{
			get { return this.action; }
		}

		/// <summary>
		/// Returns the current agent
		/// </summary>
		/// <remarks>This method should return 'this'.</remarks>
		protected abstract TAgent CurrentAgent
		{
			get;
		}

		/// <summary>
		/// Returns the environment
		/// </summary>
		protected TEnvironment Environment
		{
			get { return this.environment; }
		}

		/// <summary>
		/// Creates the learner
		/// </summary>
		protected abstract ILearningAlgorithm<TState> CreateLearner();

		/// <summary>
		/// Initializes the agent
		/// </summary>
		/// <param name="environment">The environment that the agent is added to</param>
		public virtual void Initialize(TEnvironment environment)
		{			
			this.environment = environment;
			this.learner = this.CreateLearner();
		}

		/// <summary>
		/// Resets the agent to the given episode number
		/// </summary>
		/// <param name="episode">The episode number</param>
		public virtual void Reset(int episode)
		{
			this.action = 0;
			var defaultState = this.environment.GetDefaultState(this.CurrentAgent);
			this.prevState = defaultState;
			this.state = defaultState;
			this.learner.AfterEpisode(episode);
		}

		/// <summary>
		/// Updates the agent at the given time step
		/// </summary>
		/// <param name="timeStep">The current time step</param>
		/// <param name="episode">The current episode number</param>
		public virtual void Update(int timeStep, int episode)
		{
			this.action = this.learner.SelectAction(this.state);
			this.environment.Execute(this.CurrentAgent, this.action, episode);
		}

		/// <summary>
		/// Rewards the agent after the agent has been updated
		/// </summary>
		/// <param name="reward">The reward given by the environment</param>
		/// <param name="isTerminal">Indicates the new state is terminal</param>
		/// <param name="episode">The current episode number</param>
		public void Reward(double reward, bool isTerminal, int episode)
		{
			this.learner.Update(this.prevState, this.state, this.action, reward);
		}
	}
}
