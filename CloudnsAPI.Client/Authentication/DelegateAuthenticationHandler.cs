using CloudnsAPI.Client.Abstractions;
using CloudnsAPI.Client.Response;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace CloudnsAPI.Client.Authentication
{
	public class DelegateAuthenticator : IAuthenticator
	{
		private readonly DelegateAuthenticationOptions _options;
		private bool loggedIn = false;

		public DelegateAuthenticator()
			:this(new DelegateAuthenticationOptions())
		{ }

		public DelegateAuthenticator(DelegateAuthenticationOptions options)
		{
			_options = options;
		}

		public string Key { get => _options.Key; set => _options.Key = value; }
		public string Password { get => _options.Password; set => _options.Password = value; }

		public async Task AuthenticateAsync(HttpRequestMessage request)
		{
			if (!loggedIn)
			{
				await Login();
				loggedIn = true;
			}

			var uriBuilder = new UriBuilder(request.RequestUri);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			query["auth-id"] = _options.Key;
			query["auth-password"] = _options.Password;
			uriBuilder.Query = query.ToString();
			request.RequestUri = uriBuilder.Uri;
		}

		public async Task<LoginResponse> Login()
		{
			var result = await new HttpClient().PostAsync($"https://api.cloudns.net/login/login.json?auth-id={_options.Key}&auth-password={_options.Password}", new StringContent(""));
			return await result.ReadAsAsync<LoginResponse>();
		}

		public async Task<LoginResponse> Login(string key, string password)
		{
			_options.Key = key;
			_options.Password = password;
			return await Login();
		}
	}
}
