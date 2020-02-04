using CommonServiceLocator;
using CS.Utils.Console;
using Prism.Ioc;

namespace CS.Utils.Unity
{
    public abstract class ConsoleApp: ConsoleAppBase
    {
        protected override IContainerExtension CreateContainerExtension()
        {
            return new UnityContainerExtension();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IServiceLocator, UnityServiceLocatorAdapter>();
        }

        public override void Dispose()
        {
            base.Dispose();
            Container.GetContainer().Dispose();
        }
    }
}
