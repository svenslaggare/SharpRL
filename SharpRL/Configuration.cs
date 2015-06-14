using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRL
{
	/// <summary>
	/// Contains the configuration for the system
	/// </summary>
	public sealed class Configuration
	{
		private readonly IDictionary<string, object> values = new Dictionary<string, object>();
		private readonly int maxEpisodes;
		private readonly Random random;

		/// <summary>
		/// Creates a new configuration
		/// </summary>
		/// <param name="maxEpisodes">The maximum number of episodes</param>
		/// <param name="random">The random generator</param>
		public Configuration(int maxEpisodes, Random random  = null)
		{
			this.maxEpisodes = maxEpisodes;
			this.random = random ?? new Random();
			this.values.Add("MaxEpisodes", this.maxEpisodes);
			this.values.Add("Random", this.random);
		}

		/// <summary>
		/// Returns the number of episodes
		/// </summary>
		public int MaxEpisodes
		{
			get { return this.maxEpisodes; }
		}

		/// <summary>
		/// Returns the random generator
		/// </summary>
		public Random Random
		{
			get { return this.random; }
		}

		/// <summary>
		/// Adds the given key-value to the configuration
		/// </summary>
		/// <param name="key">The key</param>
		/// <param name="value">The value</param>
		public void Add(string key, object value)
		{
			this.values.Add(key, value);
		}

		/// <summary>
		/// Indicates if the configuration contains the given key
		/// </summary>
		/// <param name="key">The key</param>
		public bool ContainsKey(string key)
		{
			return this.values.ContainsKey(key);
		}

		/// <summary>
		/// Returns the value for the given key
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="key">The value</param>
		/// <returns>The value or the default value for the type</returns>
		public T Get<T>(string key)
		{
			object value;
			if (this.values.TryGetValue(key, out value))
			{
				return (T)value;
			}
			else
			{
				return default(T);
			}
		}
	}
}
