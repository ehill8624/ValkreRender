
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using System.Reactive.Disposables;

using ReactiveUI;

namespace Render.Android.Fragments
{
	public abstract class FragmentBase<TVViewModel> : ReactiveFragment<TVViewModel> where TVViewModel : class
	{
		protected Lazy<CompositeDisposable> ControlBindings = new Lazy<CompositeDisposable>(() => new CompositeDisposable());
		protected List<EventHandler> eventList = new List<EventHandler> ();

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			SetupUserInterface (view);
			base.OnViewCreated (view, savedInstanceState);
		}

		public override void OnResume()
		{
			base.OnResume();

			//suspendHelper = new AutoSuspendHelper(this);

			BindControls();
		}

		public override void OnPause()
		{
			base.OnPause();

			UnbindControls();
		}

		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);

			//suspendHelper.OnSaveInstanceState(outState);
		}

		protected abstract void SetupUserInterface(View view);
		protected abstract void BindControls();

		protected void UnbindControls()
		{
			if (ControlBindings == null) return;

			ControlBindings.Value.Clear();
		}

		public T GetPassingObject<T>(string key, T defaultObj = default(T)) {
			if (((Render.Android.Activities.IFragmentBaseActivity)this.Activity).PassingObjects.ContainsKey (key)) {
				var obj = ((Render.Android.Activities.IFragmentBaseActivity)this.Activity).PassingObjects [key];
				if (obj != null)
					return (T)obj;
				else
					return defaultObj;
			} else {
				return defaultObj;
			}
		}
		public void SavePassingObject(string key, object obj) {
			if (this.Activity is Render.Android.Activities.IFragmentBaseActivity) {
				((Render.Android.Activities.IFragmentBaseActivity)this.Activity).PassingObjects [key] = obj;
			}
		}
		public void SwitchToFragment(Fragment fragment, Bundle bundle = null, params KeyValuePair<string,object>[] objs) {
			if (bundle != null) {
				fragment.Arguments = bundle;
			}
			if (this.Activity is Render.Android.Activities.IFragmentBaseActivity) {
				foreach (var item in objs) {
					((Render.Android.Activities.IFragmentBaseActivity)this.Activity).PassingObjects[item.Key] = item.Value;
				}
				((Render.Android.Activities.IFragmentBaseActivity)this.Activity).SwitchFragment (fragment);
			}
		}

		/*
		protected Android.Graphics.Drawables.Drawable resizeImage(int resId, int w, int h)
		{
			// load the origial Bitmap
			Android.Graphics.Bitmap BitmapOrg = Android.Graphics.BitmapFactory.DecodeResource(this.Resources, resId);
			int width = BitmapOrg.Width;
			int height = BitmapOrg.Height;
			int newWidth = w;
			int newHeight = h;
			// calculate the scale
			float scaleWidth = ((float) newWidth) / width;
			float scaleHeight = ((float) newHeight) / height;
			// create a matrix for the manipulation
			Android.Graphics.Matrix matrix = new Android.Graphics.Matrix();
			matrix.PostScale(scaleWidth, scaleHeight);
			Android.Graphics.Bitmap resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(BitmapOrg, 0, 0,width, height, matrix, true);
			return new Android.Graphics.Drawables.BitmapDrawable(resizedBitmap);
		}
		*/
	}
}

