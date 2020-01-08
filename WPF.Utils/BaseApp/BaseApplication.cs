using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Prism.Unity;
using Prism.Unity.Ioc;
using System.Windows;
using WPF.Utils.Dialogs;

namespace WPF.Utils.BaseApp
{
    public abstract class BaseApplication: Application
    {
        protected IContainerExtension Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegisterContainer();
            RegisterComponents();
            RegisterDialogs();
            RegisterViewModelLocator();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ((IContainerProvider)Container).GetContainer().Dispose();
            base.OnExit(e);
        }

        private void RegisterContainer()
        {
            Container = new UnityContainerExtension();
            Container.RegisterInstance(Container);
        }

        protected virtual void RegisterComponents()
        {
            Container.RegisterSingleton<IEventAggregator, EventAggregator>();
        }

        protected virtual void RegisterDialogs()
        {
            Container.Register<IDialogService, DialogService>();
            Container.RegisterDialogWindow<DialogHost>();
        }

        protected abstract void RegisterViewModelLocator();
    }
}
