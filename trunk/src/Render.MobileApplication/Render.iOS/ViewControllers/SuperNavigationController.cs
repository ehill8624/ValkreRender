using System;
using MonoTouch.UIKit;

namespace Render.iOS.ViewControllers
{
	public class SuperNavigationController : UINavigationController
	{
		public event EventHandler<UIInterfaceOrientation> OrientationChanged;

		public SuperNavigationController ()
		{
		}
			
		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);

			var orientationChanged = OrientationChanged;

			if (orientationChanged != null)
				orientationChanged (this, this.InterfaceOrientation);
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return this.TopViewController != null 
				? TopViewController.GetSupportedInterfaceOrientations () 
				: base.GetSupportedInterfaceOrientations ();

		}

		public override bool ShouldAutorotate ()
		{
			return TopViewController != null 
				? TopViewController.ShouldAutorotate () 
				: base.ShouldAutorotate ();

		}
	}
}

