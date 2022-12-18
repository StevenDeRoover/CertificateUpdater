using CloudnsAPI.Client.Abstractions;
using CloudnsAPI.Client.Response;
using System;
using System.Net.Http;
using System.Web;

namespace CloudnsAPI.Client.Requests
{
	internal class ZoneRecordRequest : PostRequest<RecordResponse>, IZoneRecordRequest
	{
		private string zone;

		public ZoneRecordRequest(Client client, string zone) : base(client)
		{
			this.zone = zone;
		}

		protected override string GetUrl(RecordResponse value)
		{
			var uri = "/dns/mod-record.json";
			var query = HttpUtility.ParseQueryString("");
			query["domain-name"] = zone;
			query["record-id"] = value.Id;
			query["host"] = value.Host;
			query["record"] = value.Record;
			query["ttl"] = value.TTL;
			return uri += "?" + query.ToString();
		}

		protected override HttpContent CreateContent(RecordResponse value)
		{
			return new StringContent("");
		}
	}
}