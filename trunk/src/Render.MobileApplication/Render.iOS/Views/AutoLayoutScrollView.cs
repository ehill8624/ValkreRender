using System;
using MonoTouch.UIKit;
using System.Drawing;
using Cirrious.FluentLayouts.Touch;

namespace Render.iOS.Views
{
	public class AutoLayoutScrollView : UIScrollView
	{

		public readonly UIView CustomContentView;

		public AutoLayoutScrollView (RectangleF frame) : base(frame)
		{
			CustomContentView = new UIView (RectangleF.Empty);
			CustomContentView.BackgroundColor = UIColor.Blue;

			CustomContentView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			CustomContentView.TranslatesAutoresizingMaskIntoConstraints = true;

			Add (CustomContentView);
		}

		public override void AddSubview (UIView view)
		{
			if (view != CustomContentView)
				CustomContentView.AddSubview (view);
			else
				base.AddSubview (CustomContentView);
		}

//		public override SizeF ContentSize {
//			get {
//				return base.ContentSize;
//			}
//			set {
//
//				base.ContentSize 
//				= CustomContentView.Frame 
//					= new RectangleF (CustomContentView.Frame.Location, value);
//			}
//		}
			

		public void DoNotTranslateAutoresizingMaskIntoConstraints(){
			CustomContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();
		}
	}
}

