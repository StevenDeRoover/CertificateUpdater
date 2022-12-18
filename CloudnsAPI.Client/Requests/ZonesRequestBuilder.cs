using CloudnsAPI.Client.Abstractions;

namespace CloudnsAPI.Client.Requests
{
	internal class ZonesRequestBuilder : IZonesRequestBuilder
	{
		private readonly Client client;

		public ZonesRequestBuilder(Client client)
		{
			this.client = client;
		}

		public IZoneRequestBuilder this[string zone] => new ZoneRequestBuilder(client, zone);
	}
}