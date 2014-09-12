using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace Render.Android
{
    public static class Utilities
    {
        public static void DismissKeyboard(this Activity activity)
        {
            DismissKeyboardInternal(activity);   
        }

        public static void DismissKeyboard(this Fragment fragment)
        {
            DismissKeyboardInternal(fragment.Activity);
        }

        private static void DismissKeyboardInternal(Activity activity)
        {
            if (activity == null)
                return;

            var inputManager = activity.GetSystemService(Context.InputMethodService) as InputMethodManager;

            if (inputManager == null || activity.CurrentFocus == null)
                return;

            inputManager.HideSoftInputFromWindow(activity.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}