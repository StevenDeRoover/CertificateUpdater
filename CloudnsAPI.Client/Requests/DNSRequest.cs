using CloudnsAPI.Client.Abstractions;
using CloudnsAPI.Client.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Requests
{
	internal class DNSRequest : Request<IEnumerable<DNSResponse>>, IDNSRequest
	{
		public DNSRequest(Client client) : base(client)
		{
		}

		protected override string GetUrl()
		{
			return "/dns/list-zones.json?page=1&rows-per-page=100";
		}
	}
}