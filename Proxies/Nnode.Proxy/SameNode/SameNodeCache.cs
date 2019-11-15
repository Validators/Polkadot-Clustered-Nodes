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

		public SameNodeRequest GetRequest(string cacheKey)
		{
			var key = "SameNode:" + cacheKey;

			var sameNodeRequest = cache.Get<SameNodeRequest>(key);

			return sameNodeRequest;
		}

		public void SaveRequest(string cacheKey, string host)
		{
			var key = "SameNode:" + cacheKey;
			var request = new SameNodeRequest { Host = host };

			// Keep in cache for this time, reset time if accessed.
			//
			var cacheEntryOptions = new MemoryCacheEntryOptions()
			  .SetSlidingExpiration(TimeSpan.FromMinutes(10));

			cache.Set(key, request, cacheEntryOptions);
		}
	}
}