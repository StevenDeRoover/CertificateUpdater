using CloudnsAPI.Client.Abstractions;

namespace CloudnsAPI.Client.Requests
{
	internal class ZoneRecordsRequestBuilder : IZoneRecordsRequestBuilder
	{
		private Client client;
		private string zone;

		public ZoneRecordsRequestBuilder(Client client, string zone)
		{
			this.client = client;
			this.zone = zone;
		}

		public IZoneRecordRequestBuilder this[string id] => new ZoneRecordRequestBuilder(client, zone);

		public IZoneRecordsRequest Request() => new ZoneRecordsRequest(client, zone);
	}
}