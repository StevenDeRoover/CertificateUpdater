using CloudnsAPI.Client.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Authentication
{
	internal class AuthenticationHandler : DelegatingHandler
	{
		private IAuthenticator _authenticator;

		public AuthenticationHandler(IAuthenticator authenticator)
			:this(authenticator, null)
		{
		}

		public AuthenticationHandler(IAuthenticator authenticator, HttpMessageHandler innerHandler)
		{
			this.InnerHandler = innerHandler ?? new HttpClientHandler();
			this._authenticator = authenticator;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			await _authenticator.AuthenticateAsync(request);
			return await base.SendAsync(request, cancellationToken);
		}
	}
}