using System;
using System.Linq;
using System.Threading.Tasks;
//using Render.MobileCore.Validators;
using ReactiveUI;
using Serilog;

namespace Render.MobileCore.ViewModels
{
	public class Login : ViewModelBase<Login>
	{
		public Login ()
		{
		}

		public override string Title
		{
			get { return Values.Strings.Login_Title; }
		}

		protected override void RegisterObservables()
		{
		}
	}
}

