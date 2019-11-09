using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nnode.Proxy
{
	public class AppSettings
	{
		public int BlockchainId { get; set; }
		public string ApiUrl { get; set; }
		public string HmacClientId { get; set; }
		public string HmacClientSecret { get; set; }
	}
}
