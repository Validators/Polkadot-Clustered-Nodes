using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nnode.Proxy
{
	public static class HttpContextExtensions
	{
		/// <summary>
		/// Supports Cloudflare Origin IP header
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static string GetClientIp(this HttpContext context)
		{
			var request = context.Request;

			if (context.Request.Headers.ContainsKey("CF-Connecting-IP"))
			{
				return request.Headers["CF-Connecting-IP"][0] ?? "";
			}

			return context.Connection?.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";
		}
	}
}
