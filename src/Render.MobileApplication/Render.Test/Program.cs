using System;
using Render.API.Client;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Render.Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Console.WriteLine ("Hello World!");

			Test ();

			System.Threading.Thread.Sleep (60000);
			return;
		}

		public static async void Test() {
			APIClient _client = new APIClient ("https://api.render-next.com/mobile/", "caroline.cao@eightbot.com", "!Q@W3e4r");
			//_client.authenticate

			var _authResult = await _client.authenticate ("caroline.cao@eightbot.com", "!Q@W3e4r").ConfigureAwait (true);


			await _client.getactivity (_authResult.FirstOrDefault()).ConfigureAwait (true);
		}
	}
}
