using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Util;

namespace SharpRL.Examples.MultiArmedBandit
{
	/// <summary>
	/// Represents a slot machine
	/// </summary>
	public sealed class SlotMachine
	{
		/// <summary>
		/// The standard deviation for the machine
		/// </summary>
		public double StandardDeviation { private set; get; }
		
		/// <summary>
		/// The mean for the machine
		/// </summary>
		public double Mean { private set; get; }

		/// <summary>
		/// Creates a new slot machine using the given normal distribution
		/// </summary>
		/// <param name="standardDeviation">The standard deviation</param>
		/// <param name="mean">The mean</param>
		public SlotMachine(double standardDeviation, double mean)
		{
			this.StandardDeviation = standardDeviation;
			this.Mean = mean;
		}
	}

	/// <summary>
	/// Represents a multi-armed bandit environment
	/// </summary>
	public sealed class MultiArmedBanditEnvironment : Environment<MultiArmedBanditEnvironment, StatelessAgent<MultiArmedBanditEnvironment>, EmptyState>
	{
		private readonly IList<SlotMachine> slotMachines;
		private readonly NormalRandomGenerator normalRandomGenerator;

		/// <summary>
		/// The total amount of reward received
		/// </summary>
		public double TotalReward { get; private set; }

		/// <summary>
		/// Creates a new multi-armed bandit environment using the given slot machines
		/// </summary>
		/// <param name="config">The config</param>
		/// <param name="slotMachines">The slot machines</param>
		public MultiArmedBanditEnvironment(Configuration config, IList<SlotMachine> slotMachines) 
			: base(config)
		{
			this.slotMachines = slotMachines;
			this.normalRandomGenerator = new NormalRandomGenerator(config.Random);
		}

		//Not used
		public override EmptyState GetDefaultState(StatelessAgent<MultiArmedBanditEnvironment> agent)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the current environment
		/// </summary>
		protected override MultiArmedBanditEnvironment CurrentEnvironment
		{
			get { return this; }
		}

		/// <summary>
		/// Resets the total reward
		/// </summary>
		public void ResetReward()
		{
			this.TotalReward = 0;
		}

		/// <summary>
		/// Calculates the reward for the given machine
		/// </summary>
		/// <param name="machine">The machine</param>
		private double CalculateReward(int machine)
		{
			var slotMachine = this.slotMachines[machine];
			return this.normalRandomGenerator.Next(slotMachine.StandardDeviation, slotMachine.Mean);
		}

		/// <summary>
		/// Executes the given action for the given agent
		/// </summary>
		/// <param name="agent">The agent</param>
		/// <param name="actionNumber">The index of the action</param>
		/// <param name="episode">The current episode number</param>
		public override void Execute(StatelessAgent<MultiArmedBanditEnvironment> agent, int actionNumber, int episode)
		{
			double reward = this.CalculateReward(actionNumber);
			this.TotalReward += reward;
			agent.Reward(reward, false, episode);
		}
	}
}
