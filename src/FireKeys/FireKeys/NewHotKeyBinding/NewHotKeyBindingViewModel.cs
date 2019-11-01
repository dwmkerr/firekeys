using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using Apex.MVVM;
using FireKeys.Controls;
using FireKeysAPI;
using FireKeysAPI.Actions;

namespace FireKeys.NewHotKeyBinding
{
    [ViewModel]
    public class NewHotKeyBindingViewModel : ViewModel, INewActionUserInterface
    {
        private readonly ObservableCollection<HotKeyModifierSelection> _modifierSelections = new ObservableCollection<HotKeyModifierSelection>();
        public ObservableCollection<HotKeyModifierSelection> ModifierSelections { get { return _modifierSelections; } }

        public HotKeyModifiers ModifiersEnum
        {
            get
            {
                return ModifierSelections.Where(ms => ms.Selected).Aggregate(HotKeyModifiers.None, (e, ms) => e |= ms.HotKeyModifier);
            }
        }

        public NewHotKeyBindingViewModel() : this(new HotKey())
        {
            
        }
        public NewHotKeyBindingViewModel(HotKey hotkey)
        {
            HotKey = hotkey;
            OKCommand = new Command(DoOKCommand, false);
            CancelCommand = new Command(DoCancelCommand);

            CreateActions();

            //  Select the create program action.
            SelectedAction = Actions.Single(a => a is ExecuteProgramAction);

            Enum.GetValues(typeof (HotKeyModifiers)).OfType<HotKeyModifiers>().Where(e => e != HotKeyModifiers.None)
                .Select(e => new HotKeyModifierSelection
                             {
                                 Selected = HotKey.Modifiers.HasFlag(e),
                                 HotKeyModifier = e
                             })
                .ToList().ForEach(ms => _modifierSelections.Add(ms));

            ModifierSelections.ToList().ForEach(ms=>ms.PropertyChanged += (s, ea) =>
                                                                          {
                                                                              HotKey.Modifiers = ModifiersEnum;
                                                                          });
        }

        private void CreateActions()
        {
            //  Create an instance of each action type.
            foreach(var actionType in FireKeysApplication.Instance.GetActionTypes())
                Actions.Add((IHotKeyAction)Activator.CreateInstance(actionType));
        }

        private void UpdateOKCommandStatus()
        {
            OKCommand.CanExecute = !string.IsNullOrEmpty(DisplayName);
        }

        /// <summary>
        /// The NotifyingProperty for the HotKey property.
        /// </summary>
        private readonly NotifyingProperty HotKeyProperty =
          new NotifyingProperty("HotKey", typeof(HotKey), default(HotKey));

        /// <summary>
        /// Gets or sets HotKey.
        /// </summary>
        /// <value>The value of HotKey.</value>
        public HotKey HotKey
        {
            get { return (HotKey)GetValue(HotKeyProperty); }
            set
            {
                SetValue(HotKeyProperty, value);
                ModifierSelections.ToList().ForEach(ms => ms.Selected = value.Modifiers.HasFlag(ms.HotKeyModifier));
            }
        }

        
        /// <summary>
        /// The NotifyingProperty for the DisplayName property.
        /// </summary>
        private readonly NotifyingProperty DisplayNameProperty =
          new NotifyingProperty("DisplayName", typeof(string), default(string));

        /// <summary>
        /// Gets or sets DisplayName.
        /// </summary>
        /// <value>The value of DisplayName.</value>
        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set
            {
                SetValue(DisplayNameProperty, value);
                UpdateOKCommandStatus();
            }
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

        /// <summary>
        /// The Actions observable collection.
        /// </summary>
        private readonly ObservableCollection<IHotKeyAction> ActionsProperty =
          new ObservableCollection<IHotKeyAction>();

        /// <summary>
        /// Gets the Actions observable collection.
        /// </summary>
        /// <value>The Actions observable collection.</value>
        public ObservableCollection<IHotKeyAction> Actions
        {
            get { return ActionsProperty; }
        }

        
        /// <summary>
        /// The NotifyingProperty for the SelectedAction property.
        /// </summary>
        private readonly NotifyingProperty SelectedActionProperty =
          new NotifyingProperty("SelectedAction", typeof(IHotKeyAction), default(IHotKeyAction));

        /// <summary>
        /// Gets or sets SelectedAction.
        /// </summary>
        /// <value>The value of SelectedAction.</value>
        public IHotKeyAction SelectedAction
        {
            get { return (IHotKeyAction)GetValue(SelectedActionProperty); }
            set
            {
                SetValue(SelectedActionProperty, value);
                if (actionUserInterfaces.ContainsKey(value) == false)
                    actionUserInterfaces[value] = value.CreateNewActionUserInterface(this);
                SelectedActionUserInterface = actionUserInterfaces[value];
            }
        }

        private readonly Dictionary<IHotKeyAction, FrameworkElement> actionUserInterfaces = new Dictionary<IHotKeyAction, FrameworkElement>();
        
        /// <summary>
        /// The NotifyingProperty for the SelectedActionUserInterface property.
        /// </summary>
        private readonly NotifyingProperty SelectedActionUserInterfaceProperty =
          new NotifyingProperty("SelectedActionUserInterface", typeof(FrameworkElement), default(FrameworkElement));

        /// <summary>
        /// Gets or sets SelectedActionUserInterface.
        /// </summary>
        /// <value>The value of SelectedActionUserInterface.</value>
        public FrameworkElement SelectedActionUserInterface
        {
            get { return (FrameworkElement)GetValue(SelectedActionUserInterfaceProperty); }
            set { SetValue(SelectedActionUserInterfaceProperty, value); }
        }

        public void SetSuggestedDisplayName(string suggestedDisplayName)
        {
            DisplayName = suggestedDisplayName;
        }

        public class HotKeyModifierSelection : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }

            public HotKeyModifiers HotKeyModifier { get; set; }

            private bool _selected;

            public bool Selected
            {
                get { return _selected; }
                set
                {
                    _selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }
    }
}
