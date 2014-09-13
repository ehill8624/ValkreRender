using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.ComponentModel;
using System.Drawing;
using Cirrious.FluentLayouts.Touch;

namespace Render.iOS.Views
{
	[Register("FlatButton")]
	public class FlatButton : UIButton
	{
		private const double animationDuration = .1d;

		private UIColor originalTitleColor = UIColor.White;

		private UIActivityIndicatorView loading;

		private UIColor 
			normalColor = UIColor.Clear, 
			selectedColor, 
			disabledColor = UIColor.Gray;

		float cornerRadius = 4.0f;
		public float CornerRadius {
			get {
				return cornerRadius;
			}
			set {
				cornerRadius = value;
				Initialize ();
			}
		}

		public FlatButton (IntPtr handle) : base(handle){
			Initialize ();
		}

		public FlatButton (RectangleF frame) : base(frame){
			Initialize ();
		}
			
		private void Initialize(){
		
			if (loading == null) {
				loading = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
				loading.Alpha = default(float);
				loading.TranslatesAutoresizingMaskIntoConstraints = false;
				this.Add (loading);
				this.AddConstraints (
					loading.WithSameCenterX(this),
					loading.WithSameCenterY(this)
				);
			}
				

			InvokeOnMainThread(() => {
				Layer.AllowsEdgeAntialiasing = true;
				Layer.EdgeAntialiasingMask = MonoTouch.CoreAnimation.CAEdgeAntialiasingMask.All;
				Layer.MasksToBounds = true;

				Layer.CornerRadius = cornerRadius;

				UIView.Animate(animationDuration, () => {
					BackgroundColor = Enabled ? normalColor : disabledColor;
					Alpha = Highlighted ? .80f : 1.0f;
				});
			
			});
		}

		public override bool Highlighted {
			get {
				return base.Highlighted;
			}
			set {
				base.Highlighted = value;

				UIView.Animate(animationDuration, 
					() => {
						BackgroundColor = 
							Highlighted && selectedColor != null 
								? selectedColor
								: normalColor;

						Alpha = Highlighted ? .80f : 1.0f;
					});
			}
		}

		public override bool Enabled {
			get {
				return base.Enabled;
			}
			set {
				base.Enabled = value;

				Initialize ();
			}
		}
			
		public void ToggleLoading(bool isLoading = true){
			if (isLoading) {
				loading.StartAnimating ();
				originalTitleColor = TitleColor(UIControlState.Normal);
			}
			else
				loading.StopAnimating ();

			InvokeOnMainThread(() => 
				UIView.Animate(animationDuration, () =>
					{
					loading.Alpha = isLoading ? 1.0f : default(float);
					SetTitleColor(isLoading ? UIColor.Clear : originalTitleColor, UIControlState.Normal);
					}));
		}

		public void SetBackgroundColor(UIColor color, UIControlState state){
			switch (state) {
				case UIControlState.Highlighted:
				case UIControlState.Selected:
					selectedColor = color;
					break;
				case UIControlState.Disabled:
					disabledColor = color;
					break;
				default:
					normalColor = color;
					break;
			}

			Initialize ();
		}
	}
}

