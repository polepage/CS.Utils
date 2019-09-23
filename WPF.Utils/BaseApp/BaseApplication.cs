using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Prism.Unity.Ioc;
using System.Windows;

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
            //_container.RegisterDialogWindow<DialogHost>();
        }

        protected abstract void RegisterViewModelLocator();
    }
}
