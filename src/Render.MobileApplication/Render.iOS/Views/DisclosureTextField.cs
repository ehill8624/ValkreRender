using System;
using System.Drawing;
using Cirrious.FluentLayouts.Touch;
using MonoTouch.UIKit;
using JVFloatSharp;

namespace Render.iOS.Views
{
	public class DisclosureTextField : JVFloatLabeledTextField
	{

		public event EventHandler DisclosureSelected;

		private UITapGestureRecognizer textFieldTapped;

		UITableViewCell disclosure;

		UIView touchView;

		public DisclosureTextField (RectangleF frame) : base(frame)
		{
			this.RightViewMode = UITextFieldViewMode.Always;

			disclosure = new UITableViewCell ();
			disclosure.Frame = new RectangleF (0, 0, this.Frame.Height, this.Frame.Height);
			disclosure.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			this.RightView = disclosure;

			touchView = new UIView ();
			touchView.TranslatesAutoresizingMaskIntoConstraints = false;
			touchView.UserInteractionEnabled = true;
			touchView.MultipleTouchEnabled = false;
			Add (touchView);

			textFieldTapped = new UITapGestureRecognizer (() => {
				var disclosureSelected = DisclosureSelected;
				if(disclosureSelected != null)
					disclosureSelected(this, EventArgs.Empty);
			});
		}	

		public override void WillMoveToSuperview (UIView newsuper)
		{
			base.WillMoveToSuperview (newsuper);

			touchView.AddGestureRecognizer (textFieldTapped);
		}

		public override void RemoveFromSuperview ()
		{
			base.RemoveFromSuperview ();

			touchView.RemoveGestureRecognizer (textFieldTapped);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			touchView.Frame = new RectangleF (0, 0, this.Frame.Width, this.Frame.Height);
			disclosure.Frame = new RectangleF (this.Frame.Width - this.Frame.Height, 0, this.Frame.Height, this.Frame.Height);
		}
	}

}

