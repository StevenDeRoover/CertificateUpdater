using CloudnsAPI.Client.Abstractions;
using CloudnsAPI.Client.Response;
using System.Collections;
using System.Collections.Generic;

namespace CloudnsAPI.Client.Requests
{
	internal class ZoneRecordsRequest : Request<IDictionary<string, RecordResponse>>, IZoneRecordsRequest
	{
		private readonly Client client;
		private readonly string zone;

		public ZoneRecordsRequest(Client client, string zone) : base(client)
		{
			this.client = client;
			this.zone = zone;
		}

		protected override string GetUrl() => $"/dns/records.json?domain-name={zone}&page=1&rows-per-page=100";
	}
}