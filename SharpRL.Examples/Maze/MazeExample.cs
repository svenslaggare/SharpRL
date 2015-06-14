using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning;
using SharpRL.Learning.Algorithms;

namespace SharpRL.Examples.Maze
{
	/// <summary>
	/// Represents a maze example.
	/// The agent will try to find its way through a maze.
	/// </summary>
	public static class MazeExample
	{
		/// <summary>
		/// Runs the example
		/// </summary>
		public static void Run()
		{
			var tiles = new MazeTile[10, 10];
			tiles[0, 0] = MazeTile.Agent;
			tiles[2, 4] = MazeTile.Wall;
			tiles[3, 4] = MazeTile.Wall;
			tiles[4, 4] = MazeTile.Wall;
			tiles[5, 4] = MazeTile.Wall;
			tiles[9, 9] = MazeTile.Goal;

			var environment = new MazeEnvironment(new Configuration(200), tiles);
			var agent = new MazeAgent();
			environment.AddAgent(agent);
			environment.Initialize();

			for (int episode = 0; episode < environment.Config.MaxEpisodes; episode++)
			{
				environment.Reset(episode);

				for (int step = 0; step < 1000; step++)
				{
					if (environment.Update(episode))
					{
						break;
					}
				}

				Console.WriteLine(string.Format("Episode: {0} - {1} steps.", episode, agent.NumSteps));
			}

			PrintPath(environment, agent);
			Console.ReadLine();
		}

		/// <summary>
		/// Prints the best path
		/// </summary>
		private static void PrintPath(MazeEnvironment environment, MazeAgent agent)
		{
			var learner = agent.Learner as QLearning<MazeState>;
			var path = new HashSet<MazeState>();
			MazeState state = environment.GetDefaultState(agent);
			path.Add(state);

			while (true)
			{
				MazeAction action = (MazeAction)learner.SelectAction(state);

				switch (action)
				{
					case MazeAction.Up:
						state = state.Add(y: -1);
						break;
					case MazeAction.Down:
						state = state.Add(y: 1);
						break;
					case MazeAction.Right:
						state = state.Add(x: 1);
						break;
					case MazeAction.Left:
						state = state.Add(x: -1);
						break;;
				}

				path.Add(state);

				if (environment.Tiles[state.PositionX, state.PositionY] == MazeTile.Goal)
				{
					break;
				}
			}


			for (int y = 0; y < environment.Length; y++)
			{
				for (int x = 0; x < environment.Width; x++)
				{
					switch (environment.Tiles[x, y])
					{
						case MazeTile.Floor:
							if (path.Contains(new MazeState(x, y)))
							{
								Console.Write("x");
							}
							else
							{
								Console.Write(" ");
							}
							break;
						case MazeTile.Wall:
							Console.Write("W");
							break;
						case MazeTile.Agent:
							Console.Write("A");
							break;
						case MazeTile.Goal:
							Console.Write("G");
							break;
					}
				}

				Console.WriteLine();
			}
		}
	}
}
