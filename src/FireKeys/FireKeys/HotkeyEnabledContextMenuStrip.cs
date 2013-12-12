using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FireKeysAPI;

namespace FireKeys
{
    /// <summary>
    /// A Hotkey Enabled Context Menu Strip is a Context Menu Strip
    /// that can be opened via a Windows Hotkey.
    /// </summary>
    public class HotkeyEnabledContextMenuStrip : ContextMenuStrip
    {
        /// <summary>
        /// Registers the hot key.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="id">The id.</param>
        /// <param name="fsModifiers">The fs modifiers.</param>
        /// <param name="vk">The vk.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        /// <summary>
        /// Unregisters the hot key.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        protected override void Dispose(bool disposing)
        {
            //  Free unmanaged resources.
            UnregisterHotkeyBindings();

            base.Dispose(disposing);
        }


        public void UnregisterHotkeyBindings()
        {
            //  Free unmanaged resources.
            foreach (var keyBinding in hotKeyCodesToHotKeys.Values)
                UnregisterHotKey(Handle, keyBinding.WindowsHotkeyId);
            hotKeyCodesToHotKeys.Clear();
        }

        public void RegisterHotKeyToMenuItem(HotKeyBinding hotKey)
        {
            //  Set the id.
            hotKey.WindowsHotkeyId = uniqueHotkeyIdCounter++;

            //  Create a hotkey registration.
            hotKey.HotKeyRegisteredSuccessfully = RegisterHotKey(Handle, 
                hotKey.WindowsHotkeyId, (uint) hotKey.HotKey.Modifiers, (uint) hotKey.HotKey.HotKeyCharacter);

            hotKeyCodesToHotKeys[hotKey.WindowsHotkeyId] = hotKey;
        }
        
        /// <summary>
        /// The WindowProc.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message"/> to process.</param>
        protected override void WndProc(ref Message m)
        {
            //  Call the base.
            base.WndProc(ref m);

            //  Have we hit the hotkey?
            if (m.Msg == WM_HOTKEY)
            {
                //  Get the hotkey id.
                var hotKeyId = (int) m.WParam;

                //  Get the hotkey.
                HotKeyBinding hotKey;
                if (hotKeyCodesToHotKeys.TryGetValue(hotKeyId, out hotKey) == false)
                {
                    //  TODO: log unknown hotkey.
                    return;
                }

                //  Activate the hotkey.
                hotKey.Action.Execute();
            }
        }

        /// <summary>
        /// An id for our application's hotkey.
        /// </summary>
        private int hotkeyUniqueId = 123;
        
        private int uniqueHotkeyIdCounter = 140;

        /// <summary>
        /// Windows message code.
        /// </summary>
        private const int WM_HOTKEY = 0x0312;

        /// <summary>
        /// The hot key codes to hot keys dictionary.
        /// </summary>
        private readonly Dictionary<int, HotKeyBinding> hotKeyCodesToHotKeys = new Dictionary<int, HotKeyBinding>(); 

        /// <summary>
        /// Gets or sets the initial position control.
        /// </summary>
        /// <value>
        /// The initial position control.
        /// </value>
        public Control InitialPositionControl { get; set; }
    }
}