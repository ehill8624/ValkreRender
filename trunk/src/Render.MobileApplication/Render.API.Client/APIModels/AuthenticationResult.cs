using System;
using Newtonsoft.Json;

namespace PDRMobile.API.Client.APIModels
{
	public class AuthenticationResult
	{
		public AuthenticationResult ()
		{
		}

		[JsonProperty("account-name")]
		public string AccountName {get;set;}

		[JsonProperty("token")]
		public string Token {get;set;}
	}
}

