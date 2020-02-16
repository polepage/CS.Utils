using Android.App;
using Android.Runtime;
using CommonServiceLocator;
using CS.Utils.Modularity;
using CS.Utils.Unity;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using System;
using Unity;

namespace Droid.Utils.BaseApp
{
    public abstract class DroidAppBase: Application
    {
        public DroidAppBase(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

        private IContainerExtension _containerExtension;
        private IModuleCatalog _moduleCatalog;

        public IContainerProvider Container => _containerExtension;

        public override void OnCreate()
        {
            base.OnCreate();

            _containerExtension = CreateContainerExtension();
            _moduleCatalog = CreateModuleCatalog();

            RegisterRequiredTypes(_containerExtension);
            RegisterTypes(_containerExtension);
            _containerExtension.FinalizeExtension();

            ConfigureServiceLocator();

            ConfigureModuleCatalog(_moduleCatalog);
            InitializeModules();
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            Container?.GetContainer()?.Dispose();
        }

        protected abstract IContainerExtension CreateContainerExtension();

        protected virtual IModuleCatalog CreateModuleCatalog()
        {
            return new ModuleCatalog();
        }

        protected virtual void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(_containerExtension);
            containerRegistry.RegisterInstance(_moduleCatalog);
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<IModuleInitializer, ModuleInitializer>();
            containerRegistry.RegisterSingleton<IModuleManager, ModuleManager>();
        }

        protected abstract void RegisterTypes(IContainerRegistry containerRegistry);

        protected virtual void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => _containerExtension.Resolve<IServiceLocator>());
        }

        protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) { }

        protected virtual void InitializeModules()
        {
            IModuleManager manager = _containerExtension.Resolve<IModuleManager>();
            manager.Run();
        }
    }
}