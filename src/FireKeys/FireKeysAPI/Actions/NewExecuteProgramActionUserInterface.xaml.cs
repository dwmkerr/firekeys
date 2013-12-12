using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FireKeysAPI.Actions
{
    /// <summary>
    /// Interaction logic for NewExecuteProgramActionUserInterface.xaml
    /// </summary>
    public partial class NewExecuteProgramActionUserInterface : UserControl
    {
        public NewExecuteProgramActionUserInterface()
        {
            InitializeComponent();
        }

        public INewActionUserInterface NewActionUserInterface { get; set; }

        private void PathTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //  If we've got a file name, suggest it as the action name.
            var fileName = System.IO.Path.GetFileNameWithoutExtension(textBoxPath.Text);
            if(!string.IsNullOrEmpty(fileName) && NewActionUserInterface != null)
                NewActionUserInterface.SetSuggestedDisplayName(fileName);

        }
    }
}
