using CloudnsAPI.Client.Response;
using System.Collections.Generic;

namespace CloudnsAPI.Client.Abstractions
{
	public interface IZoneRecordsRequest : IRequest<IDictionary<string, RecordResponse>>
	{
	}
}