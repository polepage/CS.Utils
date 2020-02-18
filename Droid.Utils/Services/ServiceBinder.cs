using Android.OS;

namespace Droid.Utils.Services
{
    public class ServiceBinder<T> : Binder, IServiceBinder<T>
    {
        public ServiceBinder(T service)
        {
            Service = service;
        }

        public T Service { get; }
    }
}