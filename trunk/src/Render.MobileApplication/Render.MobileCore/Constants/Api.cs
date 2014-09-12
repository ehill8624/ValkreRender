using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render.MobileCore.Constants
{
    internal static class Api
    {
#if DEBUG
        public const string ApiBase = "https://api.render-next.com/mobile/";
//		public const string ApiBase = "https://192.168.28.10:44300/api/";
#else
		public const string ApiBase = "https://api.render-next.com/mobile/";
#endif

        public static TimeSpan ApiDefaultTimeout = TimeSpan.FromSeconds(600);

    }
}
