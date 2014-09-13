using System;
using MonoTouch.UIKit;
using System.Drawing;
using Splat;
using MonoTouch.Foundation;
using Cirrious.FluentLayouts.Touch;

namespace PDRMobile.iOS.CollectionViewCells
{
	public class EstimatePhotoCellHeader : UICollectionReusableView
	{
		public static readonly NSString Key = new NSString("EstimatePhotoCellHeader");

		public UILabel Header { get; private set; }

		public EstimatePhotoCellHeader (IntPtr handle) : base(handle)
		{
			SetupCell ();
		}

		[Export("InitWithFrame:")]
		public EstimatePhotoCellHeader (RectangleF frame) : base(frame)
		{
			SetupCell ();
		}

		private void SetupCell(){
			BackgroundColor = MobileCore.Values.Colors.BrownGray.ToNative ();

			Header = new UILabel{ 
				Frame = new RectangleF(0, 0, UIScreen.MainScreen.Bounds.Width, Constants.Layout.MinimumTouchControlSize),
				TextColor = MobileCore.Values.Colors.White.ToNative(),
				Lines = 0,
				TextAlignment = UITextAlignment.Left
			};

			Add (Header);

			this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();

			this.AddConstraints (
				Header.AtLeftOf(this, Constants.Layout.HorizontalPadding),
				Header.AtRightOf(this),
				Header.WithSameHeight(this)
			);
		}
	}
}

