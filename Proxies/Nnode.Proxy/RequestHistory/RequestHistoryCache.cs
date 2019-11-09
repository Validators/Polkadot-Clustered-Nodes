using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nnode.Proxy.RequestHistory
{
	/// <summary>
	/// Keeps a list of all requests. I synced with a "analytics" server for treatment.
	/// </summary>
	public class RequestHistoryCache
	{
		private readonly IMemoryCache cache;

		public RequestHistoryCache(IMemoryCache cache)
		{
			this.cache = cache;
		}

		public string keyCount = "requestsCount";
		public string key = "requests";

		public List<HttpRequestModel> GetRequests()
		{
			return cache.Get<List<HttpRequestModel>>(key) ?? new List<HttpRequestModel>();
		}

		public int GetRequestCount()
		{
			return cache.Get<int>(keyCount);
		}

		/// <summary>
		/// Removes the cache
		/// </summary>
		public void FlushList()
		{
			// TODO: Add locks while removing.
			cache.Remove(key);
			cache.Remove(keyCount);
		}

		public void SaveRequest(HttpRequestModel httpRequestModel)
		{
			var requestList = cache.GetOrCreate(key, empty => { return new List<HttpRequestModel>(); });

			requestList.Add(httpRequestModel);


			// TODO: Limit cache size

			cache.Set(key, requestList);
			cache.Set(keyCount, requestList.Count);
		}
	}
}
