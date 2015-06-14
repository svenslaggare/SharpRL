using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Examples.CatAndMouse
{
	/// <summary>
	/// Represents a cat and mouse example.
	/// The mouse will try to get the cheese but the same time tries to avoid the cat.
	/// </summary>
	public static class CatAndMouseExample
	{
		/// <summary>
		/// Runs the example
		/// </summary>
		public static void Run()
		{
			//var tiles = new CatAndMouseTile[10, 10];
			//tiles[2, 4] = CatAndMouseTile.Wall;
			//tiles[3, 4] = CatAndMouseTile.Wall;
			//tiles[4, 4] = CatAndMouseTile.Wall;
			//tiles[5, 4] = CatAndMouseTile.Wall;
			//tiles[5, 5] = CatAndMouseTile.Wall;
			//tiles[5, 6] = CatAndMouseTile.Wall;
			//tiles[5, 7] = CatAndMouseTile.Wall;
			//tiles[6, 7] = CatAndMouseTile.Wall;
			//tiles[7, 7] = CatAndMouseTile.Wall;
			//tiles[8, 7] = CatAndMouseTile.Wall;

			var tiles = new CatAndMouseTile[5, 5];
			tiles[2, 2] = CatAndMouseTile.Wall;
			tiles[3, 2] = CatAndMouseTile.Wall;
			tiles[1, 2] = CatAndMouseTile.Wall;
			tiles[2, 3] = CatAndMouseTile.Wall;
			tiles[2, 1] = CatAndMouseTile.Wall;

			var environment = new CatAndMouseEnvironment(new Configuration(500000), tiles);
			var agent = new MouseAgent();
			environment.AddAgent(agent);
			environment.Initialize();

			DrawWorld(environment);

			//Train
			Simulate(environment, environment.Config.MaxEpisodes);
			
			//Now for real
			environment.ResetScores();
			agent.Learner.FollowPolicy = true;
			Simulate(environment, 10000);

			Console.WriteLine(string.Format("Cat: {0} - Mouse: {1}.", environment.CatScore, environment.MouseScore));
			Console.WriteLine("Win ratio: " + ((environment.MouseScore) / (double)(environment.MouseScore + environment.CatScore)) * 100 + "%");

			Console.ReadLine();
		}

		/// <summary>
		/// Simulates the environment
		/// </summary>
		/// <param name="environment">The environment</param>
		/// <param name="numEpisodes">The number of episodes</param>
		private static void Simulate(CatAndMouseEnvironment environment, int numEpisodes)
		{
			for (int episode = 0; episode < numEpisodes; episode++)
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

		/// <summary>
		/// Draws the world
		/// </summary>
		/// <param name="environment">The environment</param>
		private static void DrawWorld(CatAndMouseEnvironment environment)
		{
			for (int y = 0; y < environment.Length; y++)
			{
				for (int x = 0; x < environment.Width; x++)
				{
					switch (environment.Tiles[x, y])
					{
						case CatAndMouseTile.Floor:
							Console.Write(" ");
							break;
						case CatAndMouseTile.Wall:
							Console.Write("W");
							break;
					}
				}

				Console.WriteLine();
			}
		}
	}
}
