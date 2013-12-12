using System;
using System.Runtime.Serialization;

namespace FireKeysAPI
{
    /// <summary>
    /// A HotKey binding represents a binding between a hotkey and an action.
    /// </summary>
    [DataContract]
    public class HotKeyBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyBinding"/> class.
        /// </summary>
        public HotKeyBinding()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Gets or sets the hot key.
        /// </summary>
        /// <value>
        /// The hot key.
        /// </value>
        [DataMember]
        public HotKey HotKey { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        [DataMember]
        public IHotKeyAction Action { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the windows hotkey id.
        /// </summary>
        /// <value>
        /// The windows hotkey id.
        /// </value>
        [DataMember]
        public int WindowsHotkeyId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this binding is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this binding is enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the hot key registered successfully.
        /// </summary>
        /// <value>
        /// <c>true</c> if the hot key registered successfully; otherwise, <c>false</c>.
        /// </value>
        public bool HotKeyRegisteredSuccessfully { get; set; }
    }
}
