using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Response
{
	public class DNSResponse
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; }
		[JsonProperty("group")]
		public string Group { get; set; }
		[JsonProperty("hasBulk")]
		public bool HasBulk { get; set; }
		[JsonProperty("zone")]
		public string Zone { get; set; }
		[JsonProperty("status")]
		public string Status { get; set; }
		[JsonProperty("serial")]
		public string Serial { get; set; }
		[JsonProperty("isUpdated")]
		public bool IsUpdated { get; set; }
	}
}
