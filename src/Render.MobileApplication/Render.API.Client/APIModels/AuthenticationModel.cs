using System;
using Newtonsoft.Json;

namespace PDRMobile.API.Client.APIModels
{
	public class AuthenticationModel
	{
		public AuthenticationModel ()
		{
		}

		[JsonProperty("username")]
		public string Username {get;set;}

		[JsonProperty("password")]
		public string Password {get;set;}
	}
}

