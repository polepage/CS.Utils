using Android.OS;

namespace Droid.Utils.Services
{
    public interface IServiceBinder<T>: IBinder
    {
        T Service { get; }
    }
}