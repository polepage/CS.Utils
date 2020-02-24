using Android.Content;
using Android.OS;
using System;

namespace Droid.Utils.Services
{
    // T is the service interface
    public class ServiceConnection<T> : Java.Lang.Object, IServiceConnection
        where T: class
    {
        public event Action Connected;
        public event Action Disconnected;

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            Service = (service as IServiceBinder<T>)?.Service;
            Connected?.Invoke();
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            Disconnected?.Invoke();
        }

        public T Service { get; private set; }
    }
}