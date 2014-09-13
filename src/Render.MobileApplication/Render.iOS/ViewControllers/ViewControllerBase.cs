using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using System.Threading.Tasks;

namespace Render.iOS.ViewControllers
{
    public abstract class ViewControllerBase<TViewModel> : ReactiveViewController, IViewFor<TViewModel> where TViewModel : class
    {
		private readonly UIActivityIndicatorView loading = new UIActivityIndicatorView();

		private bool _maintainBindings;
		public bool MaintainBindings {
			get {
				return _maintainBindings;
			}
			set {
				_maintainBindings = value;
			}
		}

        //Binding
        protected Lazy<CompositeDisposable> ControlBindings = new Lazy<CompositeDisposable>(() => new CompositeDisposable());

        private TViewModel _viewModel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupUserInterface();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

			if(!MaintainBindings)
            	BindControls();

			View.EndEditing (true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

			if(!MaintainBindings)
            	UnbindControls();
        }

        protected abstract void SetupUserInterface();
        protected abstract void BindControls();

        protected void UnbindControls()
        {
            if (ControlBindings == null) return;

            ControlBindings.Value.Clear();
        }

		public async Task<T> ExecuteMaintainingBindings<T>(Func<Task<T>> actionToExecute) {
			try {
				_maintainBindings = true;

				var result =  await actionToExecute();

				return result;

			} finally {
				_maintainBindings = false;
			}
		}

		public async Task ExecuteMaintainingBindings(Func<Task> actionToExecute) {
			try {
				_maintainBindings = true;

				await actionToExecute();

			} finally {
				_maintainBindings = false;
			}
		}

		public void ExecuteMaintainingBindings(Action actionToExecute) {
			try {
				_maintainBindings = true;

				actionToExecute();

			} finally {
				_maintainBindings = false;
			}
		}

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TViewModel)value; }
        }

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }
    }
}