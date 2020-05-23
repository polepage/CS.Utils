using Prism.Modularity;
using System.Linq;
using System.Reflection;

namespace WPF.Utils.Modularity
{
    /// <summary>
    /// Reader for module dependency attribute for the WPF case where dependencies are manually added.
    /// </summary>
    public static class ModuleDependency
    {
        public static string[] GetDependencies<T>()
        {
            return typeof(T)
                .GetCustomAttributes<ModuleDependencyAttribute>()
                .Select(dependency => dependency.ModuleName)
                .ToArray();
        }
    }
}
