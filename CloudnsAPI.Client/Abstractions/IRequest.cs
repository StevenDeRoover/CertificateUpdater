using System.Threading.Tasks;

namespace CloudnsAPI.Client.Abstractions
{
	public interface IRequest<T>
	{
		Task<T> GetAsync();
	}
}
