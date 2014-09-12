using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Lang.Ref;

using ReactiveUI;

namespace Render.Android.Activities
{
    public abstract class ActivityBase<TViewModel> : ReactiveActivity<TViewModel> where TViewModel : class
    {
        //Autosuspend Helper
        //AutoSuspendHelper suspendHelper;

        //Binding
        protected Lazy<CompositeDisposable> ControlBindings = new Lazy<CompositeDisposable>(() => new CompositeDisposable());

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetupUserInterface();
        }

        protected override void OnResume()
        {
            base.OnResume();

            //suspendHelper = new AutoSuspendHelper(this);

            BindControls();
        }

        protected override void OnPause()
        {
            base.OnPause();

            UnbindControls();
        }

        protected override void OnRestart()
        {
            base.OnRestart();

            //suspendHelper.OnRestart();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            //suspendHelper.OnSaveInstanceState(outState);
        }

        protected abstract void SetupUserInterface();
        protected abstract void BindControls();

        protected void UnbindControls()
        {
            if (ControlBindings == null) return;

            ControlBindings.Value.Clear();
        }
    }
}