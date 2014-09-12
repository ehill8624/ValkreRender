using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Render.MobileCore.Extensions
{
    public static class IDisposableExtensions
    {
        public static void DisposeWith(this IDisposable observable, CompositeDisposable disposables)
        {
            disposables.Add(observable);
        }
    }
}
