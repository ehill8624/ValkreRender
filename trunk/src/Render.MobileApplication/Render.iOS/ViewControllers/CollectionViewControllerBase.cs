using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
namespace Render.iOS.ViewControllers
{
    public abstract class CollectionViewControllerBase<TViewModel> : ReactiveCollectionViewController, IViewFor<TViewModel> where TViewModel : class
    {
		public CollectionViewControllerBase(UICollectionViewLayout withLayout) : base(withLayout){

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

            BindControls();

			View.EndEditing (true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            UnbindControls();
        }

        protected abstract void SetupUserInterface();
        
		protected abstract void BindControls();

        protected void UnbindControls()
        {
            if (ControlBindings == null) return;

            ControlBindings.Value.Clear();
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