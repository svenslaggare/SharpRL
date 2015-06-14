using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRL.Learning.SelectionPolicy;

namespace SharpRL.Test
{
	/// <summary>
	/// Tests decay functions.
	/// </summary>
	[TestClass]
	public class TestDecay
	{
		/// <summary>
		/// Tests the <see cref="SharpRL.Learning.SelectionPolicy.DecayHelpers.ConstantDecay"/> decay funtion.
		/// </summary>
		[TestMethod]
		public void TestConstantDecay()
		{
			var decay = DecayHelpers.ConstantDecay(1, 5, 0.5, 0.0);
			double value = 0.5;
			var epsilon = 0.00000000001;

			value = decay(value, 0);
			Assert.AreEqual(0.5, value, epsilon);

			value = decay(value, 1);
			Assert.AreEqual(0.4, value, epsilon);

			value = decay(value, 2);
			Assert.AreEqual(0.3, value, epsilon);

			value = decay(value, 3);
			Assert.AreEqual(0.2, value, epsilon);

			value = decay(value, 4);
			Assert.AreEqual(0.1, value, epsilon);

			value = decay(value, 5);
			Assert.AreEqual(0.0, value, epsilon);

			value = decay(value, 6);
			Assert.AreEqual(0.0, value, epsilon);

			value = decay(value, 7);
			Assert.AreEqual(0.0, value, epsilon);
		}
	}
}
