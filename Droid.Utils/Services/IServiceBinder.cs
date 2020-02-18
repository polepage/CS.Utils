namespace Droid.Utils.Services
{
    public interface IServiceBinder<T>
    {
        T Service { get; }
    }
}