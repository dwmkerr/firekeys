using System.Windows;

namespace FireKeys.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mainView.ViewModel.NewCommand.Executed += NewCommand_Executed;
            mainView.ViewModel.CloseCommand.Executed += (sender, args) => Close();
        }

        void NewCommand_Executed(object sender, Apex.MVVM.CommandEventArgs args)
        {
            var window = new NewHotKeyBinding.NewHotKeyBindingWindow();
            if (window.ShowDialog() == true)
            {
                FireKeysApplication.Instance.HotKeyBindings.Add(window.HotKeyBinding);
                FireKeysApplication.Instance.SaveSettings();
            }
        }
    }
}
