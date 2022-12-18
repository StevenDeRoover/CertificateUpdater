using CloudnsAPI.Client.Abstractions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Requests
{
	internal abstract class Request<T> : IRequest<T>
	{
		private readonly Client client;

		public Request(Client client)
		{
			this.client = client;
		}

		protected abstract string GetUrl();

		public async Task<T> GetAsync()
		{
			HttpClient client = this.client.CreateClient();
			return await (await client.GetAsync(GetUrl())).ReadAsAsync<T>();
		}
	}
}