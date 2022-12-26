using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Owin;
using System.Net.Http.Headers;
using System;
using Topshelf.Configurators;

namespace CertificateUpdater.Service.Api
{
	public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public string User { get; set; }
		public string Pass { get; set; }

		private const string _AuthenticationScheme = "Basic";

		public bool Authorize([NotNull] DashboardContext context)
		{
			var owinContext = new OwinContext(context.GetOwinEnvironment());
			var header = owinContext.Request.Headers["Authorization"];

			if (Missing_Authorization_Header(header))
			{
				SetChallengeResponse(owinContext.Response);
				return false;
			}

			var authValues = AuthenticationHeaderValue.Parse(header);

			if (Not_Basic_Authentication(authValues))
			{
				SetChallengeResponse(owinContext.Response);
				return false;
			}

			var tokens = Extract_Authentication_Tokens(authValues);

			if (tokens.Are_Invalid())
			{
				SetChallengeResponse(owinContext.Response);
				return false;
			}

			if (tokens.Credentials_Match(User, Pass))
			{
				return true;
			}

			return false;
			//can add some more logic here...

		}

		private bool Missing_Authorization_Header(string header)
		{
			return string.IsNullOrWhiteSpace(header);
		}

		private static bool Not_Basic_Authentication(AuthenticationHeaderValue authValues)
		{
			return !_AuthenticationScheme.Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase);
		}

		private static BasicAuthenticationTokens Extract_Authentication_Tokens(AuthenticationHeaderValue authValues)
		{
			var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
			var parts = parameter.Split(':');
			return new BasicAuthenticationTokens(parts);
		}

		private void SetChallengeResponse(IOwinResponse response)
		{
			response.StatusCode = 401;
			response.Headers.Add("WWW-Authenticate", new string[] { "Basic realm=\"Hangfire Dashboard\"" });
			var buffer = System.Text.Encoding.UTF8.GetBytes("Authentication is required.");
			response.Body.WriteAsync(buffer, 0, buffer.Length);
		}
	}
}
