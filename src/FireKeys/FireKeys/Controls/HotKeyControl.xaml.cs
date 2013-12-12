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
          DependencyProperty.Register("HotKey", typeof(HotKey), typeof(HotKeyControl),
          new PropertyMetadata(default(HotKey), new PropertyChangedCallback(OnHotKeyChanged)));

        /// <summary>
        /// Gets or sets HotKey.
        /// </summary>
        /// <value>The value of HotKey.</value>
        public HotKey HotKey
        {
            get { return (HotKey)GetValue(HotKeyProperty); }
            set { SetValue(HotKeyProperty, value); }
        }

        /// <summary>
        /// Called when HotKey is changed.
        /// </summary>
        /// <param name="o">The dependency object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnHotKeyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var me = (HotKeyControl)o;

            var newHotKey = args.NewValue as HotKey;
            if (newHotKey != null)
            {
                me.checkBoxCtrl.IsChecked = newHotKey.Modifiers.HasFlag(HotKeyModifiers.Control);
                me.checkBoxAlt.IsChecked = newHotKey.Modifiers.HasFlag(HotKeyModifiers.Alt);
                me.checkBoxShift.IsChecked = newHotKey.Modifiers.HasFlag(HotKeyModifiers.Shift);
                me.checkBoxWin.IsChecked = newHotKey.Modifiers.HasFlag(HotKeyModifiers.Windows);
                me.textBoxHotKey.Text = KeyInterop.KeyFromVirtualKey((int) newHotKey.HotKeyCharacter).ToString();
            }
        }

        private void TextBoxHotKey_OnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            //  If we have any of the modifiers pressed, we can activate the check box.
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                checkBoxCtrl.IsChecked = !checkBoxCtrl.IsChecked;
            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
                checkBoxAlt.IsChecked = !checkBoxAlt.IsChecked;
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                checkBoxShift.IsChecked = !checkBoxShift.IsChecked;
            if (e.Key == Key.LWin || e.Key == Key.RWin)
                checkBoxWin.IsChecked = !checkBoxWin.IsChecked;

            //  Handle modifiers.
            switch (e.Key)
            {
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LWin:
                case Key.RWin:
                case Key.System:
                    break;
                default:
                    textBoxHotKey.Text = e.Key.ToString();
                    if (HotKey != null)
                    {
                        HotKey.HotKeyCharacter = (System.Windows.Forms.Keys) KeyInterop.VirtualKeyFromKey(e.Key);
                    }
                    break;
            }
        }

        private void TextBoxHotKey_OnKeyUp(object sender, KeyEventArgs e)
        {
        }

        private void OnModiferCheckChanged(object sender, RoutedEventArgs e)
        {
            UpdateHotKeyFromUI();
        }

        private void UpdateHotKeyFromUI()
        {
            if (HotKey != null)
            {
                HotKey.Modifiers = HotKeyModifiers.None;
                HotKey.Modifiers |= checkBoxCtrl.IsChecked == true ? HotKeyModifiers.Control : HotKeyModifiers.None;
                HotKey.Modifiers |= checkBoxAlt.IsChecked == true ? HotKeyModifiers.Alt : HotKeyModifiers.None;
                HotKey.Modifiers |= checkBoxShift.IsChecked == true ? HotKeyModifiers.Shift : HotKeyModifiers.None;
                HotKey.Modifiers |= checkBoxWin.IsChecked == true ? HotKeyModifiers.Windows : HotKeyModifiers.None;
            }
        }
    }
}
