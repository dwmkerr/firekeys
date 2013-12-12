using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;

namespace FireKeysAPI.Actions
{
    /// <summary>
    /// An action to open an URL.
    /// </summary>
    public class OpenURLAction : IHotKeyAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        public void Execute()
        {
            //  Open the URL.
            Task.Factory.StartNew(() =>
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = URL
                    }
                };
                process.Start();
            });
        }

        /// <summary>
        /// Creates the new action user interface.
        /// </summary>
        /// <param name="newActionUserInterface">The new action user interface.</param>
        /// <returns>The new action user interface.</returns>
        public FrameworkElement CreateNewActionUserInterface(INewActionUserInterface newActionUserInterface)
        {
            return new NewOpenURLActionUserInterface { DataContext = this, NewActionUserInterface = newActionUserInterface };
        }
        
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string URL { get; set; }

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
                return Properties.Resources.Webpage32;
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
                return Properties.Resources.Webpage32;
            }
        }

        /// <summary>
        /// Gets the display name of the action.
        /// </summary>
        /// <returns></returns>
        public string ActionDisplayName
        {
            get { return "Open a URL"; }
        }
    }
}
