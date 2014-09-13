using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Render.MobileCore.Extensions;
using ReactiveUI;

using Splat;


namespace Render.Android.Activities
{
	[Activity]
    public class Login : ActivityBase<MobileCore.ViewModels.Login>
    {
        //Controls
        private EditText username, password;
        private TextView forgotPassword, createNewAccount;
        private Button login;
        private ProgressBar loading;

        public Login() { }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			//git hub test
            ViewModel = new MobileCore.ViewModels.Login();

			Title = ViewModel.Title;
        }

        protected override void SetupUserInterface()
        {
			SetContentView(Resource.Layout.Login);
        }

        protected override void BindControls()
        {
 
        }
    }
}