using System.Text;
using System.Windows.Forms;

namespace FireKeysAPI
{
    public class HotKey
    {
        /// <summary>
        /// Gets or sets the hot key character.
        /// </summary>
        /// <value>
        /// The hot key character.
        /// </value>
        public Keys HotKeyCharacter { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        /// <value>
        /// The modifiers.
        /// </value>
        public HotKeyModifiers Modifiers { get; set; }

        /// <summary>
        /// To the short string.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string ToShortString()
        {
            var builder = new StringBuilder();
            if (Modifiers.HasFlag(HotKeyModifiers.Control))
                builder.Append("Ctrl + ");
            if (Modifiers.HasFlag(HotKeyModifiers.Alt))
                builder.Append("Alt + ");
            if (Modifiers.HasFlag(HotKeyModifiers.Shift))
                builder.Append("Shift + ");
            if (Modifiers.HasFlag(HotKeyModifiers.Windows))
                builder.Append("Win + ");
            builder.Append(HotKeyCharacter.ToString());
            return builder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToShortString();
        }
    }
}
