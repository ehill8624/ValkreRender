using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;

namespace Render.Android.Activities
{
	[Activity(MainLauncher = true, Theme="@style/Theme.Splash", NoHistory=true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			Thread.Sleep (3000);
			StartActivity (typeof(Login));
        }
    }
}