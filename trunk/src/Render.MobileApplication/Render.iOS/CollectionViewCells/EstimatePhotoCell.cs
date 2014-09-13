using System;
using MonoTouch.UIKit;
using ReactiveUI.Cocoa;
using MonoTouch.Foundation;
using ReactiveUI;
using Cirrious.FluentLayouts.Touch;
using System.Drawing;
using System.Reactive.Linq;
using Splat;
using MonoTouch.CoreGraphics;

namespace PDRMobile.iOS.CollectionViewCells
{
	public class EstimatePhotoCell : ReactiveCollectionViewCell, IViewFor<MobileCore.ViewModels.Photos.PhotoInformation>
	{

		public static readonly NSString Key = new NSString("EstimatePhotoCell");

		MobileCore.ViewModels.Photos.PhotoInformation _viewModel;

		UIImageView image;

		[Export("InitWithFrame:")]
		public EstimatePhotoCell(RectangleF frame) : base(frame)
		{
			SetupCell ();
		}

		public EstimatePhotoCell (IntPtr handle) : base(handle)
		{
			SetupCell ();
		}
			
		private void SetupCell(){

			//TODO: Replace this with thumbnail url
			this.WhenAnyValue(x => x.ViewModel.ThumbnailUrl)
				.Where(t => !string.IsNullOrEmpty(t))
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(async t => { 

					var thumbNail = await MobileCore.PDRMobileRepository.Current.DownloadPhotoAsync(t, 100f, 100f);

					if(thumbNail == null) return;

					await UIView.AnimateAsync(Constants.Animation.StandardAnimationDuration, async () => {
						image.Image = thumbNail.ToNative();
						image.Alpha = 1.0f;
					});

				});
					
			this.ContentView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

			ContentView.Transform = CGAffineTransform.MakeScale (.95f, .95f);

			BackgroundView = new UIView{ BackgroundColor = MobileCore.Values.Colors.Orange.ToNative () };

			image = new UIImageView ();
			image.ContentMode = UIViewContentMode.ScaleAspectFill;
			ContentView.Add (image);

			this.ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();

			SetNeedsUpdateConstraints ();
		}

		public override void UpdateConstraints ()
		{
			base.UpdateConstraints ();

			foreach (var view in Subviews)
				view.RemoveConstraints (view.Constraints);

			this.AddConstraints (
				image.AtTopOf(this.ContentView),
				image.AtLeftOf(this.ContentView),
				image.AtRightOf(this.ContentView),
				image.AtBottomOf(this.ContentView)
			);
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (MobileCore.ViewModels.Photos.PhotoInformation)value; }
		}

		public PDRMobile.MobileCore.ViewModels.Photos.PhotoInformation ViewModel
		{
			get { return _viewModel; }
			set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
		}
	}
}

