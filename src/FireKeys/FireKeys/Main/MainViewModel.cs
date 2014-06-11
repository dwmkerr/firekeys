using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using Apex.MVVM;
using FireKeys.NewHotKeyBinding;
using FireKeysAPI;

namespace FireKeys.Main
{
    [ViewModel]
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            CloseCommand = new Command(DoCloseCommand);
            NewCommand = new Command(DoNewCommand);
            DeleteHotKeyBindingCommand = new Command(DoDeleteHotKeyBindingCommand);
            ShowSuggestionsCommand = new Command(DoShowSuggestionsCommand);
            EditHotKeyBindingCommand = new Command(DoEditCommand);

            UpdateHotHeyBindings();

            //  When the bindings change in the model, update them in the vm.
            FireKeysApplication.Instance.HotKeyBindings.CollectionChanged += (sender, args) => UpdateHotHeyBindings();

            HotKeyBindings.CollectionChanged += (sender, args) => { IsHotKeyBindingsCollectionEmpty = !HotKeyBindings.Any(); };

            IsHotKeyBindingsCollectionEmpty = HotKeyBindings.Count == 0;
        }

        public void UpdateHotHeyBindings()
        {
            //  Clear the hotkey bindings.
            HotKeyBindings.Clear();

            //  Add each hotkey binding.
            foreach (var hotKeyBinding in FireKeysApplication.Instance.HotKeyBindings)
            {
                var vm = new HotKeyBindingViewModel();
                vm.FromModel(hotKeyBinding);
                HotKeyBindings.Add(vm);
            }
        }

        
        /// <summary>
        /// The Suggestions observable collection.
        /// </summary>
        private readonly ObservableCollection<HotKeyBindingViewModel> HotKeyBindingsProperty =
          new ObservableCollection<HotKeyBindingViewModel>();

        /// <summary>
        /// Gets the Suggestions observable collection.
        /// </summary>
        /// <value>The Suggestions observable collection.</value>
        public ObservableCollection<HotKeyBindingViewModel> HotKeyBindings
        {
            get { return HotKeyBindingsProperty; }
        }

        /// <summary>
        /// Performs the Close command.
        /// </summary>
        /// <param name="parameter">The Close command parameter.</param>
        private void DoCloseCommand(object parameter)
        {
        }

        /// <summary>
        /// Gets the Close command.
        /// </summary>
        /// <value>The value of .</value>
        public Command CloseCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs the New command.
        /// </summary>
        /// <param name="parameter">The New command parameter.</param>
        private void DoNewCommand(object parameter)
        {
            var window = new NewHotKeyBinding.NewHotKeyBindingWindow();
            if (window.ShowDialog() == true)
            {
                FireKeysApplication.Instance.HotKeyBindings.Add(window.HotKeyBinding);
                FireKeysApplication.Instance.SaveSettings();
            }
        }

        /// <summary>
        /// Performs the Edit command.
        /// </summary>
        /// <param name="parameter">The edit command parameter.</param>
        private void DoEditCommand(object parameter)
        {
            var binding = parameter as HotKeyBindingViewModel;
            if (binding != null)
            {
                var model = new NewHotKeyBindingViewModel(binding.Model.HotKey)
                                                {
                                                    DisplayName = binding.Model.DisplayName,
                                                    SelectedAction = binding.Model.Action
                                                };
                model.Actions.Clear();
                model.Actions.Add(binding.Model.Action);
                var window = new NewHotKeyBindingWindow(model);

                if (window.ShowDialog() == true)
                {
                    binding.HotKeyBinding = window.HotKeyBinding;
                    FireKeysApplication.Instance.SaveSettings();
                }
            }
        }

        /// <summary>
        /// Gets the New command.
        /// </summary>
        /// <value>The value of .</value>
        public Command NewCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs the DeleteHotKeyBinding command.
        /// </summary>
        /// <param name="parameter">The DeleteHotKeyBinding command parameter.</param>
        private void DoDeleteHotKeyBindingCommand(object parameter)
        {
            //  We must have a binding provided.
            var binding = (HotKeyBindingViewModel) parameter;

            //  Remove the binding from the model, this will cause the view model to update.
            FireKeysApplication.Instance.HotKeyBindings.Remove(binding.Model);
            FireKeysApplication.Instance.SaveSettings();
        }

        /// <summary>
        /// Gets the DeleteHotKeyBinding command.
        /// </summary>
        /// <value>The value of .</value>
        public Command DeleteHotKeyBindingCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the EditHotKeyBinding command.
        /// </summary>
        /// <value>The value of .</value>
        public Command EditHotKeyBindingCommand
        {
            get;
            private set;
        }

        
        /// <summary>
        /// The NotifyingProperty for the IsHotKeyBindingsCollectionEmpty property.
        /// </summary>
        private readonly NotifyingProperty IsHotKeyBindingsCollectionEmptyProperty =
          new NotifyingProperty("IsHotKeyBindingsCollectionEmpty", typeof(bool), default(bool));

        /// <summary>
        /// Gets or sets IsHotKeyBindingsCollectionEmpty.
        /// </summary>
        /// <value>The value of IsHotKeyBindingsCollectionEmpty.</value>
        public bool IsHotKeyBindingsCollectionEmpty
        {
            get { return (bool)GetValue(IsHotKeyBindingsCollectionEmptyProperty); }
            set { SetValue(IsHotKeyBindingsCollectionEmptyProperty, value); }
        }
        

        /// <summary>
        /// Performs theShowSuggestions command.
        /// </summary>
        /// <param name="parameter">The ShowSuggestions command parameter.</param>
        private void DoShowSuggestionsCommand(object parameter)
        {
        }

        /// <summary>
        /// Gets the ShowSuggestions command.
        /// </summary>
        /// <value>The value of .</value>
        public Command ShowSuggestionsCommand
        {
            get;
            private set;
        }
    }
}
