using System;
using MonoTouch.UIKit;
using PDRMobile.Models;
using ReactiveUI;
using MonoTouch.Foundation;

namespace PDRMobile.iOS.TableViewSources
{
	public class ContactSource : ReactiveUI.ReactiveTableViewSource <MobileCore.ViewModels.Contacts.Contact>
	{
		public ContactSource (UITableView tableView, IReactiveNotifyCollectionChanged<MobileCore.ViewModels.Contacts.Contact> collection, NSString cellKey, float sizeHint, Action<UITableViewCell> initializeCellAction = null)
			: base(tableView, collection, cellKey, sizeHint, initializeCellAction)
		{

		}

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath) as TableViewCells.ContactCell;

			cell.ViewModel.AcceptCommand.Execute (null);

			tableView.DeselectRow (indexPath, true);
		}
	}
}

