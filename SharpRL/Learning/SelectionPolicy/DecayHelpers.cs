using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL.Learning.SelectionPolicy
{
	/// <summary>
	/// Contains decay functions
	/// </summary>
	public static class DecayHelpers
	{
		/// <summary>
		/// A constant decay function
		/// </summary>
		/// <param name="startEpisode">The episode to start decaying</param>
		/// <param name="stopEpisode">The episode to stop decaying. At this episode, value = stopValue.</param>
		/// <param name="startValue">The start value</param>
		/// <param name="stopValue">The stop value</param>
		/// <returns>The decay function</returns>
		public static Decay ConstantDecay(int startEpisode, int stopEpisode, double startValue, double stopValue)
		{
			int deltaEpisode = (stopEpisode + 1) - startEpisode;
			double deltaValue = stopValue - startValue;
			double delta = (deltaValue / deltaEpisode);

			return (value, episode) =>
			{
				if (episode >= startEpisode && episode < stopEpisode)
				{
					return value + delta;
				}
				else if (episode >= stopEpisode)
				{
					return stopValue;
				}
				else
				{
					return value;
				}
			};
		}
	}
}
