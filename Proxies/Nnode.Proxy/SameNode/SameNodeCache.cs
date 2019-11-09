using Microsoft.Extensions.Caching.Memory;
using System;

namespace Nnode.Proxy.SameNode
{
	/// <summary>
	/// Keeps a list of requests with associated IP, API-Key, and node id to ensure they reach the same node. Are cached 10 minutes sliding.
	/// </summary>
	public class SameNodeCache
	{
		private readonly IMemoryCache cache;

		public SameNodeCache(IMemoryCache cache)
		{
			this.cache = cache;
		}

		public SameNodeRequest GetRequest(string ip)
		{
			var key = "SameNode:" + ip;

			var sameNodeRequest = cache.Get<SameNodeRequest>(key);

			return sameNodeRequest;
		}

		public void SaveRequest(string ip, string host)
		{
			//TODO: Validate IP

			var key = "SameNode:" + ip;
			var request = new SameNodeRequest { Host = host };

			// Keep in cache for this time, reset time if accessed.
			//
			var cacheEntryOptions = new MemoryCacheEntryOptions()
			  .SetSlidingExpiration(TimeSpan.FromMinutes(10));

			cache.Set(key, request, cacheEntryOptions);
		}
	}
}