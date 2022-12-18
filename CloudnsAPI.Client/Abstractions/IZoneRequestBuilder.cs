namespace CloudnsAPI.Client.Abstractions
{
	public interface IZoneRequestBuilder
	{
		IZoneRecordsRequestBuilder Records { get; }
	}
}
