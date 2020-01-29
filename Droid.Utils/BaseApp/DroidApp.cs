using Android.App;
using Android.Runtime;
using CS.Utils.Unity;
using Prism.Ioc;
using System;
using Unity;

namespace Droid.Utils.BaseApp
{
    public abstract class DroidApp: Application
    {
        public DroidApp(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

        protected static IContainerExtension ContainerExtension { get; private set; }
        public static IContainerProvider Container => ContainerExtension;

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterContainer();
            RegisterComponents();
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            Container?.GetContainer()?.Dispose();
        }

        protected virtual void RegisterComponents() { }

        private void RegisterContainer()
        {
            IUnityContainer unity = new UnityContainer();
            // Load file?

            ContainerExtension = new UnityContainerExtension(unity);
            ContainerExtension.RegisterInstance(ContainerExtension);
        }
    }
}