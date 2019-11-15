using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nnode.Proxy.Authentication
{
	public static class ApiAuthenticationHandler
	{
		const string apiKey = "test";
		const string apiSecret = "test";

		public static void ConfigureApiAuthenticationHandler(this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				// Save ApiKey for later
				//
				var apiKey = "";
				if (context.Request.Headers.ContainsKey("ApiKey"))
				{
					apiKey = context.Request.Headers["ApiKey"];
				}

				// Support apikey in querystring (for websocket)
				//
				var apiKeyFromQuery = context.Request.Query["apiKey"].SingleOrDefault();
				if (apiKeyFromQuery != null)
					apiKey = apiKeyFromQuery;

				context.Items["ProjectApiKey"] = apiKey;

				var hash = "";
				if (context.Request.Headers.ContainsKey("Hash"))
				{
					hash = context.Request.Headers["Hash"];
				}

				string bodyContent = new StreamReader(context.Request.Body).ReadToEnd();
				context.Request.Body.Position = 0;

				if (!ComputeHash(apiSecret, bodyContent, hash))
				{
					context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
					Console.WriteLine("Access denied - HMAC Authentication failed. ApiKey or Hash in header may be missing. ApiKey: " + apiKey + " Hash: " + hash);
				}
				else
				{
					await next();
				}
			});
		}

		/// <summary>
		/// Returns true if the server can generate the same hash as the one the client provided.
		/// </summary>
		/// <param name="sharedSecret"></param>
		/// <param name="body"></param>
		/// <param name="clientHash"></param>
		/// <returns></returns>
		private static bool ComputeHash(string sharedSecret, string body, string clientHash)
		{
			string hashString;
			var key = Encoding.UTF8.GetBytes(sharedSecret);
			using (var hmac = new HMACSHA512(key))
			{
				byte[] hashmessage = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
				hashString = ToHex(hashmessage);
			}

			return hashString.Equals(clientHash, StringComparison.InvariantCultureIgnoreCase);

		}
		/// <summary>
		/// Just a little bit faster according to SO
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToHex(byte[] bytes)
		{
			char[] c = new char[bytes.Length * 2];

			byte b;

			for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
			{
				b = ((byte)(bytes[bx] >> 4));
				c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

				b = ((byte)(bytes[bx] & 0x0F));
				c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
			}

			return new string(c);
		}

	}
}
