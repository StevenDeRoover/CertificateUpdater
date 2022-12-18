namespace CloudnsAPI.Client.Abstractions
{
	public interface IDNSRequestBuilder
	{
		IDNSRequest Request();

		IZonesRequestBuilder Zone { get; }
	}
}
