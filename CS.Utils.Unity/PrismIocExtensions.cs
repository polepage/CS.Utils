using Prism.Ioc;
using Unity;

namespace CS.Utils.Unity
{
    /// <summary>
    /// This class is copied from Prism.Unity.Ioc to be used without the Prism.WPF package.
    /// </summary>
    public static class PrismIocExtensions
    {
        public static IUnityContainer GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IUnityContainer>)containerProvider).Instance;
        }

        public static IUnityContainer GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<IUnityContainer>)containerRegistry).Instance;
        }
    }
}
