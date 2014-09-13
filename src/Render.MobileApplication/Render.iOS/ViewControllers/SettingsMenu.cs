using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Cirrious.FluentLayouts.Touch;
using JVFloatSharp;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PDRMobile.MobileCore.Extensions;
using ReactiveUI;
using ReactiveUI.Cocoa;
using ReactiveUI.Mobile;
using Splat;
using PDRMobile.iOS.Views;
using IQKeyboardManager;

namespace PDRMobile.iOS.ViewControllers
{
	class SettingsMenu : ViewControllerBase<MobileCore.ViewModels.SettingsMenu>
    {
        AutoSuspendHelper suspendHelper;

		private UIButton passwordManagement, myProfile, myRates, billing, inviteUsers;

		private UIScrollView scrollView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			ViewModel = new MobileCore.ViewModels.SettingsMenu();

            Title = ViewModel.Title;
        }

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			IQKeyboardManager.IQKeyboardManager.SharedManager.EnableAutoToolbar = false;
			IQKeyboardManager.IQKeyboardManager.SharedManager.ShouldResignOnTouchOutside = true;

			View.EndEditing (true);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if (!NavigationController.ViewControllers.Contains (this))
				ViewModel.NavigateBackCommand.Execute (null);
		}

        protected override void SetupUserInterface()
        {
			EdgesForExtendedLayout = UIRectEdge.None;

			View = scrollView =
				new UIScrollView (RectangleF.Empty){ 
				BackgroundColor = MobileCore.Values.Colors.LightGray.ToNative (),
					ShowsHorizontalScrollIndicator = false,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
					TranslatesAutoresizingMaskIntoConstraints = true
				};

			passwordManagement = new UIButton(UIButtonType.DetailDisclosure);
			passwordManagement.SetTitle(MobileCore.Values.Strings.Settings_PasswordManagement, UIControlState.Normal);
			scrollView.Add(passwordManagement);

			myProfile = new UIButton(UIButtonType.DetailDisclosure);
			myProfile.SetTitle(MobileCore.Values.Strings.Settings_MyProfile, UIControlState.Normal);
			scrollView.Add(myProfile);

			myRates = new UIButton(UIButtonType.DetailDisclosure);
			myRates.SetTitle(MobileCore.Values.Strings.Settings_Rates, UIControlState.Normal);
			scrollView.Add(myRates);

			billing = new UIButton(UIButtonType.DetailDisclosure);
			billing.SetTitle(MobileCore.Values.Strings.Settings_Billing, UIControlState.Normal);
			scrollView.Add(billing);

			inviteUsers = new UIButton(UIButtonType.DetailDisclosure);
			inviteUsers.SetTitle(MobileCore.Values.Strings.Settings_AddUsers, UIControlState.Normal);
			scrollView.Add(inviteUsers);

			scrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();

            View.AddConstraints(

				passwordManagement.AtTopOf(scrollView, Constants.Layout.VerticalPadding),
				passwordManagement.AtLeftOf(scrollView, Constants.Layout.HorizontalPadding),
				passwordManagement.WithSameWidth(scrollView)
					.Minus(Constants.Layout.HorizontalPadding * 2),

				myProfile.WithSameWidth(scrollView)
					.Minus(Constants.Layout.HorizontalPadding * 2),
				myProfile.Below(passwordManagement, Constants.Layout.VerticalPadding),
				myProfile.AtLeftOf(scrollView, Constants.Layout.HorizontalPadding),

				myRates.WithSameWidth(scrollView)
					.Minus(Constants.Layout.HorizontalPadding * 2),
				myRates.Below(myProfile, Constants.Layout.VerticalPadding),
				myRates.AtLeftOf(scrollView, Constants.Layout.HorizontalPadding),

				billing.WithSameWidth(scrollView)
					.Minus(Constants.Layout.HorizontalPadding * 2),
				billing.Below(myRates, Constants.Layout.VerticalPadding),
				billing.AtLeftOf(scrollView, Constants.Layout.HorizontalPadding),

				inviteUsers.WithSameWidth(scrollView)
					.Minus(Constants.Layout.HorizontalPadding * 2),
				inviteUsers.Below(billing, Constants.Layout.VerticalPadding),
				inviteUsers.AtLeftOf(scrollView, Constants.Layout.HorizontalPadding),
				inviteUsers.Bottom()
					.EqualTo()
					.BottomOf(scrollView)
					.Minus(Constants.Layout.VerticalPadding)
                );
        }

        protected override void BindControls()
        {
			ViewModel.NavigateBackCommand.Subscribe (obs => {
				SetEditing(false, false);
			});

			this.BindCommand(ViewModel, vm => vm.PasswordManagementCommand, c => c.passwordManagement)
				.DisposeWith(ControlBindings.Value);

			this.BindCommand(ViewModel, vm => vm.MyProfileCommand, c => c.myProfile)
				.DisposeWith(ControlBindings.Value);

			this.BindCommand(ViewModel, vm => vm.RatesCommand, c => c.myRates)
				.DisposeWith(ControlBindings.Value);

			this.BindCommand(ViewModel, vm => vm.BillingCommand, c => c.billing)
				.DisposeWith(ControlBindings.Value);

			this.BindCommand(ViewModel, vm => vm.AddUsersCommand, c => c.inviteUsers)
				.DisposeWith(ControlBindings.Value);

            ViewModel.WhenAnyValue(x => x.IsLoading)
                .Subscribe(isLoading =>
                {
                    View.EndEditing(true);

                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = isLoading;
                })
                .DisposeWith(ControlBindings.Value);
        }
    }
}