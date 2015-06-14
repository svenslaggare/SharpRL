using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning;
using SharpRL.Learning.Algorithms;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Examples.Maze
{
	/// <summary>
	/// The possible actions for the maze agent
	/// </summary>
	public enum MazeAction
	{
		Up,
		Down,
		Right,
		Left
	}

	/// <summary>
	/// Represents a maze agent
	/// </summary>
	public sealed class MazeAgent : Agent<MazeEnvironment, MazeAgent, MazeState>
	{
		private int numSteps = 0;

		/// <summary>
		/// Creates a new maze agent
		/// </summary>
		public MazeAgent()
		{

		}

		/// <summary>
		/// Returns the number of steps the agent has walked
		/// </summary>
		public int NumSteps
		{
			get { return this.numSteps; }
		}

		/// <summary>
		/// Returns the current agent
		/// </summary>
		protected override MazeAgent CurrentAgent
		{
			get { return this; }
		}

		/// <summary>
		/// Creates the learner
		/// </summary>
		protected override ILearningAlgorithm<MazeState> CreateLearner()
		{
			double alpha = 0.4;
			double gamma = 1.0;
			double epsilon = 0.4;
			int stopDecayAt = (int)(0.4 * this.Environment.Config.MaxEpisodes);

			var selectionPolicy = new EGreedy(
				epsilon,
				this.Environment.Config.Random,
				DecayHelpers.ConstantDecay(0, stopDecayAt, epsilon, 0));

			return QLearning<MazeState>.New(
				Enum.GetValues(typeof(MazeAction)).Length,
				selectionPolicy,
				alpha,
				gamma,
				this.Environment.Config.Random);
		}

		/// <summary>
		/// Resets the agent to the given episode number
		/// </summary>
		/// <param name="episode">The episode number</param>
		public override void Reset(int episode)
		{
			base.Reset(episode);
			this.numSteps = 0;
		}

		/// <summary>
		/// Updates the agent at the given time step
		/// </summary>
		/// <param name="timeStep">The current time step</param>
		/// <param name="episode">The current episode number</param>
		public override void Update(int timeStep, int episode)
		{
			base.Update(timeStep, episode);
			this.numSteps++;
		}
	}
}
