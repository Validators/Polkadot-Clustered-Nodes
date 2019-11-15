using Microsoft.AspNetCore.Builder;
using ProxyKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nnode.Proxy.SameNode
{
	public static class SameNodeHandler
	{
		public static void ConfigureSameNodeHandler(this IApplicationBuilder app, SameNodeCache sameNodeList, RoundRobin roundRobin)
		{
			app.Use(async (context, next) =>
			{
				// Getting ApiKey from the request stream (defined in ApiAuthenticationHandler)
				//
				var apiKey = context.Items["ProjectApiKey"];
				var ip = context.GetClientIp();

				var cacheKey = string.Format("{0}-{1}", apiKey, ip);

				var sameNodeRequest = sameNodeList.GetRequest(cacheKey);
				string currentNodeHost = null;

				// Is Cached
				//
				if (sameNodeRequest != null)
				{
					currentNodeHost = sameNodeRequest.Host;
				}
				else
				{
					currentNodeHost = roundRobin.Next().Uri.Host;
					sameNodeList.SaveRequest(cacheKey, currentNodeHost);
				}

				// Save in request stream for next middleware
				//
				context.Items["NodeHost"] = currentNodeHost;

				// Call the next delegate/middleware in the pipeline
				await next();
			});
		}
	}
}
