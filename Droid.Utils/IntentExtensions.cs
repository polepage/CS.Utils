using Android.Content;

namespace Droid.Utils
{
    public static class IntentExtensions
    {
        public static T GetTypedExtra<T>(this Intent intent, string name)
            where T: class
        {
            return intent.GetParcelableExtra(name) as T;
        }
    }
}