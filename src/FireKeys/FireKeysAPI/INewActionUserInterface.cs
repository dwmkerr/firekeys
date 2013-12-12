using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireKeysAPI
{
    /// <summary>
    /// A Contract that represents something that can represent the user 
    /// interface of a 'New Action' UI.
    /// </summary>
    public interface INewActionUserInterface
    {
        /// <summary>
        /// Suggests a display name for the action.
        /// </summary>
        /// <param name="suggestedDisplayName">The suggested display name.</param>
        void SetSuggestedDisplayName(string suggestedDisplayName);
    }
}
