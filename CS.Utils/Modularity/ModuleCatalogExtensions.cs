using Prism.Modularity;

namespace CS.Utils.Modularity
{
    public static class ModuleCatalogExtensions
    {
        public static IModuleCatalog AddModule<T>(this IModuleCatalog catalog, InitializationMode mode = InitializationMode.WhenAvailable)
            where T : IModule =>
            catalog.AddModule<T>(typeof(T).Name, mode);

        public static IModuleCatalog AddModule<T>(this IModuleCatalog catalog, string name)
            where T : IModule =>
            catalog.AddModule<T>(name, InitializationMode.WhenAvailable);

        public static IModuleCatalog AddModule<T>(this IModuleCatalog catalog, string name, InitializationMode mode)
            where T : IModule =>
            catalog.AddModule(new ModuleInfo(typeof(T), name, mode));

        public static IModuleCatalog AddModule<T>(this IModuleCatalog catalog, params string[] dependsOn)
            where T : IModule =>
            catalog.AddModule<T>(InitializationMode.WhenAvailable, dependsOn);

        public static IModuleCatalog AddModule<T>(this IModuleCatalog catalog, string name, params string[] dependsOn)
            where T : IModule =>
            catalog.AddModule<T>(name, InitializationMode.WhenAvailable, dependsOn);

        public static IModuleCatalog AddModule<T>(this IModuleCatalog catalog, InitializationMode mode, params string[] dependsOn)
            where T : IModule =>
            catalog.AddModule<T>(typeof(T).Name, mode, dependsOn);

        public static IModuleCatalog AddModule<T>(this IModuleCatalog catalog, string name, InitializationMode mode, params string[] dependsOn)
            where T : IModule
        {
            var moduleInfo = new ModuleInfo(name, typeof(T).AssemblyQualifiedName, dependsOn)
            {
                InitializationMode = mode
            };
            return catalog.AddModule(moduleInfo);
        }
    }
}
