using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FireKeys.NewHotKeyBinding;
using FireKeysAPI;
using UserControl = System.Windows.Controls.UserControl;

namespace FireKeys.Controls
{
    /// <summary>
    /// Interaction logic for HotKeyControl.xaml
    /// </summary>
    public partial class HotKeyControl : UserControl
    {
        public HotKeyControl()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// The DependencyProperty for the HotKey property.
        /// </summary>
        public static readonly DependencyProperty HotKeyProperty =
          DependencyProperty.Register("HotKey", typeof(NewHotKeyBindingViewModel), typeof(HotKeyControl),
          new PropertyMetadata(default(NewHotKeyBindingViewModel)));

        /// <summary>
        /// Gets or sets HotKey.
        /// </summary>
        /// <value>The value of HotKey.</value>
        public NewHotKeyBindingViewModel HotKey
        {
            get { return (NewHotKeyBindingViewModel)GetValue(HotKeyProperty); }
            set
            {
                SetValue(HotKeyProperty, value);
            }
        }
    }

    public class KeyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = (int)value;
            return KeyInterop.KeyFromVirtualKey(key).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textboxValue = value.ToString();
            Key key;
            if(Enum.TryParse(textboxValue,true,out key))
            {
                return (Keys)KeyInterop.VirtualKeyFromKey(key);
            }
            return null;
        }
    }
}
