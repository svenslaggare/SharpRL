using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRL.Learning;
using SharpRL.Learning.Algorithms;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Examples.TicTacToe
{
	/// <summary>
	/// Represents a board agent
	/// </summary>
	public sealed class BoardAgent : Agent<BoardEnvironment, BoardAgent, BoardState>
	{
		private readonly BoardTileType placementTile;
		private int boardSize;

		/// <summary>
		/// Creates a new board agent
		/// </summary>
		/// <param name="placementTile">The placement tile</param>
		public BoardAgent(BoardTileType placementTile)
		{
			if (placementTile == BoardTileType.Empty)
			{
				throw new ArgumentException("The placement tile cannot be empty.");
			}

			this.placementTile = placementTile;
		}
		
		/// <summary>
		/// Returns the placement tile
		/// </summary>
		public BoardTileType PlacementTile
		{
			get { return this.placementTile; }
		}

		/// <summary>
		/// Returns the current agent
		/// </summary>
		protected override BoardAgent CurrentAgent
		{
			get { return this; }
		}

		/// <summary>
		/// Initializes the agent
		/// </summary>
		/// <param name="environment">The environment</param>
		public override void Initialize(BoardEnvironment environment)
		{
			this.boardSize = environment.Size;
			base.Initialize(environment);
		}

		/// <summary>
		/// Creates the learner
		/// </summary>
		protected override ILearningAlgorithm<BoardState> CreateLearner()
		{
			double alpha = 0.05;
			double gamma = 0.1;
			int stopDecayAt = (int)(0.4 * this.Environment.Config.MaxEpisodes);

			double epsilon = 0.4;
			var selectionPolicy = new EGreedy(
				epsilon,
				this.Environment.Config.Random,
				DecayHelpers.ConstantDecay(0, stopDecayAt, epsilon, 0));

			//double tau = 200;
			//var selectionPolicy = new Softmax(
			//	tau,
			//	this.Environment.Config.Random,
			//	DecayHelpers.ConstantDecay(0, stopDecayAt, tau, 0));

			//return QLearning<BoardState>.New(
			//	this.boardSize * this.boardSize,
			//	selectionPolicy,
			//	alpha,
			//	gamma,
			//	this.Environment.Config.Random);
			return Sarsa<BoardState>.New(
				this.boardSize * this.boardSize,
				selectionPolicy,
				alpha,
				gamma,
				this.Environment.Config.Random);
		}
	}
}
