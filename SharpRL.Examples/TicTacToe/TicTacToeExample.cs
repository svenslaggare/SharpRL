using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents a Tic-tac-toe example.
	/// </summary>
	public static class TicTacToeExample
	{
		/// <summary>
		/// Runs the example
		/// </summary>
		public static void Run()
		{
			int trainingEpisodes = 100000;
			var environment = new BoardEnvironment(new Configuration(trainingEpisodes));

			var agent = new BoardAgent(BoardTileType.Cross);
			environment.AddAgent(agent);
			environment.TrainingStrategy = new MinimaxStrategy();
			environment.Initialize();

			//Train
			var startTime = DateTime.UtcNow;
			Simulate(environment, environment.Config.MaxEpisodes);
			Console.WriteLine("Training time: " + (DateTime.UtcNow - startTime));
	
			//Game
			environment.ResetScores();
			agent.Learner.FollowPolicy = true;

			int gameEpisodes = 200;
			Simulate(environment, gameEpisodes);

			Func<int, double> winRaito = wins => ((double)wins / gameEpisodes) * 100;

			Console.WriteLine("Cross wins: " + environment.CrossWins + " (" + winRaito(environment.CrossWins) + "%)");
			Console.WriteLine("Circle wins: " + environment.CircleWins + " (" + winRaito(environment.CircleWins) + "%)");
			Console.WriteLine("Ties: " + environment.Ties + " (" + winRaito(environment.Ties) + "%)");

			Console.ReadLine();
		}

		/// <summary>
		/// Starts a game
		/// </summary>
		[STAThread]
		public static void StartGame()
		{
			var form = new TicTacToeGame();
			form.ShowDialog();
		}

		/// <summary>
		/// Simulates the environment for the given amount of episodes
		/// </summary>
		/// <param name="environment">The environment</param>
		/// <param name="episodes">The number of episodes</param>
		private static void Simulate(BoardEnvironment environment, int episodes)
		{
			for (int episode = 0; episode < episodes; episode++)
			{
				environment.Reset(episode);

				while (true)
				{
					if (environment.Update(episode))
					{
						break;
					}
				}
			}
		}
	}
}
