using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Abstractions
{
	public interface IZoneRecordsRequestBuilder
	{
		IZoneRecordsRequest Request();

		IZoneRecordRequestBuilder this[string id] { get; }
	}
}
