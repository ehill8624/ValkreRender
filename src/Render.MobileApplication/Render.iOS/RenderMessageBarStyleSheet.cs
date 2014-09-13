using System;
using MessageBar;
using MonoTouch.UIKit;

namespace Render.iOS
{
	public class RenderMessageBarStyleSheet : MessageBarStyleSheet
	{
		public override MonoTouch.UIKit.UIColor BackgroundColorForMessageType (MessageType messageType)
		{
			switch (messageType) {
				case MessageType.Error:
					return UIColor.Red;
				default:
					return base.BackgroundColorForMessageType (messageType);
			}

		}
	}
}

