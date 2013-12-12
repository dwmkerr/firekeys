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
using FireKeysAPI;

namespace FireKeys.NewHotKeyBinding
{
    /// <summary>
    /// Interaction logic for NewHotKeyBindingView.xaml
    /// </summary>
    public partial class NewHotKeyBindingView : UserControl
    {
        public NewHotKeyBindingView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public NewHotKeyBindingViewModel ViewModel { get { return (NewHotKeyBindingViewModel) DataContext; } }

    }
}
