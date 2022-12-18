using CloudnsAPI.Client.Abstractions;

namespace CloudnsAPI.Client.Requests
{
	internal class DNSRequestBuilder : IDNSRequestBuilder
	{
		private Client client;

		public DNSRequestBuilder(Client client)
		{
			this.client = client;
		}

		public IZonesRequestBuilder Zone => new ZonesRequestBuilder(client);

		public IDNSRequest Request() => new DNSRequest(client);
	}
}