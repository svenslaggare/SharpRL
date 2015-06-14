using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning.SelectionPolicy
{
	/// <summary>
	/// Represents an E-greedy action selection policy.
	/// The policy will choose the best action based on the Q-value with a probability of 1-epsilon
	/// and with the epsilon probability a random action will be selected.
	/// </summary>
	public sealed class EGreedy : IActionSelectionPolicy
	{
		private double epsilon;
		private readonly Random random;
		private readonly Decay decayFn;

		/// <summary>
		/// Creates a new E-greedy selection policy
		/// </summary>
		/// <param name="epsilon">The probability that a random action will be selected</param>
		/// <param name="random">The random generator</param>
		/// <param name="decayFn">The decay function for epsilon. If null, epsilon is not decayed.</param>
		public EGreedy(double epsilon, Random random, Decay decayFn = null)
		{
			if (epsilon < 0)
			{
				throw new ArgumentException("Expected epsilon to be >= 0.", "epsilon");
			}
			
			this.epsilon = epsilon;
			this.random = random;
			this.decayFn = decayFn;
		}

		/// <summary>
		/// Returns the epsilon
		/// </summary>
		public double Epsilon
		{
			get { return this.epsilon; }
		}

		/// <summary>
		/// Updates the selection policy
		/// </summary>
		/// <param name="episode">The episode number</param>
		public void Update(int episode)
		{
			if (this.decayFn != null)
			{
				this.epsilon = this.decayFn(this.epsilon, episode);
			}
		}

		/// <summary>
		/// Selects an action from the given Q-value
		/// </summary>
		/// <param name="value">The Q-value</param>
		/// <returns>The index of the action to execute</returns>
		public int Select(QValue value)
		{
			if (this.random.NextDouble() < this.epsilon)
			{
				//Random action
				return PolicyHelpers.SelectRandom(value, this.random);
			}
			else
			{
				//Best action
				return PolicyHelpers.SelectMax(value, this.random);
			}
		}
	}
}
