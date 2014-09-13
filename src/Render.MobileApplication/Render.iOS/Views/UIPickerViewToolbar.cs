using System;
using MonoTouch.UIKit;

namespace Render.iOS.Views
{
	public class UIPickerViewToolbar : UIToolbar
	{
		public event EventHandler DoneClicked;

		private UIBarButtonItem doneButton;

		public UIPickerViewToolbar (UIColor barTintColor = null, UIColor tintColor = null) : base()
		{
			this.BarStyle = UIBarStyle.Default;

			if (tintColor != null)
				this.TintColor = tintColor;

			if (barTintColor != null) {
				Translucent = true;
				BarTintColor = barTintColor;
			}
			else
				Translucent = true;

			doneButton = new UIBarButtonItem (UIBarButtonSystemItem.Done);

			this.SetItems (new []{ new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton }, false);
		}

		private void OnDoneClicked(object sender, EventArgs e){
			var doneClicked = DoneClicked;
			if (doneClicked != null)
				doneClicked (sender, e);
		}

		public override void WillMoveToSuperview (UIView newsuper)
		{
			base.WillMoveToSuperview (newsuper);

			doneButton.Clicked -= OnDoneClicked;
			doneButton.Clicked += OnDoneClicked;
		}

		public override void RemoveFromSuperview ()
		{
			base.RemoveFromSuperview ();

			doneButton.Clicked -= OnDoneClicked;
		}
	}
}

