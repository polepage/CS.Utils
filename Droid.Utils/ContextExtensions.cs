using Android.Content;
using Android.OS;

namespace Droid.Utils
{
    public static class ContextExtensions
    {
        public static void StartForegroundServiceCompat(this Context context, Intent intent)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }
    }
}