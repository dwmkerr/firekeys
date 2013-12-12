using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Apex.MVVM;
using FireKeysAPI;
using FireKeysAPI.Actions;

namespace FireKeys.Suggestions
{
    [ViewModel]
    public class SuggestionsViewModel : ViewModel
    {
        public SuggestionsViewModel()
        {
            //  Create the commands.
            OKCommand = new Command(DoOKCommand);
            CancelCommand = new Command(DoCancelCommand);
        }

        
        /// <summary>
        /// The Suggestions observable collection.
        /// </summary>
        private readonly ObservableCollection<SuggestionViewModel> SuggestionsProperty =
          new ObservableCollection<SuggestionViewModel>();

        /// <summary>
        /// Gets the Suggestions observable collection.
        /// </summary>
        /// <value>The Suggestions observable collection.</value>
        public ObservableCollection<SuggestionViewModel> Suggestions
        {
            get { return SuggestionsProperty; }
        }

        /// <summary>
        /// Performs the OK command.
        /// </summary>
        /// <param name="parameter">The OK command parameter.</param>
        private void DoOKCommand(object parameter)
        {
        }

        /// <summary>
        /// Gets the OK command.
        /// </summary>
        /// <value>The value of .</value>
        public Command OKCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs the Cancel command.
        /// </summary>
        /// <param name="parameter">The Cancel command parameter.</param>
        private void DoCancelCommand(object parameter)
        {
        }

        /// <summary>
        /// Gets the Cancel command.
        /// </summary>
        /// <value>The value of .</value>
        public Command CancelCommand
        {
            get;
            private set;
        }

        public void CreateSuggestions(IEnumerable<BindingSuggestion> suggestions)
        {
            //  Go through each suggestion that exists...
            foreach (var suggestion in suggestions)
            {
                var path = suggestion.FindValidSuggestionPath();
                if (string.IsNullOrEmpty(path)) continue;

                //  Create a VM for it.
                var newVm = new SuggestionViewModel();
                newVm.FromModel(suggestion);
                newVm.HotKeyBinding = new HotKeyBinding
                    {
                        DisplayName = suggestion.DisplayName,
                        HotKey = suggestion.HotKey,
                        Action = new ExecuteProgramAction
                            {
                                Program = path
                            }
                    };
                Suggestions.Add(newVm);
            }
        }
    }
}
