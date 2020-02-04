using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CS.Utils.Modularity
{
    public class ModuleManager : IModuleManager
    {
        public ModuleManager(IModuleInitializer moduleInitializer, IModuleCatalog moduleCatalog)
        {
            ModuleInitializer = moduleInitializer;
            ModuleCatalog = moduleCatalog;
        }

        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        protected IModuleCatalog ModuleCatalog { get; }
        protected IModuleInitializer ModuleInitializer { get; }

        public void LoadModule(string moduleName)
        {
            var modules = ModuleCatalog.Modules.Where(m => m.ModuleName == moduleName);
            if (modules == null || modules.Count() == 0)
            {
                throw new ModuleNotFoundException(moduleName, string.Format(CultureInfo.CurrentCulture, "Module not found", moduleName));
            }
            else if (modules.Count() > 1)
            {
                throw new DuplicateModuleException(moduleName, string.Format(CultureInfo.CurrentCulture, "Duplicate module", moduleName));
            }

            LoadModules(ModuleCatalog.CompleteListWithDependencies(modules));
        }

        public void Run()
        {
            LoadModulesWhenAvailable();
        }

        protected void LoadModulesWhenAvailable()
        {
            var whenAvailableModules = ModuleCatalog.Modules.Where(m => m.InitializationMode == InitializationMode.WhenAvailable && m.State == ModuleState.NotStarted);
            if (whenAvailableModules != null)
            {
                LoadModules(whenAvailableModules);
            }
        }

        protected virtual void LoadModules(IEnumerable<IModuleInfo> moduleInfos)
        {
            foreach (var moduleInfo in moduleInfos)
            {
                if (moduleInfo.State == ModuleState.NotStarted)
                {
                    try
                    {
                        moduleInfo.State = ModuleState.Initializing;
                        ModuleInitializer.Initialize(moduleInfo);
                        moduleInfo.State = ModuleState.Initialized;
                        RaiseLoadModuleCompleted(moduleInfo);
                    }
                    catch (Exception ex)
                    {
                        RaiseLoadModuleCompleted(moduleInfo, ex);
                    }

                }
            }
        }

        protected void RaiseLoadModuleCompleted(IModuleInfo moduleInfo, Exception ex = null)
        {
            LoadModuleCompleted?.Invoke(this, new LoadModuleCompletedEventArgs(moduleInfo, ex));
        }
    }
}
