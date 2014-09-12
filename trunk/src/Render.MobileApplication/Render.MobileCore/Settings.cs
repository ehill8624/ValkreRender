// Helpers/Settings.cs

using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace Render.MobileCore
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }


        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault(Constants.Settings.UsernameKey, string.Empty);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(Constants.Settings.UsernameKey, value))
                    AppSettings.Save();
            }
        }

        public static bool PasswordIsTemporary
        {
            get
            {
                return AppSettings.GetValueOrDefault(Constants.Settings.PasswordIsTemporaryKey, true);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(Constants.Settings.PasswordIsTemporaryKey, value))
                    AppSettings.Save();
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(Constants.Settings.PasswordKey, string.Empty);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(Constants.Settings.PasswordKey, value))
                    AppSettings.Save();
            }
        }



    }
}