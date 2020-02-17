using Android.Runtime;
using CommonServiceLocator;
using CS.Utils.Unity;
using Droid.Utils.BaseApp;
using Prism.Ioc;
using System;

namespace Droid.Utils.Unity
{
    public abstract class DroidApp : DroidAppBase
    {
        public DroidApp(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

        protected override IContainerExtension CreateContainerExtension()
        {
            return new UnityContainerExtension();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IServiceLocator, UnityServiceLocatorAdapter>();
        }
    }
}
