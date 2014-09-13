using System;
using MonoTouch.UIKit;
using Render.iOS.ViewControllers;

namespace Render.iOS.Extensions
{
	public static class UIViewControllerExtensions
	{
		public static void SetupFlyoutNavigationInNavBar(this UIViewController viewController){

			if (viewController == null || viewController.NavigationItem == null)
				return;

			viewController.NavigationItem.LeftBarButtonItem = 
				new UIBarButtonItem(UIImage.FromBundle("hamburger_menu"), UIBarButtonItemStyle.Plain , 
					delegate {
						var primaryNavigationMenu = UIApplication.SharedApplication.KeyWindow.RootViewController as PrimaryNavigationMenu;

						if(primaryNavigationMenu != null)
							primaryNavigationMenu.ToggleMenu();
					});

		}

		public static void FlyoutMenuEnabled(this UIViewController viewController, bool enabled){
			if (viewController == null || viewController.NavigationItem == null)
				return;

			var primaryNavigationMenu = UIApplication.SharedApplication.KeyWindow.RootViewController as PrimaryNavigationMenu;

			if(primaryNavigationMenu != null)
				primaryNavigationMenu.FlyoutMenuEnabled = enabled;
		}
	}
}

