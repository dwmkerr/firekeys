using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FireKeys;

namespace HotKeyControl
{
    public partial class HotKeyControl: UserControl
    {
        public HotKeyControl()
        {
            InitializeComponent();

            
        }

        private void textBoxKey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            //  Handle modifiers.
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    checkBoxShift.Checked = !checkBoxShift.Checked;
                    break;
                case Keys.Menu:
                    checkBoxAlt.Checked = !checkBoxAlt.Checked;
                    break;
                case Keys.ControlKey:
                    checkBoxCtrl.Checked = !checkBoxCtrl.Checked;
                    break;
                default:
                    hotKey = e.KeyCode;
                    textBoxKey.Text = e.KeyCode.ToString();
                    break;
            }
        }

        private void textBoxKey_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private Keys hotKey;

        public HotKeyModifiers Modifiers
        {
            get 
            { 
                var modifiers = HotKeyModifiers.None;
                modifiers |= checkBoxCtrl.Checked ? HotKeyModifiers.Control : HotKeyModifiers.None;
                modifiers |= checkBoxAlt.Checked ? HotKeyModifiers.Alt : HotKeyModifiers.None;
                modifiers |= checkBoxShift.Checked ? HotKeyModifiers.Shift : HotKeyModifiers.None;
                modifiers |= checkBoxWindows.Checked ? HotKeyModifiers.Windows : HotKeyModifiers.None;
                return modifiers;
            }
            set
            {
                checkBoxCtrl.Checked = value.HasFlag(HotKeyModifiers.Control);
                checkBoxAlt.Checked = value.HasFlag(HotKeyModifiers.Alt);
                checkBoxShift.Checked = value.HasFlag(HotKeyModifiers.Shift);
                checkBoxWindows.Checked = value.HasFlag(HotKeyModifiers.Windows);
            }
        }

        public Keys HotKey
        {
            get { return hotKey; }
            set 
            { 
                hotKey = value;
                textBoxKey.Text = hotKey.ToString();
            }
        }
    }
}
