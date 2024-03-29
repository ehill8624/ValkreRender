﻿using System;
using System.Drawing;
using ReactiveUI;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Reactive.Subjects;
using System.Reactive.Concurrency;
using System.Linq;
using System.Threading;
using System.Reactive.Disposables;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Splat;
using System.Reactive;

namespace ReactiveUI
{
	public class ReactiveSwipeTableViewCell 
		: EightBot.SwipeCell.SwipeTableViewCell, 
			ReactiveTableViewCell,
			IReactiveNotifyPropertyChanged<ReactiveSwipeTableViewCell>, 
			IHandleObservableErrors, IReactiveObject, ICanActivate
	{
		public ReactiveSwipeTableViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier) { setupRxObj(); }
		public ReactiveSwipeTableViewCell(UITableViewCellStyle style, NSString reuseIdentifier) : base(style, reuseIdentifier) { setupRxObj(); }

		public event PropertyChangingEventHandler PropertyChanging {
			add { PropertyChangingEventManager.AddHandler(this, value); }
			remove { PropertyChangingEventManager.RemoveHandler(this, value); }
		}

		void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args)
		{
			PropertyChangingEventManager.DeliverEvent(this, args);
		}

		public event PropertyChangedEventHandler PropertyChanged {
			add { PropertyChangedEventManager.AddHandler(this, value); }
			remove { PropertyChangedEventManager.RemoveHandler(this, value); }
		}

		void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args)
		{
			PropertyChangedEventManager.DeliverEvent(this, args);
		}

		/// <summary>
		/// Represents an Observable that fires *before* a property is about to
		/// be changed.         
		/// </summary>
		public IObservable<IReactivePropertyChangedEventArgs<ReactiveTableViewCell>> Changing {
			get { return this.getChangingObservable(); }
		}

		/// <summary>
		/// Represents an Observable that fires *after* a property has changed.
		/// </summary>
		public IObservable<IReactivePropertyChangedEventArgs<ReactiveTableViewCell>> Changed {
			get { return this.getChangedObservable(); }
		}

		public IObservable<Exception> ThrownExceptions { get { return this.getThrownExceptionsObservable(); } }

		void setupRxObj()
		{
		}

		/// <summary>
		/// When this method is called, an object will not fire change
		/// notifications (neither traditional nor Observable notifications)
		/// until the return value is disposed.
		/// </summary>
		/// <returns>An object that, when disposed, reenables change
		/// notifications.</returns>
		public IDisposable SuppressChangeNotifications()
		{
			return this.suppressChangeNotifications();
		}

		Subject<Unit> activated = new Subject<Unit>();
		public IObservable<Unit> Activated { get { return activated; } }
		Subject<Unit> deactivated = new Subject<Unit>();
		public IObservable<Unit> Deactivated { get { return deactivated; } }

		public override void WillMoveToSuperview(UIView newsuper)
		{
			base.WillMoveToSuperview(newsuper);
			RxApp.MainThreadScheduler.Schedule(() => (newsuper != null ? activated : deactivated).OnNext(Unit.Default));
		}
	}
}
