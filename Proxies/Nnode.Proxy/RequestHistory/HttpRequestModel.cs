using System;

namespace Nnode.Proxy.RequestHistory
{
	public class HttpRequestModel
	{
		public long? ContentLength { get; set; }
		public DateTime Utc { get; set; }
		public string Ip { get; set; }
		public string NodeHost { get; set; }
		public Guid ProjectApiKey { get; set; }
		public string Method { get; set; }
		public Uri Uri { get; set; }
		public string Body { get; set; }
		public bool IsWebsocket { get; set; }
		public bool IsHttps { get; set; }
	}
}