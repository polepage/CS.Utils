using Microsoft.Xaml.Behaviors;
using Prism.Services.Dialogs;
using System.Windows;

namespace WPF.Utils.Behaviors
{
    public class DialogAwareClose : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject.DataContext is IDialogAware dialogAware)
            {
                dialogAware.RequestClose += OnRequestClose;
            }
        }

        private void OnRequestClose(IDialogResult obj)
        {
            AssociatedObject.Close();
        }
    }
}
