namespace WPF.Utils.Dialogs
{
    public static class DialogParams
    {
        public static readonly string Title = "Title";

        public static class File
        {
            public static readonly string Filter = "Filter";
            public static readonly string Target = "Target";
        }

        public static class Alert
        {
            public static readonly string Content = "Content";
            public static readonly string Buttons = "Buttons";
            public static readonly string Image = "Image";
            public static readonly string DefaultResult = "Default";

            public enum AlertButtons
            {
                Ok = 0,
                OkCancel = 1,
                YesNoCancel = 3,
                YesNo = 4
            }

            public enum AlertImage
            {
                None = 0,
                Info = 64,
                Question = 32,
                Warning = 48,
                Error = 16
            }
        }
    }
}
