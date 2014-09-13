using System;
using MonoTouch.UIKit;

namespace Render.iOS.Views
{
	public class UIPickerView<T> : UIPickerView
	{

		public override MonoTouch.Foundation.NSObject DataSource {
			get {
				return base.DataSource;
			}
			set {
				base.DataSource = value;
			}
		}

		public override void Select (int row, int component, bool animated)
		{
			base.Select (row, component, animated);
		}
	}
}

