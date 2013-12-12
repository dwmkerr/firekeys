using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml.Serialization;
using FireKeysAPI;
using FireKeysAPI.Actions;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace FireKeys
{
    /// <summary>
    /// The <see cref="FireKeysApplication"/> Singleton class.
    /// </summary>
    public sealed class FireKeysApplication
    {
        /// <summary>
        /// The Singleton instace. Declared 'static readonly' to enforce
        /// a single instance only and lazy initialisation.
        /// </summary>
        private static readonly FireKeysApplication instance = new FireKeysApplication();

        /// <summary>
        /// Initializes a new instance of the <see cref="FireKeysApplication"/> class.
        /// Declared private to enforce a single instance only.
        /// </summary>
        private FireKeysApplication()
        {
        }

        public IEnumerable<Type> GetActionTypes()
        {
            return new[] {typeof (OpenFolderAction), typeof (OpenURLAction), typeof (ExecuteProgramAction)};
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            //  Load suggestions.
            LoadSuggestionsFile();
            
            //  Load the settings file.
            LoadSettingsFile();

            //  Load the bindings file.
            LoadBindingsFile();

            //  Apply the windows settings.
            ApplyWindowsSettings();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            SaveSettingsFile();

            SaveBindingsFile();
        }

        /// <summary>
        /// Loads the suggestions.
        /// </summary>
        private void LoadSuggestionsFile()
        {
            //  Clear existing suggestions.
            bindingSuggestions = new List<BindingSuggestion>();

            //  Attempt to load the suggestions from the manifest resource stream.
            try
            {
                using (var suggestionsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FireKeys.Suggestions.xml"))
                {
                    //  Deserialize the suggestions.
                    var serializer = new XmlSerializer(typeof(List<BindingSuggestion>));
                    bindingSuggestions = (List<BindingSuggestion>)serializer.Deserialize(suggestionsStream);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Loads the settings file.
        /// </summary>
        private void LoadSettingsFile()
        {
            //  Do we have a settings file?
            if (File.Exists(GetSettingsPath()))
            {
                //  Try and load it.
                try
                {
                    using (var stream = new FileStream(GetSettingsPath(), FileMode.Open))
                    {
                        //  Create a serializer.
                        var serializer = new XmlSerializer(typeof(Settings));

                        //  Read the settings.
                        Settings = (Settings)serializer.Deserialize(stream);

                    }
                }
                catch (Exception exception)
                {
                    //  Trace the exception.
                    System.Diagnostics.Trace.WriteLine("Exception loading settings file: " + exception);

                    //  Warn the user.
                    MessageBox.Show("Failed to load the settings file.", "Error");
                }
            }
            else
            {
                //  We have no settings file - create the default settings.
                CreateDefaultSettings();
                SaveSettings();
            }
        }

        private void LoadBindingsFile()
        {
            hotKeyBindings.Clear();

            //  Do we have a settings file?
            if (File.Exists(GetBindingsPath()))
            {
                //  Try and load it.
                try
                {
                    using (var stream = new FileStream(GetBindingsPath(), FileMode.Open))
                    {
                        //  Create a serializer.
                        var serializer = CreateHotKeyBindingsSerializer();

                        //  Deserize the contract, set the bindings.
                        var contract = (HotKeyBindingsContract)serializer.ReadObject(stream);
                        if(contract != null)
                            contract.HotKeyBindings.ForEach(hkb => hotKeyBindings.Add(hkb));
                    }
                }
                catch (Exception exception)
                {
                }
            }
        }

        private void SaveSettingsFile()
        {
            //  Try and open the strem.
            try
            {
                using (var stream = new FileStream(GetSettingsPath(), FileMode.Create))
                {
                    //  Create a serializer.
                    var serializer = new XmlSerializer(typeof(Settings));

                    //  Write the settings.
                    serializer.Serialize(stream, Settings);
                }
            }
            catch (Exception exception)
            {
                //  Trace the exception.
                System.Diagnostics.Trace.WriteLine("Exception saving settings file: " + exception);

                //  Warn the user.
                MessageBox.Show("Failed to save the settings file.", "Error");
            }
        }

        private void SaveBindingsFile()
        {
            //  Try and save it.
                try
                {
                    using (var stream = new FileStream(GetBindingsPath(), FileMode.Create))
                    {
                        //  Create a serializer.
                        var serializer = CreateHotKeyBindingsSerializer();

                        //  Deserize the contract, set the bindings.
                        var contract = new HotKeyBindingsContract { HotKeyBindings = (from hkb in hotKeyBindings select hkb).ToList()};
                        serializer.WriteObject(stream, contract);
                        stream.Flush();
                    }
                }
                catch (Exception exception)
                {
                }
        }

        private DataContractSerializer CreateHotKeyBindingsSerializer()
        {
            //  Create a serializer.
            var serializer = new DataContractSerializer(typeof(HotKeyBindingsContract), GetActionTypes());
            return serializer;
        }

        /// <summary>
        /// Applies the windows settings, such as the 'start on logon'.
        /// </summary>
        public void ApplyWindowsSettings()
        {
            //  Create a startup manager.
            var startupManager = new ApplicationAutoStartupManager();
            startupManager.ApplicationPath = Application.ExecutablePath;

            //  Are we set to start on windows startup?
            if (Settings.StartOnWindowsLogon)
                startupManager.AddToStartupFolder();
            else
                startupManager.RemoveFromStartupFolder();
        }

        /// <summary>
        /// Creates the default settings.
        /// </summary>
        private void CreateDefaultSettings()
        {
            Settings = new Settings
                {
                    StartOnWindowsLogon = true
                };
        }

        public void ShowSuggestionsWindow(Window parentWindow = null)
        {
            //  Create a suggestions window.
            var suggestionsWindow = new Suggestions.SuggestionsWindow
                {
                    Owner = parentWindow
                };
            
            //  Provide the full set of suggestions...
            suggestionsWindow.Suggestions = this.Suggestions;
            
            //  Show the window - if not OKd then return.
            if (suggestionsWindow.ShowDialog() != true)
                return;

            //  Otherwise, create a binding for each suggestions.
            foreach (var suggestion in suggestionsWindow.AcceptedSuggestions)
            {
                //  Add each suggestion.
                var binding = new HotKeyBinding
                    {
                        DisplayName = suggestion.DisplayName,
                        HotKey = suggestion.HotKey,
                        IsEnabled = true,
                        Action = new ExecuteProgramAction
                            {
                                Program = suggestion.FindValidSuggestionPath()
                            }
                    };
                HotKeyBindings.Add(binding);
            }

            SaveSettings();
        }

        /// <summary>
        /// Gets the ApplicationContext Singleton Instance.
        /// </summary>
        public static FireKeysApplication Instance
        {
            get { return instance; }
        }
        
        /// <summary>
        /// Gets the settings path.
        /// </summary>
        /// <returns></returns>
        private string GetSettingsPath()
        {
            var executableDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (executableDirectory == null)
                throw new InvalidOperationException("Cannot get the executable directory.");
            return Path.Combine(executableDirectory, @"Settings.xml");
        }

        /// <summary>
        /// Gets the settings path.
        /// </summary>
        /// <returns></returns>
        private string GetBindingsPath()
        {
            var executableDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (executableDirectory == null)
                throw new InvalidOperationException("Cannot get the executable directory.");
            return Path.Combine(executableDirectory, @"Bindings.xml");
        }

        /// <summary>
        /// The binding suggestions.
        /// </summary>
        private List<BindingSuggestion> bindingSuggestions;

        /// <summary>
        /// The hot key bindings
        /// </summary>
        private readonly ObservableCollection<HotKeyBinding> hotKeyBindings = new ObservableCollection<HotKeyBinding>(); 

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// Gets the suggestions.
        /// </summary>
        /// <value>
        /// The suggestions.
        /// </value>
        public IEnumerable<BindingSuggestion> Suggestions
        {
            get { return bindingSuggestions; }
        }

        /// <summary>
        /// Gets the hot key bindings.
        /// </summary>
        /// <value>
        /// The hot key bindings.
        /// </value>
        public ObservableCollection<HotKeyBinding> HotKeyBindings
        {
            get { return hotKeyBindings; }
        }
    }
}
