using Prism.Modularity;
using System;
using System.Collections.ObjectModel;

namespace CS.Utils.Modularity
{
    public class ModuleInfo : IModuleInfo
    {
        public ModuleInfo()
        {
        }

        public ModuleInfo(string name, string type, params string[] dependsOn)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (dependsOn == null)
                throw new ArgumentNullException(nameof(dependsOn));

            ModuleType = type;
            ModuleName = name;
            foreach (string dependency in dependsOn)
            {
                if (!DependsOn.Contains(dependency))
                {
                    DependsOn.Add(dependency);
                }
            }
        }

        public ModuleInfo(string name, string type)
            : this(name, type, Array.Empty<string>())
        {
        }

        public ModuleInfo(Type moduleType)
            : this(moduleType, moduleType.Name) { }

        public ModuleInfo(Type moduleType, string moduleName)
            : this(moduleType, moduleName, InitializationMode.WhenAvailable) { }

        public ModuleInfo(Type moduleType, string moduleName, InitializationMode initializationMode)
            : this(moduleName, moduleType.AssemblyQualifiedName)
        {
            InitializationMode = initializationMode;
        }

        public InitializationMode InitializationMode { get; set; }
        public ModuleState State { get; set; }

        private Collection<string> _dependsOn;
        public Collection<string> DependsOn
        {
            get => _dependsOn ??= new Collection<string>();
            set => _dependsOn = value;
        }

        private Type _type;
        public string ModuleType
        {
            get => _type.AssemblyQualifiedName;
            set
            {
                _type = Type.GetType(value);
                ModuleName = _type.Name;
                foreach (ModuleDependencyAttribute dependencyAttribute in _type.GetCustomAttributes(typeof(ModuleDependencyAttribute), false))
                {
                    string dependency = dependencyAttribute.ModuleName;
                    if (!DependsOn.Contains(dependency))
                    {
                        DependsOn.Add(dependency);
                    }
                }
            }
        }

        private string _moduleName;
        public string ModuleName
        {
            get => string.IsNullOrEmpty(_moduleName) ? _type.Name : _moduleName;
            set => _moduleName = value;
        }

        public string Ref
        {
            get => throw new NotSupportedException("Module Ref not supported");
            set { }
        }
    }
}
