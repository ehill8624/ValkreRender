using System;
using FlyoutNavigation;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using Splat;
using System.Drawing;
using System.Threading.Tasks;

namespace Render.iOS.ViewControllers
{
	public class PrimaryNavigationMenu : UIViewController
	{
		public bool FlyoutMenuEnabled {
			get;
			set;
		}

		public void ToggleMenu(){
		}

		/*
		private FlyoutNavigationController flyoutNavigation;

		public bool FlyoutMenuEnabled {
			get;
			set;
		}

		private UIImageView logo;

		public PrimaryNavigationMenu ()
		{
			logo = new UIImageView (UIImage.FromFile ("Logout.png")){
				ContentMode = UIViewContentMode.ScaleAspectFit,
				AutoresizingMask = UIViewAutoresizing.FlexibleMargins,
				TranslatesAutoresizingMaskIntoConstraints = true
			};

			flyoutNavigation =
				new FlyoutNavigationController ();

			flyoutNavigation.EdgesForExtendedLayout = UIRectEdge.All;
			flyoutNavigation.View.Frame = UIScreen.MainScreen.Bounds;
			flyoutNavigation.AutomaticallyAdjustsScrollViewInsets = false;
			flyoutNavigation.NavigationTableView.RowHeight = Constants.Layout.MinimumTouchControlSize * 1.5f;
			flyoutNavigation.NavigationTableView.Bounces = false;
			flyoutNavigation.NavigationTableView.BackgroundColor = MobileCore.Values.Colors.BrownGray.ToNative ();
			flyoutNavigation.Position = FlyOutNavigationPosition.Left;
			flyoutNavigation.NavigationTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;


			flyoutNavigation.ShouldReceiveTouch += FlyoutMenuShouldReceiveTouch;

			var logout = 
				new StyledStringElement ("Logout") { 
				Image = UIImage.FromBundle ("logout"),
				BackgroundColor = MobileCore.Values.Colors.Clear.ToNative (), 
				TextColor = MobileCore.Values.Colors.White.ToNative ()
			};
			logout.Tapped += async () => await Login.NavigateToLogin(this.View.Window);

			var configuration = new Section () {
				new StyledStringElement (MobileCore.Values.Strings.PrimaryMenu_Settings) { 
					Image = UIImage.FromBundle("settings_white"),
					BackgroundColor = MobileCore.Values.Colors.Clear.ToNative (), 
					TextColor = MobileCore.Values.Colors.White.ToNative () 
				}, 
				logout
			};

			var topNav = new Section (logo, null) {
				new StyledStringElement (MobileCore.Values.Strings.PrimaryMenu_Estimates) { 
					Image = UIImage.FromBundle ("estimates_white"),
					BackgroundColor = MobileCore.Values.Colors.BrownGray.ToNative (), 
					TextColor = MobileCore.Values.Colors.White.ToNative () 
				},
				new StyledStringElement (MobileCore.Values.Strings.PrimaryMenu_Contacts) { 
					Image = UIImage.FromBundle ("contacts_white"),
					BackgroundColor = MobileCore.Values.Colors.BrownGray.ToNative (), 
					TextColor = MobileCore.Values.Colors.White.ToNative () 
				}
			};


			flyoutNavigation.NavigationRoot =
				new RootElement ("Render Mobile") {
				topNav,
				configuration
			};

			// TODO:
//			var contactsController = Utilities.BuildNavigationController ();
//			contactsController.PushViewController (new Contacts.ContactList (0), false);


			flyoutNavigation.ViewControllers = new UIViewController[]{ null };

			View.AddSubview (flyoutNavigation.View);
		}

		private bool FlyoutMenuShouldReceiveTouch (UIGestureRecognizer recognizer, UITouch touch)
		{
			return FlyoutMenuEnabled;
		}

		public static async Task<bool> NavigateToPrimaryMenu(UIWindow window){
			if (window == null)
				throw new ArgumentNullException ("window");

			return await UIView.TransitionNotifyAsync(
				window,
				.5d,
				UIViewAnimationOptions.TransitionFlipFromRight,
				() => {

					var animationsEnabledOriginalState = UIView.AnimationsEnabled;
					UIView.AnimationsEnabled = false;

					window.RootViewController = new PrimaryNavigationMenu();
					UIView.AnimationsEnabled = animationsEnabledOriginalState;
				});
		}

		public void ToggleMenu(){
			if (flyoutNavigation != null)
				flyoutNavigation.ToggleMenu ();
		}
		*/

	}
}

