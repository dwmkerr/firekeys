using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace FireKeys
{
    /// <summary>
    /// A utility class that aids in the management of auto-start applications.
    /// </summary>
    public class ApplicationAutoStartupManager
    {
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        private void ValidateParameters()
        {
            if (string.IsNullOrEmpty(ApplicationPath))
                throw new InvalidOperationException("The ApplicationPath is null or empty.");
            if (File.Exists(ApplicationPath) == false)
                throw new InvalidOperationException("The ApplicationPath does not point to a file.");
        }

        /// <summary>
        /// Gets the shortcut path.
        /// </summary>
        /// <returns></returns>
        private string GetShortcutPath()
        {
            //  Get the startup folder path.
            var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            //  Create the shortcut name and path.
            var shortcutName = Path.GetFileNameWithoutExtension(ApplicationPath) + ".lnk";
            var shortcutPath = Path.Combine(startupFolder, shortcutName);

            //  Return the shortcut path.
            return shortcutPath;
        }

        /// <summary>
        /// Adds to startup folder.
        /// </summary>
        public void AddToStartupFolder()
        {
            //  Validate the parameters.
            ValidateParameters();

            //  Create the shell.
            var shell = new WshShellClass();
            
            //  Create the shortcut.
            var shortcut = (IWshShortcut)shell.CreateShortcut(GetShortcutPath());

            //  Set the target path.
            shortcut.TargetPath = ApplicationPath;

            //  Set the description.
            shortcut.Description = "Launch " + Path.GetFileNameWithoutExtension(ApplicationPath);
            
            // Location for the shortcut's icon
            //MyShortcut.IconLocation = Application.StartupPath + @"\app.ico";
            shortcut.IconLocation = ApplicationPath + ",0";

            //  Save the shortcut.
            shortcut.Save();
        }

        /// <summary>
        /// Removes from startup folder.
        /// </summary>
        public void RemoveFromStartupFolder()
        {
            //  Get the shortcut path.
            var shortcutPath = GetShortcutPath();

            //  Do we have it?
            if(File.Exists(shortcutPath))
                File.Delete(shortcutPath);
        }

        /// <summary>
        /// Determines whether the path is in the startup folder.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the path is in the startup folder; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInStartupFolder()
        {
            return File.Exists(GetShortcutPath());
        }

        /// <summary>
        /// Adds to startup registry.
        /// </summary>
        public void AddToStartupRegistry()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes from startup registry.
        /// </summary>
        public void RemoveFromStartupRegistry()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the path is is in the startup section of the registry.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the path starts up in the registry; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInStartupRegistry()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the application path.
        /// </summary>
        /// <value>
        /// The application path.
        /// </value>
        public string ApplicationPath { get; set; }
    }
}
