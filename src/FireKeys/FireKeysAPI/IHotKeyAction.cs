using System.Drawing;
using System.Windows;

namespace FireKeysAPI
{
    /// <summary>
    /// Represents a hot key action.
    /// </summary>
    public interface IHotKeyAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        void Execute();

        /// <summary>
        /// Gets the small icon.
        /// </summary>
        /// <value>
        /// The small icon.
        /// </value>
        Icon SmallIcon { get; }

        /// <summary>
        /// Gets the large icon.
        /// </summary>
        /// <value>
        /// The large icon.
        /// </value>
        Icon LargeIcon { get; }

        /// <summary>
        /// Gets the display name of the action.
        /// </summary>
        /// <returns></returns>
        string ActionDisplayName { get; }

        /// <summary>
        /// Creates the new action user interface.
        /// </summary>
        /// <param name="newActionUserInterface">The new action user interface.</param>
        /// <returns></returns>
        FrameworkElement CreateNewActionUserInterface(INewActionUserInterface newActionUserInterface);
    }
}
