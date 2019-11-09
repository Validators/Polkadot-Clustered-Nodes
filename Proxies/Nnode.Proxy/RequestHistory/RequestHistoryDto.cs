using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nnode.Proxy.RequestHistory
{
	public class RequestHistoryDto
	{
		public List<HttpRequestModel> Requests { get; set; }
		public int BlockchainId { get; set; }

	}
}
