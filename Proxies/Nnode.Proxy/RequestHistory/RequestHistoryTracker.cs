using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nnode.Proxy.RequestHistory
{
	public static class RequestHistoryTracker
	{
		public static void ConfigureRequestHistoryTracker(this IApplicationBuilder app, RequestHistoryCache requestHistoryList)
		{
			app.Use(async (context, next) =>
			{
				var method = context.Request.Method;
				var logRequest = true;

				if (context.Request.Path.Equals("/favicon.ico", StringComparison.InvariantCultureIgnoreCase))
					logRequest = false;


				if (logRequest && (method.ToLower() == "get" || method.ToLower() == "post"))
				{
					var ip = context.GetClientIp();

					var projectApiKey = Guid.Empty;
					Guid.TryParse(context.Items["ProjectApiKey"]?.ToString() ?? null, out projectApiKey);

					var isWebSocket = (context.Request.Headers?["upgrade"] ?? "") == "websocket";

					// Cloudflare SSL Proxy check
					//
					var forwardedProto = context.Request.Headers?["X-Forwarded-Proto"];
					var isHttps = false;
					if (forwardedProto.HasValue && forwardedProto.Value.Any())
					{
						isHttps = (forwardedProto.Value[0] ?? "") == "https";
					}else
					{
						isHttps = context.Request.IsHttps;
					}

					requestHistoryList.SaveRequest(new HttpRequestModel
					{
						ProjectApiKey = projectApiKey,
						NodeHost = context.Items["NodeHost"]?.ToString() ?? "",
						ContentLength = context.Request.ContentLength,
						Method = context.Request.Method,
						Utc = DateTime.UtcNow,
						Ip = ip,
						Uri = new Uri(context.Request.GetEncodedUrl()),
						IsWebsocket = isWebSocket,
						IsHttps = isHttps,
						Body = JsonConvert.SerializeObject(context.Request.Headers.ToList())
					});
				}

				// Call next middleware
				await next();
			});
		}
	}
}
