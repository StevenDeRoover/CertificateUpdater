using CloudnsAPI.Client.Abstractions;

namespace CloudnsAPI.Client.Requests
{
	internal class ZoneRecordRequestBuilder : IZoneRecordRequestBuilder
	{
		private Client client;
		private string zone;

		public ZoneRecordRequestBuilder(Client client, string zone)
		{
			this.client = client;
			this.zone = zone;
		}

		public IZoneRecordRequest Request() => new ZoneRecordRequest(client, zone);
	}
}