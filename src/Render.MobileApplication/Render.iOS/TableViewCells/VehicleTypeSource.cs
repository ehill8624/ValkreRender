using System;
using MonoTouch.UIKit;
using PDRMobile.Models;
using ReactiveUI;
using MonoTouch.Foundation;

namespace PDRMobile.iOS.TableViewSources
{
	public class VehicleTypeSource : ReactiveUI.ReactiveTableViewSource <MobileCore.ViewModels.Vehicles.VehicleType>
	{
		public VehicleTypeSource (UITableView tableView, IReactiveNotifyCollectionChanged<MobileCore.ViewModels.Vehicles.VehicleType> collection, NSString cellKey, float sizeHint, Action<UITableViewCell> initializeCellAction = null)
			: base(tableView, collection, cellKey, sizeHint, initializeCellAction)
		{

		}

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath) as TableViewCells.EstimateCell;

			cell.ViewModel.AcceptCommand.Execute (null);

			tableView.DeselectRow (indexPath, true);
		}
	}
}

