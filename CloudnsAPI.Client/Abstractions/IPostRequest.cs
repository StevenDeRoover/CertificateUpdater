using System.Threading.Tasks;

namespace CloudnsAPI.Client.Abstractions
{
	public interface IPostRequest<T>
	{
		Task PostAsync(T value);
	}

	public interface IPostRequest<T, Y>
	{
		Task<Y> PostAsync(T value);
	}
}