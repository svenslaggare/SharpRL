using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning.SelectionPolicy
{
	/// <summary>
	/// Represents a Softmax action selection policy.
	/// The policy will choose random actions with the probability based on the action-value estimates.
	/// </summary>
	public sealed class Softmax : IActionSelectionPolicy
	{
		private double tau;
		private readonly Random random;
		private readonly Decay decayFn;

		/// <summary>
		/// Creates a new Softmax selection policy
		/// </summary>
		/// <param name="tau">The temperature</param>
		/// <param name="random">The random generator</param>
		/// <param name="decayFn">The decay function for epsilon. If null, epsilon is not decayed.</param>
		public Softmax(double tau, Random random, Decay decayFn = null)
		{
			if (tau < 0)
			{
				throw new ArgumentException("Expected tau to be >= 0.", "tau");
			}

			this.tau = tau;
			this.random = random;
			this.decayFn = decayFn;
		}

		/// <summary>
		/// Returns the Tau
		/// </summary>
		public double Tau
		{
			get { return this.tau; }
		}

		/// <summary>
		/// Updates the selection policy
		/// </summary>
		/// <param name="episode">The episode number</param>
		public void Update(int episode)
		{
			if (this.decayFn != null)
			{
				this.tau = this.decayFn(this.tau, episode);
			}
		}

		/// <summary>
		/// Selects an action from the given Q-value
		/// </summary>
		/// <param name="value">The Q-value</param>
		/// <returns>The index of the action to execute</returns>
		public int Select(QValue value)
		{
			double random = this.random.NextDouble();
			double lower = 0.0;
			double upper = 0.0;

			double sumExp = 0.0;
				
			//Calculate the sum of exponentials
			for (int i = 0; i < value.Count; i++)
			{
				sumExp += Math.Exp(GetExponent(value[i]));
			}

			//Select the action
			for (int i = 0; i < value.Count; i++)
			{
				lower = upper;
				upper += Math.Exp(GetExponent(value[i])) / sumExp;

				if (random >= lower && random < upper)
				{
					return i;
				}
			}

			return 0;
		}

		/// <summary>
		/// Returns the exponent in the softmax policy
		/// </summary>
		/// <param name="value">The value</param>
		private double GetExponent(double value)
		{
			if (value != 0.0)
			{
				return value / this.tau;
			}
			else
			{
				return 0.0;
			}
		}
	}
}
