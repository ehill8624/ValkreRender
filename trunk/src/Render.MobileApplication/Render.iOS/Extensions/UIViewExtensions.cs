using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Render.iOS.Extensions
{
	public static class UIViewExtensions
	{
		public static UIImage ToImage (this UIView view, float scale = 1.0f)
		{
			var originalRasterizationScale = view.Layer.RasterizationScale;
			try {
				UIGraphics.BeginImageContext (new SizeF(view.Bounds.Size.Width * scale, view.Bounds.Size.Height * scale));
				view.Layer.RasterizationScale = scale;
				view.Layer.RenderInContext (UIGraphics.GetCurrentContext ());
				return UIGraphics.GetImageFromCurrentImageContext ();
			} finally {
				UIGraphics.EndImageContext ();
				view.Layer.RasterizationScale = originalRasterizationScale;
			}
		}
	}
}

