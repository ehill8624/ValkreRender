using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.FluentLayouts.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Render.MobileCore.Extensions;
using ReactiveUI;
using ReactiveUI.Cocoa;
using Splat;
using JVFloatSharp;
using Render.iOS.Views;
using Render.iOS.Extensions;

namespace Render.iOS.ViewControllers
{
    class Login : ViewControllerBase<Render.MobileCore.ViewModels.Login>
    {
        private JVFloatSharp.JVFloatLabeledTextField username, password;
		private FlatButton login;
        private UIButton createNewAccount, forgotPassword;

        private UIImageView logo;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = new MobileCore.ViewModels.Login();

            Title = ViewModel.Title;
        }
			
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.NavigationController.SetNavigationBarHidden (true, true);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

//			TODO: IQKeyboardManager.IQKeyboardManager.SharedManager.EnableAutoToolbar = false;
//			TODO: IQKeyboardManager.IQKeyboardManager.SharedManager.ShouldResignOnTouchOutside = true;

			View.EndEditing (true);

		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			this.NavigationController.SetNavigationBarHidden (false, true);
		}

        protected override void SetupUserInterface()
        {
			View.BackgroundColor = MobileCore.Values.Colors.LightGray.ToNative ();
            EdgesForExtendedLayout = UIRectEdge.None;

			logo = new UIImageView(UIImage.FromFile("logo.png"));
            Add(logo);

			username = new JVFloatLabeledTextField(RectangleF.Empty)
            {
                Placeholder = MobileCore.Values.Strings.Login_Username,
				FloatingLabelActiveTextColor = MobileCore.Values.Colors.Orange.ToNative(),
				BorderStyle = UITextBorderStyle.RoundedRect,
				KeyboardType = UIKeyboardType.EmailAddress
            };
			username.RoundedViewBackground (MobileCore.Values.Colors.White.ToNative ());
            Add(username);

            password = new JVFloatLabeledTextField(RectangleF.Empty)
            {
                SecureTextEntry = true,
                Placeholder = MobileCore.Values.Strings.Login_Password,
				FloatingLabelActiveTextColor = MobileCore.Values.Colors.Orange.ToNative(),
				BorderStyle = UITextBorderStyle.RoundedRect,
            };
			password.RoundedViewBackground (MobileCore.Values.Colors.White.ToNative ());
            Add(password);

			login = new FlatButton (RectangleF.Empty);
            login.SetTitle(MobileCore.Values.Strings.Login_Login, UIControlState.Normal);
			login.TintColor = MobileCore.Values.Colors.White.ToNative ();
			login.SetBackgroundColor(MobileCore.Values.Colors.DarkGray.ToNative (), UIControlState.Normal);
            Add(login);

            createNewAccount = new UIButton(UIButtonType.System);
            createNewAccount.SetTitle(MobileCore.Values.Strings.Login_CreateNewAccount, UIControlState.Normal);
            Add(createNewAccount);

            forgotPassword = new UIButton(UIButtonType.System);
            forgotPassword.SetTitle(MobileCore.Values.Strings.Login_ForgotPassword, UIControlState.Normal);
            Add(forgotPassword);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                logo.AtTopOf(View, Constants.Layout.VerticalPadding * 4f),
                logo.WithSameCenterX(View),

                username.Below(logo, Constants.Layout.VerticalPadding),
                username.AtLeftOf(View, Constants.Layout.HorizontalPadding),
                username.AtRightOf(View, Constants.Layout.HorizontalPadding),
				username.Height().EqualTo(Constants.Layout.MinimumTouchControlSize),

                password.Below(username, Constants.Layout.VerticalPadding),
                password.AtLeftOf(View, Constants.Layout.HorizontalPadding),
                password.AtRightOf(View, Constants.Layout.HorizontalPadding),
				password.Height().EqualTo(Constants.Layout.MinimumTouchControlSize),

                login.Below(password, Constants.Layout.VerticalPadding),
                login.AtLeftOf(View, Constants.Layout.HorizontalPadding),
                login.AtRightOf(View, Constants.Layout.HorizontalPadding),
				login.Height().EqualTo(Constants.Layout.MinimumTouchControlSize),

				createNewAccount.Below(login, Constants.Layout.VerticalPadding),
                createNewAccount.AtLeftOf(View, Constants.Layout.HorizontalPadding),
                createNewAccount.AtRightOf(View, Constants.Layout.HorizontalPadding),

                forgotPassword.Below(createNewAccount, Constants.Layout.VerticalPadding),
                forgotPassword.AtLeftOf(View, Constants.Layout.HorizontalPadding),
                forgotPassword.AtRightOf(View, Constants.Layout.HorizontalPadding)
                );
        }

        protected override void BindControls()
        {

        }

		public static async Task<bool> NavigateToLogin(UIWindow window){
			if (window == null)
				throw new ArgumentNullException ("window");

			return await UIView.TransitionNotifyAsync(
				window,
				.5d,
				UIViewAnimationOptions.TransitionFlipFromLeft,
				() => {

					var animationsEnabledOriginalState = UIView.AnimationsEnabled;
					UIView.AnimationsEnabled = false;

					var rootNavigationController = Utilities.BuildNavigationController();
					rootNavigationController.PushViewController(new ViewControllers.Login(), false);

					window.RootViewController = rootNavigationController;
					UIView.AnimationsEnabled = animationsEnabledOriginalState;
				});
		}
    }
}