using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Response
{
	public class RecordResponse
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("host")]
		public string Host { get; set; }

		[JsonProperty("record")]
		public string Record { get; set; }

		[JsonProperty("dynamicurl_status")]
		public int DynamicUrlStatus { get; set; }

		[JsonProperty("failover")]
		public string FailOver { get; set; }

		[JsonProperty("ttl")]
		public string TTL { get; set; }

		[JsonProperty("status")]
		public int Status { get; set; }

		public override string ToString()
		{
			return $"{Type}: {Host} ({Record})";
		}
	}
}
