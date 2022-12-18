using System.Net.Http;
using System.Threading.Tasks;

namespace CloudnsAPI.Client
{
	internal static class HttpResponseMessageExtensions
	{
		public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage message)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await message.Content.ReadAsStringAsync());
		}
	}
}
