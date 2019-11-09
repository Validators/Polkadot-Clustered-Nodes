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
				var ip = context.GetClientIp();

				var sameNodeRequest = sameNodeList.GetRequest(ip);
				string currentNodeHost = null;

				// Is Cached
				//
				if (sameNodeRequest != null)
				{
					currentNodeHost = sameNodeRequest.Host;
					//					Console.WriteLine(currentNodeHost + " cached from IP: " + ip);
				}
				else
				{
					currentNodeHost = roundRobin.Next().Uri.Host;
					sameNodeList.SaveRequest(ip, currentNodeHost);
					//					Console.WriteLine(currentNodeHost + " new from IP: " + ip);
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
