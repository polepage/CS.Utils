using Microsoft.Win32;
using Prism.Services.Dialogs;
using System;
using System.Windows;

namespace WPF.Utils.Dialogs
{
    public static class DialogExtensions
    {
        public static void ShowOpenDialog(this IDialogService service, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            var openDialog = new OpenFileDialog
            {
                Title = parameters.GetValue<string>(DialogParams.Title),
                Filter = parameters.GetValue<string>(DialogParams.File.Filter),
                ValidateNames = true
            };

            var dialogResult = new DialogResult(openDialog.ShowDialog().ToButtonResult());
            if (dialogResult.Result == ButtonResult.OK)
            {
                dialogResult.Parameters.Add(DialogParams.File.Target, openDialog.FileName);
            }

            callback?.Invoke(dialogResult);
        }

        public static void ShowSaveDialog(this IDialogService service, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            var saveDialog = new SaveFileDialog
            {
                Title = parameters.GetValue<string>(DialogParams.Title),
                Filter = parameters.GetValue<string>(DialogParams.File.Filter),
                ValidateNames = true
            };

            var dialogResult = new DialogResult(saveDialog.ShowDialog().ToButtonResult());
            if (dialogResult.Result == ButtonResult.OK)
            {
                dialogResult.Parameters.Add(DialogParams.File.Target, saveDialog.FileName);
            }

            callback?.Invoke(dialogResult);
        }

        public static void ShowMessageBox(this IDialogService service, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            parameters.TryGetValue(DialogParams.Title, out string title);
            parameters.TryGetValue(DialogParams.Alert.Buttons, out DialogParams.Alert.AlertButtons buttons);
            parameters.TryGetValue(DialogParams.Alert.Image, out DialogParams.Alert.AlertImage image);
            parameters.TryGetValue(DialogParams.Alert.DefaultResult, out ButtonResult defaultResult);

            var dialogResult = new DialogResult(MessageBox.Show(
                parameters.GetValue<string>(DialogParams.Alert.Content),
                title,
                buttons.ToMessageBoxButton(),
                image.ToMessageBoxImage(),
                defaultResult.ToMessageBoxResult()
            ).ToButtonResult());

            callback?.Invoke(dialogResult);
        }

        private static ButtonResult ToButtonResult(this bool? result)
        {
            return result.GetValueOrDefault() ? ButtonResult.OK : ButtonResult.Cancel;
        }

        private static ButtonResult ToButtonResult(this MessageBoxResult result)
        {
            return (ButtonResult)result;
        }

        private static MessageBoxResult ToMessageBoxResult(this ButtonResult result)
        {
            if (result == ButtonResult.Abort || result == ButtonResult.Ignore || result == ButtonResult.Retry)
            {
                return MessageBoxResult.No;
            }

            return (MessageBoxResult)result;
        }

        private static MessageBoxButton ToMessageBoxButton(this DialogParams.Alert.AlertButtons button)
        {
            return (MessageBoxButton)button;
        }

        private static MessageBoxImage ToMessageBoxImage(this DialogParams.Alert.AlertImage image)
        {
            return (MessageBoxImage)image;
        }
    }
}
