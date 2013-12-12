using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FireKeys.Suggestions
{
    /// <summary>
    /// Interaction logic for SuggestionsWindow.xaml
    /// </summary>
    public partial class SuggestionsWindow : Window
    {
        public SuggestionsWindow()
        {
            InitializeComponent();

            viewModel.OKCommand.Executed += (sender, args) =>
                {
                    DialogResult = true;
                    Close();
                };
            viewModel.CancelCommand.Executed += (sender, args) =>
                {
                    DialogResult = false;
                    Close();
                };

            Loaded += SuggestionsWindow_Loaded;
            Closing += SuggestionsWindow_Closing;
        }

        void SuggestionsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            acceptedSuggestions.Clear();
            acceptedSuggestions.AddRange(from s in viewModel.Suggestions where s.ApplySuggestion select s.Model);
        }

        void SuggestionsWindow_Loaded(object sender, RoutedEventArgs e)
        {

            viewModel.CreateSuggestions(Suggestions);
        }

        private readonly List<BindingSuggestion> acceptedSuggestions = new List<BindingSuggestion>(); 

        public IEnumerable<BindingSuggestion> Suggestions { get; set; }



        public IEnumerable<BindingSuggestion> AcceptedSuggestions { get { return acceptedSuggestions; } }

        void CancelCommand_Executed(object sender, Apex.MVVM.CommandEventArgs args)
        {
            DialogResult = false;
            Close();
        }

        void OKCommand_Executed(object sender, Apex.MVVM.CommandEventArgs args)
        {
            DialogResult = true;
            Close();
        }
    }
}
