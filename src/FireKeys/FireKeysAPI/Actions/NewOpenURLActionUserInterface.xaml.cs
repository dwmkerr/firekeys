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
    /// Interaction logic for NewOpenURLActionUserInterface.xaml
    /// </summary>
    public partial class NewOpenURLActionUserInterface : UserControl
    {
        public NewOpenURLActionUserInterface()
        {
            InitializeComponent();
        }

        public INewActionUserInterface NewActionUserInterface { get; set; }

        private void textBoxUrl_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBoxUrl.Text))
                {
                    var uri = new Uri(textBoxUrl.Text);
                    if (!string.IsNullOrEmpty(uri.Host) && NewActionUserInterface != null)
                    {
                        NewActionUserInterface.SetSuggestedDisplayName(uri.Host);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
