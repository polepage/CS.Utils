using Microsoft.Win32;
using Prism.Services.Dialogs;
using System;

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

        public static ButtonResult ToButtonResult(this bool? result)
        {
            return result.GetValueOrDefault() ? ButtonResult.OK : ButtonResult.Cancel;
        }
    }
}
