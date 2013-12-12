using System;

namespace FireKeysAPI
{
    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum HotKeyModifiers : uint
    {
        /// <summary>
        /// No modifiers.
        /// </summary>
        None = 0,

        /// <summary>
        /// The alt keyboard modifier.
        /// </summary>
        Alt = 1,

        /// <summary>
        /// The control keyboard modifier.
        /// </summary>
        Control = 2,

        /// <summary>
        /// The shift keyboard modifier.
        /// </summary>
        Shift = 4,

        /// <summary>
        /// The windows keyboard modifier.
        /// </summary>
        Windows = 8
    }
}