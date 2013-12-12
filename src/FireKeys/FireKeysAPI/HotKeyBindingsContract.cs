using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FireKeysAPI
{
    /// <summary>
    /// A DataContract for managing a set of hot key bindings.
    /// </summary>
    [DataContract]
    public class HotKeyBindingsContract
    {
        /// <summary>
        /// Gets or sets the hot key bindings.
        /// </summary>
        /// <value>
        /// The hot key bindings.
        /// </value>
        [DataMember]
        public List<HotKeyBinding> HotKeyBindings { get; set; } 
    }
}