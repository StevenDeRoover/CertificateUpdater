﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Response
{
	public class LoginResponse
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("statusDescription")]
		public string StatusDescription { get; set; }
	}
}
