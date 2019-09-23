using Prism.Services.Dialogs;
using System.Windows;

namespace WPF.Utils.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogHost.xaml
    /// </summary>
    public partial class DialogHost : Window, IDialogWindow
    {
        public DialogHost()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }
}
