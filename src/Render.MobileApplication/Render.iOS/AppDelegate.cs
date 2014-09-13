using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Render.MobileCore;
using ReactiveUI;
using Serilog;

namespace Render.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {

		public static AppDelegate Current {
			get;
			private set;
		}

        // class-level declarations
        UIWindow window;

        readonly AutoSuspendHelper autoSuspendHelper;

        public AppDelegate()
        {

            RxApp.SuspensionHost.CreateNewAppState = () => new AppState();
            RxApp.SuspensionHost.SetupDefaultSuspendResume();


			MessageBar.MessageBarManager.SharedInstance.StyleSheet = new RenderMessageBarStyleSheet ();

#if DEBUG || ADHOC
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
#endif

            autoSuspendHelper = new AutoSuspendHelper(this);
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			Current = this;

			var log = new LoggerConfiguration().CreateLogger();

			log.Information("Loading");

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;

            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            // If you have defined a view, add it here:
            // window.RootViewController  = navigationController;

			var rootNavigationController = Utilities.BuildNavigationController();
            rootNavigationController.PushViewController(new ViewControllers.Login(), false);
            window.RootViewController = rootNavigationController;

            // make the window visible
            window.MakeKeyAndVisible();

            Utilities.SetTintColor();

            autoSuspendHelper.FinishedLaunching(app, options);

            return true;
        }

        public override void OnActivated(UIApplication application)
        {
            autoSuspendHelper.OnActivated(application);
        }

        public override async void DidEnterBackground(UIApplication application)
        {
			await RenderRepository.Current.Shutdown ();

            autoSuspendHelper.DidEnterBackground(application);
        }
    }
}