using CloudnsAPI.Client.Abstractions;
using CloudnsAPI.Client.Authentication;
using CloudnsAPI.Client.Requests;
using CloudnsAPI.Client.Response;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudnsAPI.Client
{
    public class Client
    {
        private readonly IAuthenticator _authenticator;
        private readonly HttpMessageHandler _innerHandler;

        public Client(IAuthenticator authenticator)
            :this(authenticator,null)
        {
        }

        public string Key { get=> _authenticator.Key; set => _authenticator.Key = value; }
        public string Password { get=> _authenticator.Password; set => _authenticator.Password = value; }

		public async Task<LoginResponse> Login(string key, string password)
        {
            var loginResult = await _authenticator.Login(key, password);
            return loginResult;
        }

        public Client(IAuthenticator authenticator, HttpMessageHandler httpMessageHandler)
        {
            _authenticator = authenticator;
            _innerHandler = httpMessageHandler;
        }

        public IDNSRequestBuilder DNS => new DNSRequestBuilder(this);

        internal HttpClient CreateClient()
        {
            return new HttpClient(new AuthenticationHandler(_authenticator, _innerHandler)) { BaseAddress = new Uri("https://api.cloudns.net") };
        }
    }
}
