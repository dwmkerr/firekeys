using System.Windows;
using FireKeysAPI;
using FireKeysAPI.Actions;

namespace FireKeys.NewHotKeyBinding
{
    /// <summary>
    /// Interaction logic for NewHotKeyBindingWindow.xaml
    /// </summary>
    public partial class NewHotKeyBindingWindow : Window
    {
        public NewHotKeyBindingWindow()
        {
            InitializeComponent();

            newHotKeyBindingView.ViewModel.OKCommand.Executed += (sender, args) =>
                {
                    DialogResult = true;
                    Close();
                };
            newHotKeyBindingView.ViewModel.CancelCommand.Executed += (sender, args) =>
            {
                DialogResult = false;
                Close();
            };

            Closed += NewHotKeyBindingWindow_Closed;
        }

        void NewHotKeyBindingWindow_Closed(object sender, System.EventArgs e)
        {
            if (DialogResult != true)
                return;

            var vm = newHotKeyBindingView.ViewModel;

            //  Create the hotkey binding.
            HotKeyBinding = new HotKeyBinding
                {
                    HotKey = vm.HotKey,
                    DisplayName = vm.DisplayName,
                    Action = vm.SelectedAction
                };
        }

        public HotKeyBinding HotKeyBinding { get; private set; }
    }
}
