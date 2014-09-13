using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Splat;

namespace Render.iOS
{
    public static class Utilities
    {

		public static void SetTintColor()
        {
			UIApplication.SharedApplication.KeyWindow.TintColor = MobileCore.Values.Colors.Orange.ToNative();

//			TODO: IQKeyboardManager.IQKeyboardManager.SharedManager.ShouldToolbarUsesTextFieldTintColor = true;
        }

		public static UINavigationController BuildNavigationController()
        {
			var navigationController = new ViewControllers.SuperNavigationController();

            navigationController.NavigationBar.BarTintColor = MobileCore.Values.Colors.Orange.ToNative();
			navigationController.NavigationBar.SetTitleTextAttributes(new UITextAttributes() { TextColor = MobileCore.Values.Colors.BrownGray.ToNative() });
            navigationController.NavigationBar.TintColor = MobileCore.Values.Colors.White.ToNative();

            return navigationController;
        }

        public static Boolean IsIos7
        {
            get
            {
                return UIDevice.CurrentDevice.CheckSystemVersion(7, 0);
            }
        }

    }
}