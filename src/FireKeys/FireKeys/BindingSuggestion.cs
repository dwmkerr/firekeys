using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FireKeysAPI;

namespace FireKeys
{
    /// <summary>
    /// Represents a suggestion for a hot key binding.
    /// </summary>
    public class BindingSuggestion
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the check suggestion paths.
        /// These are paths that the program may reside at. They can include environment variables.
        /// </summary>
        /// <value>
        /// The check suggestion paths.
        /// </value>
        public List<string> CheckSuggestionPaths { get; set; }

        /// <summary>
        /// Gets or sets the hot key.
        /// </summary>
        /// <value>
        /// The hot key.
        /// </value>
        public HotKey HotKey { get; set; }

        public string FindValidSuggestionPath()
        {
            //  Go through each path.
            return CheckSuggestionPaths.Select(Environment.ExpandEnvironmentVariables).FirstOrDefault(File.Exists);
        }
    }
}
