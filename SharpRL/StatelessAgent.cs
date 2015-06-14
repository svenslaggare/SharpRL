using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning;

namespace SharpRL
{
	/// <summary>
	/// Represents an agent without state
	/// </summary>
	/// <typeparam name="TEnvironment">The type of the environment</typeparam>
	public sealed class StatelessAgent<TEnvironment> : IAgent<TEnvironment, StatelessAgent<TEnvironment>, EmptyState>
		where TEnvironment : IEnvironment<TEnvironment, StatelessAgent<TEnvironment>, EmptyState>
	{
		private int action;
		private ILearningAlgorithm<EmptyState> learner;
		private TEnvironment environment;

		private readonly Func<TEnvironment, ILearningAlgorithm<EmptyState>> createLearnerFn;

		/// <summary>
		/// Creates a new stateless agent
		/// </summary>
		/// <param name="createLearnerFn">The function that creates the learner</param>
		public StatelessAgent(Func<TEnvironment, ILearningAlgorithm<EmptyState>> createLearnerFn)
		{
			this.createLearnerFn = createLearnerFn;
		}

		/// <summary>
		/// Returns the dummy state
		/// </summary>
		EmptyState IAgent<TEnvironment, StatelessAgent<TEnvironment>, EmptyState>.State
		{
			get { return new EmptyState(); }
			set { }
		}

		/// <summary>
		/// Returns the learner
		/// </summary>
		public ILearningAlgorithm<EmptyState> Learner
		{
			get { return this.learner; }
		}

		/// <summary>
		/// Initializes the agent
		/// </summary>
		/// <param name="environment">The environment that the agent is added to</param>
		public void Initialize(TEnvironment environment)
		{
			this.environment = environment;
			this.learner = this.createLearnerFn(environment);
		}

		/// <summary>
		/// Resets the agent to the given episode number
		/// </summary>
		/// <param name="episode">The episode number</param>
		public void Reset(int episode)
		{
			this.action = 0;
			this.learner.AfterEpisode(episode);
		}

		/// <summary>
		/// Updates the agent at the given time step
		/// </summary>
		/// <param name="timeStep">The current time step</param>
		/// <param name="episode">The current episode number</param>
		public void Update(int timeStep, int episode)
		{
			this.action = this.learner.SelectAction(new EmptyState());
			this.environment.Execute(this, this.action, episode);
		}

		/// <summary>
		/// Rewards the agent after the agent has been updated
		/// </summary>
		/// <param name="reward">The reward given by the environment</param>
		/// <param name="isTerminal">Indicates the new state is terminal</param>
		/// <param name="episode">The current episode number</param>
		public void Reward(double reward, bool isTerminal, int episode)
		{
			this.learner.Update(new EmptyState(), new EmptyState(), this.action, reward);
		}
	}
}
