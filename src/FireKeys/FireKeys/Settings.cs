using System;

namespace FireKeys
{
    /// <summary>
    /// Represent the settings for FireKeys.
    /// </summary>
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to start FireKeys on windows logon.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if start on windows logon; otherwise, <c>false</c>.
        /// </value>
        public bool StartOnWindowsLogon { get; set; }
    }
}
