using System;
using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.UIKit;

namespace Render.iOS.Extensions
{
	internal static class ViewExtensions
	{
		public static T RoundedViewBackground<T>(this T view, UIColor backgroundColor, float cornerRadius = 4f, float border = 2f) where T : UIView {
			view.Layer.CornerRadius = cornerRadius;

			view.Layer.BorderWidth = border;

			view.Layer.BackgroundColor = view.Layer.BorderColor 
				= backgroundColor.CGColor;

			view.Layer.MasksToBounds = true;

			return view;
		}

		public static T RoundedBorderedViewBackground<T>(this T view, UIColor backgroundColor, UIColor borderColor, float cornerRadius = 4f, float border = 2f) where T : UIView {
			view.Layer.CornerRadius = cornerRadius;

			view.Layer.BorderWidth = border;

			view.Layer.BackgroundColor = backgroundColor.CGColor;
			view.Layer.BorderColor = borderColor.CGColor;

			view.Layer.MasksToBounds = true;

			return view;
		}
	}
}

