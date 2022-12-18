using CloudnsAPI.Client.Abstractions;

namespace CloudnsAPI.Client.Requests
{
	internal class ZoneRequestBuilder : IZoneRequestBuilder
	{
		private readonly Client client;
		private readonly string zone;

		public ZoneRequestBuilder(Client client, string zone)
		{
			this.client = client;
			this.zone = zone;
		}

		public IZoneRecordsRequestBuilder Records => new ZoneRecordsRequestBuilder(client, zone);
	}
}