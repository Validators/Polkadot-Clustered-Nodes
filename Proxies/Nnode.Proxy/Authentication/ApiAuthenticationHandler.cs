using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nnode.Proxy.Authentication
{
	public static class ApiAuthenticationHandler
	{
		public static void ConfigureApiAuthenticationHandler(this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				if (context.Request.Headers.ContainsKey("ProjectApiKey"))
				{
					context.Items["ProjectApiKey"] = context.Request.Headers["ProjectApiKey"];
				}

				var apiKeyFromQuery = context.Request.Query["apiKey"].SingleOrDefault();
				if (apiKeyFromQuery != null)
					context.Items["ProjectApiKey"] = apiKeyFromQuery;


				await next();
			});
		}
	}
}
