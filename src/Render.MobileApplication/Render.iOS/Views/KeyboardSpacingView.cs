using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace Render.iOS.Views
{
	public class KeyboardSpacingView : UIView
	{
		private NSLayoutConstraint heightConstraint = new NSLayoutConstraint ();

		private KeyboardSpacingView ()
		{
		}

		private void EstablishNotificationHandlers(){

			NSNotificationCenter.DefaultCenter.AddObserver (
				UIKeyboard.WillShowNotification, 
				notification => {
				
					if(this.Superview == null)
						return;
						
					var userInfo = notification.UserInfo;


					float duration = default(float);

					if(!float.TryParse(userInfo[UIKeyboard.AnimationDurationUserInfoKey].ToString(), out duration))
						return;

					if(Window == null)
						return;

					RectangleF keyboardFrameEnd = this.Superview.ConvertRectFromView(
						((NSValue)userInfo[UIKeyboard.FrameEndUserInfoKey]).RectangleFValue, this.Window);
						

					var windowFrame = this.Superview.ConvertRectFromView(Window.Frame, Window);

					var heightOffset = (windowFrame.Size.Height - keyboardFrameEnd.Y) - this.Superview.Frame.Y;

					heightConstraint.Constant = heightOffset;

					UIView.Animate(duration, () => {
						this.Superview.LayoutIfNeeded();
					});

					if(this.Superview is UIScrollView){
						var keyboardFrameBegin = this.Superview.ConvertRectFromView(
							((NSValue)userInfo[UIKeyboard.FrameBeginUserInfoKey]).RectangleFValue, this.Window);
							
						keyboardFrameBegin = this.Superview.ConvertRectFromView(keyboardFrameBegin,  null);

						var contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardFrameBegin.Size.Height, 0.0f);

						var scrollViewParent = this.Superview as UIScrollView;

						scrollViewParent.ScrollIndicatorInsets = 
						scrollViewParent.ContentInset = 
							contentInsets;
								
						var aRect = new RectangleF(scrollViewParent.Frame.Location, new SizeF( scrollViewParent.Frame.Width, scrollViewParent.Frame.Height - keyboardFrameBegin.Size.Height));

						UIView activeView = null;

						foreach(var subView in scrollViewParent.Subviews){
							if(subView.IsFirstResponder){
								activeView = subView;
								break;
							}
						}
							
						if(activeView != null && !aRect.Contains(activeView.Frame.Location))
							scrollViewParent.SetContentOffset(
								new PointF(scrollViewParent.ContentOffset.X, activeView.Frame.Location.Y), true);
					}
						
				});
					

			NSNotificationCenter.DefaultCenter.AddObserver (
				UIKeyboard.WillHideNotification,
				notification => {
				
					if(this.Superview == null)
						return;

					var userInfo = notification.UserInfo;

					float duration = default(float);

					if(!float.TryParse(userInfo[UIKeyboard.AnimationDurationUserInfoKey].ToString(), out duration))
						return;

					heightConstraint.Constant = 0.0f;

					UIView.Animate(duration, () => {
						Superview.LayoutIfNeeded();
					});

				
				});
		}

		private void LayoutView(){
			this.TranslatesAutoresizingMaskIntoConstraints = false;

			if (this.Superview == null)
				return;

			foreach (var constraint in this.Constraints) {
				//TODO: Figure out voodoo
				Console.WriteLine (constraint);
			}

			foreach (var constraintString in new string[]{ "H:|[view]|", "V:[view]|" }) {
				this.Superview.AddConstraints (
					NSLayoutConstraint.FromVisualFormat(
						constraintString,
						NSLayoutFormatOptions.DirectionLeadingToTrailing,
						null,
						NSDictionary.FromObjectsAndKeys(new NSObject[] { this }, new NSObject[] { new NSString("view") })					
						)
					);	
			}

			heightConstraint = 
				NSLayoutConstraint.Create (
				this,
				NSLayoutAttribute.Height,
				NSLayoutRelation.Equal, 
				null,
				NSLayoutAttribute.NoAttribute,
				1.0f,
				0.0f);

			this.AddConstraint (heightConstraint);
		}

		public static KeyboardSpacingView InstallToView(UIView parent){

			if (parent == null)
				return null;

			var view = new KeyboardSpacingView ();
			parent.Add (view);
			view.LayoutView ();
			view.EstablishNotificationHandlers ();
			return view;

		}

	}
}

