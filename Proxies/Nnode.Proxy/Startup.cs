using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nnode.Proxy.Authentication;
using Nnode.Proxy.RequestHistory;
using Nnode.Proxy.SameNode;
using ProxyKit;

namespace Nnode.Proxy
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			Console.WriteLine("# Proxy application started.");

			// AppSettings.json
			//
			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

			services.AddMemoryCache();
			services.AddSingleton<SameNodeCache>();
			services.AddSingleton<RequestHistoryCache>();
			services.AddProxy();

		}

		// List of active blockchain nodes
		//
		public UpstreamHost[] nodes = {
			new UpstreamHost("http://IP.one", weight:1)
			,new UpstreamHost("http://IP.two", weight:1)
		};


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, SameNodeCache sameNodeList, RequestHistoryCache requestHistoryList)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// API Authentication
			//
			app.ConfigureApiAuthenticationHandler();

			// Same Node handler 
			// Always define it before any other middleware that needs the selected "NodeHost".
			//
			var roundRobin = new RoundRobin(nodes);
			app.ConfigureSameNodeHandler(sameNodeList, roundRobin);

			// Tracks requests in temporary memory
			//
			app.ConfigureRequestHistoryTracker(requestHistoryList);


			// Proxy of WebSocket connections
			//
			app.UseWebSockets();
			app.UseWhen(
				context => context.WebSockets.IsWebSocketRequest,
				appInner =>
				{
					appInner.UseWebSocketProxy( // TODO: track messages in websocket
						context =>
						{

							return new Uri("ws://" + context.Items["NodeHost"] + ":9944/");
						},
						options => options.AddXForwardedHeaders());

				});

			// Proxy of HTTP connections
			//
			app.UseWhen(
				context => !context.WebSockets.IsWebSocketRequest,
				appInner => appInner.RunProxy(async contextInner =>
				{
					var response = await
						contextInner
							.ForwardTo("http://" + contextInner.Items["NodeHost"] + ":9933/")
							.CopyXForwardedHeaders()
							.AddXForwardedHeaders()
							.Send();

					if (!response.IsSuccessStatusCode)
					{
						Console.WriteLine("### Exception: " + response.StatusCode.ToString());
					}
					return response;
				}));
		}
	}
}
