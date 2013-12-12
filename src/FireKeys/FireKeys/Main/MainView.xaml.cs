using System.Windows.Controls;
using Apex.MVVM;

namespace FireKeys.Main
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    [View(typeof (MainViewModel))]
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            ViewModel.ShowSuggestionsCommand.Executed += ShowSuggestionsCommand_Executed;
        }

        void ShowSuggestionsCommand_Executed(object sender, CommandEventArgs args)
        {
            //  TODO: provide parent window.
            FireKeysApplication.Instance.ShowSuggestionsWindow();
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel)DataContext; }
        }
    }
}
