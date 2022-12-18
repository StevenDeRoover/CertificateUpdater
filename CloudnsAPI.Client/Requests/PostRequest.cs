using CloudnsAPI.Client.Abstractions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudnsAPI.Client.Requests
{
	internal abstract class PostRequest<T> : PostRequest<T, object>, IPostRequest<T>
	{
		private Client client;

		public PostRequest(Client client) : base(client)
		{
			this.client = client;
		}

		public new async Task PostAsync(T value)
		{
			await base.PostAsync(value);
		}
	}

	internal abstract class PostRequest<T, Y> : IPostRequest<T, Y>
	{
		private Client client;

		public PostRequest(Client client)
		{
			this.client = client;
		}

		protected abstract string GetUrl(T value);

		protected virtual HttpContent CreateContent(T value)
		{
			return new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(value));
		}

		public virtual async Task<Y> PostAsync(T value)
		{
			var client = this.client.CreateClient();
			return await (await client.PostAsync(GetUrl(value), CreateContent(value))).ReadAsAsync<Y>();
		}
	}
}