namespace CloudnsAPI.Client.Abstractions
{
	public interface IZonesRequestBuilder
	{
		IZoneRequestBuilder this[string zone] { get; }
	}
}
