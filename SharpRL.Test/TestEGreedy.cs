using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRL.Learning;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Test
{
	/// <summary>
	/// Tests the <see cref="SharpRL.Learning.SelectionPolicy.EGreedy"/> class.
	/// </summary>
	[TestClass]
	public class TestEGreedy
	{
		/// <summary>
		/// Tests that the random selection actions are selected with the right probability
		/// </summary>
		[TestMethod]
		public void TestRandomProbability()
		{
			var random = new Random(1337);
			var epsilon = 0.4;
			var eGreedy = new EGreedy(epsilon, random);
			var qValue = new QValue(new double[]
			{
				121, 231, 425, 676, 812, 1012, 1231, 1301, 1412, 1541, 1701, 2015
			});
			var bestAction = PolicyHelpers.SelectMax(qValue, random);

			int numBestSelected = 0;
			int numTests = 3000;

			for (int i = 0; i < numTests; i++)
			{
				int action = eGreedy.Select(qValue);

				if (action == bestAction)
				{
					numBestSelected++;
				}
			}

			Assert.AreEqual((1 - epsilon) + epsilon * (1.0 / qValue.Count), numBestSelected / (double)numTests, 0.05);
		}

		/// <summary>
		/// Tests decaying the epsilon
		/// </summary>
		[TestMethod]
		public void TestDecay()
		{
			var random = new Random();
			var epsilon = 0.5;
			var eGreedy = new EGreedy(epsilon, random, DecayHelpers.ConstantDecay(1, 5, 0.5, 0.0));
			var qValue = new QValue(new double[]
			{
				121, 231, 425, 676, 812, 1012, 1231, 1301, 1412, 1541, 1701, 2015
			});

			var valueEpsilon = 0.00000000001;

			Assert.AreEqual(0.5, eGreedy.Epsilon, valueEpsilon);
			eGreedy.Update(1);

			Assert.AreEqual(0.4, eGreedy.Epsilon, valueEpsilon);
			eGreedy.Update(2);

			Assert.AreEqual(0.3, eGreedy.Epsilon, valueEpsilon);
			eGreedy.Update(3);

			Assert.AreEqual(0.2, eGreedy.Epsilon, valueEpsilon);
			eGreedy.Update(4);

			Assert.AreEqual(0.1, eGreedy.Epsilon, valueEpsilon);
			eGreedy.Update(5);

			Assert.AreEqual(0.0, eGreedy.Epsilon, valueEpsilon);
			eGreedy.Update(6);

			Assert.AreEqual(0.0, eGreedy.Epsilon, valueEpsilon);
			eGreedy.Update(7);

			Assert.AreEqual(0.0, eGreedy.Epsilon, valueEpsilon);
		}
	}
}
