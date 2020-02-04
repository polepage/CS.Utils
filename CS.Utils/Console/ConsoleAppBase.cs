using CommonServiceLocator;
using CS.Utils.Modularity;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using System;

namespace CS.Utils.Console
{
    public abstract class ConsoleAppBase: IDisposable
    {
        private IContainerExtension _containerExtension;
        private IModuleCatalog _moduleCatalog;
        private ICommandParser _parser;

        public ConsoleAppBase()
        {
            Initialize();
        }

        public IContainerProvider Container => _containerExtension;

        public int RunStartupCommands(ICommandReader reader, out bool doContinue)
        {
            int status = 0;
            doContinue = true;
            foreach (string[] args in reader.ReadCommands())
            {
                if (!_parser.ParseStartupCommand(args, out status))
                {
                    doContinue = false;
                    return status;
                }
            }

            return status;
        }

        public int RunCommands(ICommandReader reader)
        {
            int status = 0;
            foreach (string[] args in reader.ReadCommands())
            {
                if (!_parser.ParseCommand(args, out status))
                {
                    return status;
                }
            }

            return status;
        }

        public virtual void Dispose() { }

        protected virtual void Initialize()
        {
            _containerExtension = CreateContainerExtension();
            _moduleCatalog = CreateModuleCatalog();
            _parser = CreateParser();

            RegisterRequiredTypes(_containerExtension);
            RegisterTypes(_containerExtension);
            _containerExtension.FinalizeExtension();

            ConfigureServiceLocator();

            ConfigureModuleCatalog(_moduleCatalog);
            InitializeModules();
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

        protected abstract ICommandParser CreateParser();
    }
}
