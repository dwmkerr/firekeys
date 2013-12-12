using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using FireKeysAPI.Imports;

namespace FireKeysAPI.Actions
{
    /// <summary>
    /// An action to execute a program, with an optional set of arguments.
    /// </summary>
    public class ExecuteProgramAction : IHotKeyAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        public void Execute()
        {
            //  Start the program.
            Task.Factory.StartNew(() =>
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Program,
                        Arguments = Arguments
                    }
                };
                process.Start();
            });
        }

        /// <summary>
        /// Creates the new action user interface.
        /// </summary>
        /// <param name="newActionUserInterface">The new action user interface.</param>
        /// <returns>The user interface for the new action.</returns>
        public FrameworkElement CreateNewActionUserInterface(INewActionUserInterface newActionUserInterface)
        {
            //  Create the user interface, set it's data context to the action instance.
            return new NewExecuteProgramActionUserInterface
            {
                DataContext = this, 
                NewActionUserInterface = newActionUserInterface
            };
        }

        /// <summary>
        /// Ensures the icons are created.
        /// </summary>
        private void EnsureIconsAreCreated()
        {
            //  If we do not have the small icon, create it.
            if (smallIcon == null)
            {
                try
                {
                    //  Get the small icon from the shell.
                    var shellFileInfo = new SHFILEINFO();
                    Win32.SHGetFileInfo(Program, 0, ref shellFileInfo, (uint) Marshal.SizeOf(shellFileInfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
                    smallIcon = Icon.FromHandle(shellFileInfo.hIcon);
                }
                catch
                {
                    //  We can't get the shell icon, use the unknown program icon.
                    smallIcon = Properties.Resources.UnknownProgram32;
                }
            }

            //  If we do not have the large icon, create it.
            if (largeIcon == null)
            {
                try
                {
                    //  Get the small icon from the shell.
                    var shellFileInfo = new SHFILEINFO();
                    Win32.SHGetFileInfo(Program, 0, ref shellFileInfo, (uint)Marshal.SizeOf(shellFileInfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
                    largeIcon = Icon.FromHandle(shellFileInfo.hIcon);
                }
                catch
                {
                    //  We can't get the shell icon, use the unknown program icon.
                    largeIcon = Properties.Resources.UnknownProgram32;
                }
            }
        }

        /// <summary>
        /// The small icon.
        /// </summary>
        private Icon smallIcon;

        /// <summary>
        /// The large icon.
        /// </summary>
        private Icon largeIcon;

        /// <summary>
        /// The program path.
        /// </summary>
        private string program;

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public string Arguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to try and focus the application window.
        /// </summary>
        /// <value>
        /// <c>true</c> to try and focus the application window; otherwise, <c>false</c>.
        /// </value>
        public bool TryAndFocusApplicationWindow { get; set; }

        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        public string Program
        {
            get { return program; }
            set
            {
                //  Has the property changed?
                if (program != value)
                {
                    //  Set the new value, but clear the icons so we re-create them when needed.
                    program = value;
                    smallIcon = null;
                    largeIcon = null;
                }
            }
        }

        /// <summary>
        /// Gets the small icon.
        /// </summary>
        /// <value>
        /// The small icon.
        /// </value>
        public Icon SmallIcon
        {
            get
            {
                EnsureIconsAreCreated();
                return smallIcon;
            }
        }

        /// <summary>
        /// Gets the large icon.
        /// </summary>
        /// <value>
        /// The large icon.
        /// </value>
        public Icon LargeIcon
        {
            get
            {
                EnsureIconsAreCreated(); 
                return largeIcon;
            }
        }

        /// <summary>
        /// Gets the display name of the action.
        /// </summary>
        /// <returns></returns>
        public string ActionDisplayName
        {
            get
            {
                return "Open a Program";
            }
        }

    }
}
