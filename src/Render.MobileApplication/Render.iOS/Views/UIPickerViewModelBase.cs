using System;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace Render.iOS.Views
{
	public class PickerChangedEventArgs<T> : EventArgs{
		public T SelectedValue {get;set;}
	}

	public class UIPickerViewModelBase<T> : UIPickerViewModel
	{
		private readonly List<T> values;

		private float rowHeight;

		public event EventHandler<PickerChangedEventArgs<T>> PickerChanged;

		public UIPickerViewModelBase(IEnumerable<T> values, float rowHeight = 44f)
		{
			this.values = new List<T>(values);

			this.rowHeight = rowHeight;
		}

		public override int GetComponentCount (UIPickerView picker)
		{
			return 1;
		}

		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return values.Count;
		}

		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			return values[row].ToString ();
		}

		public override float GetRowHeight (UIPickerView picker, int component)
		{
			return rowHeight;
		}

		public override void Selected (UIPickerView picker, int row, int component)
		{
			var rowValue = values [row];

			if (this.PickerChanged != null && rowValue != null)
				this.PickerChanged(this, new PickerChangedEventArgs<T>{SelectedValue = rowValue});
		}

		public void ReloadValues(IEnumerable<T> reloadValues){
			values.Clear();
			values.AddRange (reloadValues);
		}
	}
}

