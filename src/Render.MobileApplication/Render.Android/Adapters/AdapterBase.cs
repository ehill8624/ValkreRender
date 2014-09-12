using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Render.Android
{
	public abstract class AdapterBase<TEntity> : BaseAdapter<TEntity>
	{
		protected Activity _context = null;
		protected IList<TEntity> _list = new List<TEntity>();

		protected int _layoutID = 0;

		public AdapterBase (Activity context, IList<TEntity> list, int layoutID)  : base ()
		{
			this._context = context;
			this._list = list;
			this._layoutID = layoutID;
		}

		public override TEntity this[int position]
		{
			get { return _list[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return _list.Count; }
		}

		public abstract View SetupCellInterface (TEntity item, View view);

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			if (position >= _list.Count) {
				return new View (convertView.Context);
			}

			var item = _list[position];

			var view = (convertView ?? this._context.LayoutInflater.Inflate (_layoutID, parent, false));

			return this.SetupCellInterface(item, view);
		}
	}
}

