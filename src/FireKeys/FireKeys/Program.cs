using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using FireKeys.Main;
using FireKeys.Suggestions;
using FireKeysAPI;

namespace FireKeys
{
    /// <summary>
    /// The main program class.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true) == false)
            {
                //  The mutex is already created, we aren't going to run another instance
                //  if the process.
                return;
            }

            //  Load the arguments.
            var arguments = new Arguments(args);

            //  Enable visual styles and set the text rendering modes.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //  Load the settings.
            FireKeysApplication.Instance.LoadSettings();
            
            //  Create the context menu.
            CreateContextMenu();

            //  Create the tray icon.
            CreateTrayIcon();

            //  Create the menu items.
            CreateKeyBindingMenuItems();

            //  Whenever the key bindings collection changes, we'll recreate the keybinding menu items.
            FireKeysApplication.Instance.HotKeyBindings.CollectionChanged += (sender, eventArgs) => CreateKeyBindingMenuItems();

            //  If we have no key bindings, we can show the 'no bindings' tip balloon.
            trayIcon.BalloonTipClicked += (sender, eventArgs) => ShowSettingsWindow();
            if (FireKeysApplication.Instance.HotKeyBindings.Count == 0)
                ShowNoBindingsTipBalloon();
            
            //  Run the application.
            Application.Run();
            
            //  Before we terminate, make sure everything's saved.
            FireKeysApplication.Instance.SaveSettings();

            //  Release the mutex.
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Creates the context menu.
        /// </summary>
        private static void CreateContextMenu()
        {
            contextMenu = new HotkeyEnabledContextMenuStrip();
            contextMenu.Name = "Context Menu";
            
            //  Add the separator.
            contextMenu.Items.Add(new ToolStripSeparator());

            //  Add 'Add Hot Key Binding...'.
            var addHotKeyBindingItem = new ToolStripMenuItem("&New Hot Key Binding...");
            addHotKeyBindingItem.Click += (sender, args) =>
                {

                    var window = new NewHotKeyBinding.NewHotKeyBindingWindow();
                    if (window.ShowDialog() == true)
                    {
                        FireKeysApplication.Instance.HotKeyBindings.Add(window.HotKeyBinding);
                        FireKeysApplication.Instance.SaveSettings();
                    }
                };
            contextMenu.Items.Add(addHotKeyBindingItem);

            //  Add 'Settings'.
            var settingsItem = new ToolStripMenuItem("&Settings...");
            settingsItem.Click += settingsItem_Click;
            contextMenu.Items.Add(settingsItem);

            //  Add 'Exit'.
            var exitItem = new ToolStripMenuItem("E&xit");
            exitItem.Click += exitItem_Click;
            contextMenu.Items.Add(exitItem);
        }

        /// <summary>
        /// Shows the settings window.
        /// </summary>
        private static void ShowSettingsWindow()
        {
            //  Is the settings form open? If so, activate it.
            if (mainWindow != null && mainWindow.IsVisible)
            {
                mainWindow.Activate();
                return;
            }

            //  Create the settings form.
            mainWindow = new MainWindow();

            //  Show the settings form.
            mainWindow.Show();
        }

        /// <summary>
        /// Shows the no bindings tip balloon.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        static void ShowNoBindingsTipBalloon()
        {
            //  Kick off the tip balloon from the tray icon.
            trayIcon.ShowBalloonTip(5000, "FireKeys", "FireKeys is running, but there are no hot key bindings set. Click this balloon to open FireKeys and set some bindings now.", ToolTipIcon.Info);
        }

        /// <summary>
        /// Handles the Click event of the exitItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void exitItem_Click(object sender, EventArgs e)
        {
            trayIcon.Dispose();
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of the settingsItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void settingsItem_Click(object sender, EventArgs e)
        {
            ShowSettingsWindow();
        }

        /// <summary>
        /// Creates the tray icon.
        /// </summary>
        private static void CreateTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.ContextMenuStrip = contextMenu;
            trayIcon.Icon = Properties.Resources.FireKeysIcon_16;
            trayIcon.Visible = true;
            trayIcon.Text = "FireKeys - Assign hotkeys to your favourite applications";
            trayIcon.MouseDoubleClick += trayIcon_MouseDoubleClick;
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the trayIcon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        static void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowSettingsWindow();
        }

        /// <summary>
        /// Creates the accent menu items.
        /// </summary>
        public static void CreateKeyBindingMenuItems()
        {
            //  Unregister all hotkey bindings.
            contextMenu.UnregisterHotkeyBindings();

            //  Delete any previously dynamically generated items.
            foreach (var menuItemToDelete in dynamicallyGeneratedItems)
                contextMenu.Items.Remove(menuItemToDelete);
            
            //  Clear the dynamic list.
            dynamicallyGeneratedItems.Clear();

            int index = 0;

            //  Add each item.
            foreach (var item in FireKeysApplication.Instance.HotKeyBindings)
            {
                //  Create the text.
                var text = (item.IsEnabled ? item.HotKey.ToShortString() : "Disabled") + " - " + item.DisplayName;

                var image = item.Action.SmallIcon != null ? item.Action.SmallIcon.ToBitmap() : null;
                var menuItem = new ToolStripMenuItem
                {
                    Text = text,
                    Image = image,
                    Tag = item
                };
                var bindingItem = item;
                menuItem.Click += (sender, args) => bindingItem.Action.Execute();

                contextMenu.Items.Insert(index++, menuItem);
                dynamicallyGeneratedItems.Add(menuItem);
                if(item.IsEnabled)
                    contextMenu.RegisterHotKeyToMenuItem(item);
            }

            //  If we have no items, create the 'suggestions' item.
            if (dynamicallyGeneratedItems.Count == 0)
            {
                var menuItem = new ToolStripMenuItem
                {
                    Text = "Hot Key Suggestions..."
                };
                menuItem.Click += (sender, args) => FireKeysApplication.Instance.ShowSuggestionsWindow();
                contextMenu.Items.Insert(index, menuItem);
                dynamicallyGeneratedItems.Add(menuItem);
            }
        }

        /// <summary>
        /// The notify icon.
        /// </summary>
        private static NotifyIcon trayIcon;

        /// <summary>
        /// The context menu strip.
        /// </summary>
        private static HotkeyEnabledContextMenuStrip contextMenu;
        
        /// <summary>
        /// The main window.
        /// </summary>
        private static MainWindow mainWindow;

        /// <summary>
        /// The dynamically generated menu items.
        /// </summary>
        private static readonly List<ToolStripItem> dynamicallyGeneratedItems = new List<ToolStripItem>();

        /// <summary>
        /// The mutex object used to enforce single run ups only.
        /// </summary>
        private static Mutex mutex = new Mutex(true, "{E3462A51-2474-4021-BA9C-31BC1F03C075}");
    }
}