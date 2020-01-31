using CommonServiceLocator;
using Microsoft.Practices.Unity.Configuration;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Prism.Regions.Behaviors;
using Prism.Services.Dialogs;
using Prism.Unity;
using Prism.Unity.Ioc;
using Prism.Unity.Regions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Unity;
using WPF.Utils.Dialogs;

namespace WPF.Utils.BaseApp
{
    public abstract class BaseApplication: Application
    {
        protected IContainerExtension Container { get; private set; }
        protected virtual bool LoadExternalConfiguration => false;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ConfigureServiceLocator();

            RegisterComponents();
            RegisterViewModelLocator();

            ConfigureDialogs();
            ConfigureNavigation();

            ConfigureMainWindow();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ((IContainerProvider)Container).GetContainer().Dispose();
            base.OnExit(e);
        }

        private void ConfigureContainer()
        {
            IUnityContainer unity = new UnityContainer();
            if (LoadExternalConfiguration)
            {
                unity.LoadConfiguration();
            }

            Container = new UnityContainerExtension(unity);
            Container.RegisterInstance(Container);
        }

        private void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());
        }

        private void ConfigureMainWindow()
        {
            Window window = CreateMainWindow();
            
            if (Container.IsRegistered<IRegionManager>())
            {
                RegionManager.SetRegionManager(window, Container.Resolve<IRegionManager>());
                RegionManager.UpdateRegions();
            }

            MainWindow = window;
            MainWindow?.Show();
        }

        protected virtual void RegisterComponents()
        {
            Container.RegisterSingleton<IEventAggregator, EventAggregator>();
            Container.RegisterSingleton<IServiceLocator, UnityServiceLocatorAdapter>();
        }

        protected abstract void RegisterViewModelLocator();

        protected abstract Window CreateMainWindow();

        protected virtual void ConfigureDialogs()
        {
            Container.RegisterSingleton<IDialogService, DialogService>();
            Container.RegisterDialogWindow<DialogHost>();

            RegisterDialogs();
        }

        protected virtual void RegisterDialogs() { }

        protected virtual void ConfigureNavigation()
        {
            Container.RegisterSingleton<IRegionManager, RegionManager>();
            Container.RegisterSingleton<RegionAdapterMappings>();
            Container.RegisterSingleton<IRegionBehaviorFactory, RegionBehaviorFactory>();
            Container.RegisterSingleton<IRegionViewRegistry, RegionViewRegistry>();
            Container.RegisterSingleton<IRegionNavigationContentLoader, UnityRegionNavigationContentLoader>();
            Container.Register<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>();
            Container.Register<IRegionNavigationJournal, RegionNavigationJournal>();
            Container.Register<IRegionNavigationService, RegionNavigationService>();

            RegisterNavigationMappings();
            RegisterNavigationBehaviors();
            RegisterForNavigation();
        }

        protected virtual void RegisterForNavigation() { }

        protected virtual void RegisterNavigationMappings()
        {
            var mappings = Container.Resolve<RegionAdapterMappings>();
            mappings.RegisterMapping(typeof(Selector), Container.Resolve<SelectorRegionAdapter>());
            mappings.RegisterMapping(typeof(ItemsControl), Container.Resolve<ItemsControlRegionAdapter>());
            mappings.RegisterMapping(typeof(ContentControl), Container.Resolve<ContentControlRegionAdapter>());
        }

        protected virtual void RegisterNavigationBehaviors()
        {
            var behaviorFactory = Container.Resolve<IRegionBehaviorFactory>();
            behaviorFactory.AddIfMissing(BindRegionContextToDependencyObjectBehavior.BehaviorKey, typeof(BindRegionContextToDependencyObjectBehavior));
            behaviorFactory.AddIfMissing(RegionActiveAwareBehavior.BehaviorKey, typeof(RegionActiveAwareBehavior));
            behaviorFactory.AddIfMissing(SyncRegionContextWithHostBehavior.BehaviorKey, typeof(SyncRegionContextWithHostBehavior));
            behaviorFactory.AddIfMissing(RegionManagerRegistrationBehavior.BehaviorKey, typeof(RegionManagerRegistrationBehavior));
            behaviorFactory.AddIfMissing(RegionMemberLifetimeBehavior.BehaviorKey, typeof(RegionMemberLifetimeBehavior));
            behaviorFactory.AddIfMissing(ClearChildViewsRegionBehavior.BehaviorKey, typeof(ClearChildViewsRegionBehavior));
            behaviorFactory.AddIfMissing(AutoPopulateRegionBehavior.BehaviorKey, typeof(AutoPopulateRegionBehavior));
            behaviorFactory.AddIfMissing(IDestructibleRegionBehavior.BehaviorKey, typeof(IDestructibleRegionBehavior));
        }
    }
}
