using CloudnsAPI.Client.Response;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Abstractions
{
	public interface IAuthenticator
	{
		string Key { get; set; }
		string Password { get; set; }
		Task<LoginResponse> Login();
		Task<LoginResponse> Login(string key, string password);

		Task AuthenticateAsync(HttpRequestMessage request);
	}
}
