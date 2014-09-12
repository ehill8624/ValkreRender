using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ReactiveUI;

namespace Render.MobileCore.ViewModels
{
    public abstract class ViewModelBase<T> : ReactiveObject
    {
        public abstract string Title { get; }

		protected bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            protected set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

		protected string _errorText;
		public string ErrorText
		{
			get { return _errorText; }
			set { 
				this.RaiseAndSetIfChanged(ref _errorText, value);
				DisplayErrorCommand.Execute (null);
			}
		}


		protected ObservableAsPropertyHelper<bool> _isValid;
        public bool IsValid
        {
			get { return _isValid.Value; }
        }

        public List<ValidationFailure> ValidationErrors { get; protected set; }

        //TODO: public abstract AbstractValidator<T> Validator { get; }

		public ReactiveCommand<string> DisplayErrorCommand { get; private set; }

        protected ViewModelBase()
        {
			DisplayErrorCommand = ReactiveCommand.CreateAsyncTask (async _ => ErrorText);

            RegisterObservables();
        }

        protected abstract void RegisterObservables();

    }
}
