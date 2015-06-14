using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRL.Learning;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Test
{
	/// <summary>
	/// Tests the <see cref="SharpRL.Learning.SelectionPolicy.Softmax"/> class.
	/// </summary>
	[TestClass]
	public class TestSoftmax
	{
		private class TestInstance
		{
			public int Action { get; set; }
			public int Count { get; set; }
		}

		/// <summary>
		/// Tests that the random selection actions are selected with the right probability
		/// </summary>
		[TestMethod]
		public void TestRandomProbability()
		{
			var random = new Random(1337);
			var tau = 200;
			var softmax = new Softmax(tau, random);
			var qValue = new QValue(new double[]
			{
				121, 231, 425, 676
			});
			var bestAction = PolicyHelpers.SelectMax(qValue, random);

			var numSelected = new TestInstance[qValue.Count];
			
			for (int i = 0; i < qValue.Count; i++)
			{
				numSelected[i] = new TestInstance() { Action = i };
			}

			int numTests = 3000;

			for (int i = 0; i < numTests; i++)
			{
				int action = softmax.Select(qValue);
				numSelected[action].Count++;
			}

			numSelected = numSelected.OrderBy(x => x.Count).ToArray();

			Assert.AreEqual(0, numSelected[0].Action);
			Assert.AreEqual(1, numSelected[1].Action);
			Assert.AreEqual(2, numSelected[2].Action);
			Assert.AreEqual(3, numSelected[3].Action);
		}

		/// <summary>
		/// Tests decaying the tau
		/// </summary>
		[TestMethod]
		public void TestDecay()
		{
			//var random = new Random();
			//var epsilon = 0.5;
			//var eGreedy = new EGreedy(epsilon, random, DecayHelpers.ConstantDecay(1, 5, 0.5, 0.0));
			//var qValue = new QValue(new double[]
			//{
			//	121, 231, 425, 676, 812, 1012, 1231, 1301, 1412, 1541, 1701, 2015
			//});

			//var valueEpsilon = 0.00000000001;

			//Assert.AreEqual(0.5, eGreedy.Epsilon, valueEpsilon);
			//eGreedy.Decay(1);

			//Assert.AreEqual(0.4, eGreedy.Epsilon, valueEpsilon);
			//eGreedy.Decay(2);

			//Assert.AreEqual(0.3, eGreedy.Epsilon, valueEpsilon);
			//eGreedy.Decay(3);

			//Assert.AreEqual(0.2, eGreedy.Epsilon, valueEpsilon);
			//eGreedy.Decay(4);

			//Assert.AreEqual(0.1, eGreedy.Epsilon, valueEpsilon);
			//eGreedy.Decay(5);

			//Assert.AreEqual(0.0, eGreedy.Epsilon, valueEpsilon);
			//eGreedy.Decay(6);

			//Assert.AreEqual(0.0, eGreedy.Epsilon, valueEpsilon);
			//eGreedy.Decay(7);

			//Assert.AreEqual(0.0, eGreedy.Epsilon, valueEpsilon);
		}
	}
}
