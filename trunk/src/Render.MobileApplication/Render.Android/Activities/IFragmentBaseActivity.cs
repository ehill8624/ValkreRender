using System;
using Android.App;
using System.Collections;
using System.Collections.Generic;

namespace Render.Android.Activities
{
	public interface IFragmentBaseActivity
	{
		void SwitchFragment (Fragment fragment);
		IDictionary<string,object> PassingObjects {get;}
	}
}

