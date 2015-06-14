using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning;
using SharpRL.Learning.Algorithms;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Examples.CatAndMouse
{
	/// <summary>
	/// The possible actions for the mouse agent
	/// </summary>
	public enum MouseAction
	{
		Up,
		Down,
		Right,
		Left,
		UpLeft,
		UpRight,
		DownLeft,
		DownRight
	}

	/// <summary>
	/// Represents a maze agent
	/// </summary>
	public sealed class MouseAgent : Agent<CatAndMouseEnvironment, MouseAgent, MouseState>
	{
		/// <summary>
		/// Creates a new mouse agent
		/// </summary>
		public MouseAgent()
		{

		}

		/// <summary>
		/// Returns the current agent
		/// </summary>
		protected override MouseAgent CurrentAgent
		{
			get { return this; }
		}

		/// <summary>
		/// Creates the learner
		/// </summary>
		protected override ILearningAlgorithm<MouseState> CreateLearner()
		{
			double alpha = 1;
			double gamma = 0.1;
			int stopDecayAt = (int)(0.9 * this.Environment.Config.MaxEpisodes);

			double epsilon = 0.4;

			var selectionPolicy = new EGreedy(
				epsilon,
				this.Environment.Config.Random,
				DecayHelpers.ConstantDecay(0, stopDecayAt, epsilon, 0));

			return QLearning<MouseState>.New(
				Enum.GetValues(typeof(MouseAction)).Length,
				selectionPolicy,
				alpha,
				gamma,
				this.Environment.Config.Random);
			//return Sarsa<MouseState>.New(
			//	Enum.GetValues(typeof(MouseAction)).Length,
			//	selectionPolicy,
			//	alpha,
			//	gamma,
			//	this.Environment.Config.Random);
		}
	}
}
