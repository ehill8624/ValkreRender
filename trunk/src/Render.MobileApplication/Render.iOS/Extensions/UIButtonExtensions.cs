using System;
using MonoTouch.UIKit;

namespace Render.iOS
{
	public static class UIButtonExtensions
	{
		public static void CenterVerticallyWithPadding(this UIButton button, float padding = 0.0f){
			var imageSize = button.ImageView.Frame.Size;  
			var titleSize = button.TitleLabel.Frame.Size; 

			var totalHeight = (imageSize.Height + titleSize.Height + padding);  

			button.ImageEdgeInsets = new UIEdgeInsets(
				- (totalHeight - imageSize.Height),
				0.0f,
				0.0f,
				-titleSize.Width);

			button.TitleEdgeInsets = new UIEdgeInsets(
				0.0f,
				-imageSize.Width,
				-(totalHeight - titleSize.Height),
				0.0f);
		}
	}
}

