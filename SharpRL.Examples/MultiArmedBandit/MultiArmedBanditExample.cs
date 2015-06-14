using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning.Algorithms;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Examples.MultiArmedBandit
{
	/// <summary>
	/// Represents a multi-armed bandit example.
	/// The player will try maximize the reward received by playing on machines that generates reward from an unknown probability distribution.
	/// </summary>
	public static class MultiArmedBanditExample
	{
		/// <summary>
		/// Runs the example
		/// </summary>
		public static void Run()
		{
			var slotMachines = new List<SlotMachine>();
			slotMachines.Add(new SlotMachine(20, 120));
			slotMachines.Add(new SlotMachine(5, 100));
			slotMachines.Add(new SlotMachine(40, 150));
			slotMachines.Add(new SlotMachine(25, 130));
			slotMachines.Add(new SlotMachine(25, 120));
			slotMachines.Add(new SlotMachine(60, 120));

			var random = new Random(1337);
			int trainingEpisodes = 10000;
			double decayRatio = 0.4;

			var environment = new MultiArmedBanditEnvironment(new Configuration(trainingEpisodes, random), slotMachines);
			var agent = new StatelessAgent<MultiArmedBanditEnvironment>(env =>
			{
				double alpha = 0.05;
				double gamma = 0.1;
				int stopDecayAt = (int)(decayRatio * env.Config.MaxEpisodes);

				double epsilon = 0.1;

				var selectionPolicy = new EGreedy(
					epsilon,
					env.Config.Random,
					DecayHelpers.ConstantDecay(0, stopDecayAt, epsilon, 0));

				return QLearning<EmptyState>.New(
					slotMachines.Count,
					selectionPolicy,
					alpha,
					gamma,
					env.Config.Random);
			});
			environment.AddAgent(agent);
			environment.Initialize();

			for (int episode = 0; episode < environment.Config.MaxEpisodes; episode++)
			{
				environment.Reset(episode);
				environment.Update(episode);
			}

			Console.WriteLine(string.Format("Total reward: {0}", environment.TotalReward));
			Console.ReadLine();
		}
	}
}
