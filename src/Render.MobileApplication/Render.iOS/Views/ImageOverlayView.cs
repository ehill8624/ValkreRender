using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Cirrious.FluentLayouts.Touch;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using Splat;
using System.Reactive.Linq;

namespace Render.iOS.Views
{
	public class ImageOverlayView : ReactiveView
	{

		private readonly List<PointF> locations = new List<PointF>();
		UIImage drawableView;

		FlatButton add, undo;

		private bool _enabled;
		public bool Enabled {
			get { return _enabled;}
			set {
				this.RaiseAndSetIfChanged(ref _enabled, value);
			}
		}

		private bool _editing;
		public bool Editing {
			get { return _editing;}
			set {
				this.RaiseAndSetIfChanged(ref _editing, value);
			}
		}

		private bool needsPoint;

		public ImageOverlayView (UIImage viewToDraw) : this(viewToDraw, new RectangleF()){

		}

		public ImageOverlayView(UIImage viewToDraw, RectangleF frame) : base(frame){

			this.BackgroundColor = UIColor.Clear;

			drawableView = viewToDraw;
			this.MultipleTouchEnabled = false;
			needsPoint = true; 

			add = new FlatButton(RectangleF.Empty);
			add.SetTitle ("Add", UIControlState.Normal);
			add.SetBackgroundColor (MobileCore.Values.Colors.WhiteSeventyFivePercent.ToNative (), UIControlState.Normal);
			add.SetTitleColor (MobileCore.Values.Colors.DarkGray.ToNative (), UIControlState.Normal);
			add.TouchUpInside += AddClicked;
			Add (add);

			undo = new FlatButton(RectangleF.Empty);
			undo.SetTitle ("Undo", UIControlState.Normal);
			undo.SetBackgroundColor (MobileCore.Values.Colors.WhiteSeventyFivePercent.ToNative (), UIControlState.Normal);
			undo.SetTitleColor (MobileCore.Values.Colors.DarkGray.ToNative (), UIControlState.Normal);
			undo.TouchUpInside += UndoClicked;
			Add (undo);

			this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();

			this.AddConstraints (
				undo.AtBottomOf(this, Constants.Layout.VerticalPadding),
				undo.AtRightOf(this, Constants.Layout.HorizontalPadding),
				undo.Width().GreaterThanOrEqualTo(56f),
				undo.Height().GreaterThanOrEqualTo(32f),

				add.AtBottomOf(this, Constants.Layout.VerticalPadding),
				add.ToLeftOf(undo, Constants.Layout.HorizontalPadding),
				add.Width().GreaterThanOrEqualTo(56f),
				add.Height().GreaterThanOrEqualTo(32f)
			);

			this.WhenAnyValue (vm => vm.Enabled)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe (enabled => {

					UserInteractionEnabled = enabled;

					UIView.AnimateAsync(Constants.Animation.StandardAnimationDuration, () => add.Alpha = undo.Alpha =	enabled ? 1.0f : 0.0f);

				});

			this.WhenAnyValue (vm => vm.Editing)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe (editing => {
					if(Enabled)
						UIView.AnimateAsync(Constants.Animation.StandardAnimationDuration, () => add.Alpha = undo.Alpha =	editing ? 0.0f : 1.0f );

				});
		}

		void AddClicked (object sender, EventArgs e)
		{
			needsPoint = true;
		}

		void UndoClicked (object sender, EventArgs e)
		{
			if (!locations.Any ())
				return;

			locations.RemoveAt (locations.Count - 1);
			needsPoint = true;

			this.SetNeedsDisplay ();
		}
			
		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			if (!Enabled)
				return;
				
			Editing = true;

			var touch = touches.AnyObject as UITouch;

			if (needsPoint) {
				locations.Add(touch.LocationInView (this));
				needsPoint = false;
			}
			else
				locations[locations.Count - 1] = touch.LocationInView (this);

			this.SetNeedsDisplay ();
		}

		public override void TouchesMoved (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			if (!Enabled)
				return;

			Editing = true;

			var touch = touches.AnyObject as UITouch;
			locations[locations.Count - 1] = touch.LocationInView (this);
			this.SetNeedsDisplay ();
		}

		public override void TouchesCancelled (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			this.TouchesEnded (touches, evt);
		}

		public override void TouchesEnded (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			if (!Enabled)
				return;

			Editing = false;

			this.SetNeedsDisplay ();
		}

		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);

			using (CGContext context = UIGraphics.GetCurrentContext ()) {
				// turn on anti-aliasing
				context.SetAllowsAntialiasing (true);
				context.ClearRect (rect);

				foreach (var location in locations) {

					drawableView.Draw (
						new RectangleF (
							new PointF(
								location.X - (drawableView.Size.Height / 2f), 
								location.Y - (drawableView.Size.Width / 2f)
							), drawableView.Size)
					);
//
//					context.DrawImage (
//						new RectangleF (
//							new PointF(
//								location.X - (drawableView.Size.Height / 2f), 
//								location.Y - (drawableView.Size.Width / 2f)
//							), drawableView.Size), 
//						drawableView.CGImage);
				}
			}
		}
	}
}

