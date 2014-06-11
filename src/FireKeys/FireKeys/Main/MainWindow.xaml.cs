using System.Linq;
using System.Windows;
using System.Windows.Documents;

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

            mainView.ViewModel.CloseCommand.Executed += (sender, args) => Close();
        }
    }
}
