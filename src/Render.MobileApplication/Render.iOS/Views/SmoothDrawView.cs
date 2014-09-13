using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Cirrious.FluentLayouts.Touch;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using Splat;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Render.iOS.Views
{
	public class SmoothDrawView : ReactiveView
	{

		private ReactiveList<List<UIBezierPath>> paths;
		private PointF[] pts = new PointF[5]; // we now need to keep track of the four points of a Bezier segment and the first control point of the next segment
		private uint ctr;

		private float lineThickness;

		private FlatButton undo;

		private UIColor strokeColor;

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

		public SmoothDrawView (UIColor drawingColor = null, float lineThickness = 2.0f) : this(new RectangleF(), drawingColor, lineThickness){

		}

		public SmoothDrawView(RectangleF frame, UIColor drawingColor, float lineThickness) : base(frame){
			paths = new ReactiveList<List<UIBezierPath>> ();

			this.BackgroundColor = UIColor.Clear;

			strokeColor = drawingColor ?? UIColor.Yellow;

			this.lineThickness = lineThickness;

			this.MultipleTouchEnabled = false;

			undo = new FlatButton(RectangleF.Empty);
			undo.Alpha = default(float);
			undo.SetTitle ("Undo", UIControlState.Normal);
			undo.SetBackgroundColor (MobileCore.Values.Colors.WhiteSeventyFivePercent.ToNative (), UIControlState.Normal);
			undo.SetTitleColor (MobileCore.Values.Colors.DarkGray.ToNative (), UIControlState.Normal);
			Add (undo);

			this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();

			this.AddConstraints (
				undo.AtBottomOf(this, Constants.Layout.VerticalPadding),
				undo.AtRightOf(this, Constants.Layout.HorizontalPadding),
				undo.Width().GreaterThanOrEqualTo(56f),
				undo.Height().GreaterThanOrEqualTo(32f)
			);
				
			Observable.CombineLatest (
				this.WhenAnyValue (x => x.Enabled),
				paths.CountChanged.StartWith(0),
				(enabled, count) => enabled && count > 0)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe (shouldDisplay => UIView.AnimateAsync(Constants.Animation.StandardAnimationDuration, () => undo.Alpha = shouldDisplay ? 1.0f : default(float) ));

			this.WhenAnyValue (vm => vm.Enabled)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe (enabled => UserInteractionEnabled = enabled);

			Observable.FromEventPattern(h => undo.TouchUpInside += h, h => undo.TouchUpInside -= h)
				.Where(_ => paths.Any())
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(_ => {
					paths.RemoveAt (paths.Count - 1);

					this.SetNeedsDisplay ();
					});
		}

		public override void Draw (RectangleF rect)
		{
			strokeColor.SetStroke ();

			paths.ToList().ForEach(p => p.ForEach(bp => bp.Stroke ()));
		}

		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			if (!Enabled)
				return;

			Editing = true;

			ctr = 0;

			var touch = touches.AnyObject as UITouch;

			pts [0] = touch.LocationInView (this);

			paths.Add (new List<UIBezierPath> ());
		}

		public override void TouchesMoved (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			if (!Enabled)
				return;

			Editing = true;

			var touch = touches.AnyObject as UITouch;

			var p = touch.LocationInView (this);

			ctr++;

			pts [ctr] = p;

			if (ctr == 4)
			{
				pts[3] = new PointF((pts[2].X + pts[4].X)/2.0f, (pts[2].Y + pts[4].Y)/2.0f); // move the endpoint to the middle of the line joining the second control point of the first Bezier segment and the first control point of the second Bezier segment

				var path = new UIBezierPath ();

				path.LineWidth = lineThickness;
				path.MoveTo(pts[0]);

				path.AddCurveToPoint(pts[3], pts[1], pts[2]); // add a cubic Bezier from pt[0] to pt[3], with control points pt[1] and pt[2]

				paths.Last().Add (path);

				this.SetNeedsDisplay();

				// replace points and get ready to handle the next segment
				pts[0] = pts[3];
				pts[1] = pts[4];

				ctr = 1;
			}
		}

		public override void TouchesEnded (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			if (!Enabled)
				return;

			Editing = false;

			this.SetNeedsDisplay ();
			ctr = 0;
		}

		public override void TouchesCancelled (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			this.TouchesEnded (touches, evt);
		}
	}
}

