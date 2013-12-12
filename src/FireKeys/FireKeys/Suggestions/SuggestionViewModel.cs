using System;
using Apex.MVVM;
using FireKeysAPI;

namespace FireKeys.Suggestions
{
    [ViewModel]
    public class SuggestionViewModel : ViewModel<BindingSuggestion>
    {
        /// <summary>
        /// The NotifyingProperty for the HotKeyBinding property.
        /// </summary>
        private readonly NotifyingProperty HotKeyBindingProperty =
          new NotifyingProperty("HotKeyBinding", typeof(HotKeyBinding), default(HotKeyBinding));

        /// <summary>
        /// Gets or sets HotKeyBinding.
        /// </summary>
        /// <value>The value of HotKeyBinding.</value>
        public HotKeyBinding HotKeyBinding
        {
            get { return (HotKeyBinding)GetValue(HotKeyBindingProperty); }
            set { SetValue(HotKeyBindingProperty, value); }
        }

        /// <summary>
        /// The NotifyingProperty for the ApplySuggestion property.
        /// </summary>
        private readonly NotifyingProperty ApplySuggestionProperty =
          new NotifyingProperty("ApplySuggestion", typeof(bool), default(bool));

        /// <summary>
        /// Gets or sets ApplySuggestion.
        /// </summary>
        /// <value>The value of ApplySuggestion.</value>
        public bool ApplySuggestion
        {
            get { return (bool)GetValue(ApplySuggestionProperty); }
            set { SetValue(ApplySuggestionProperty, value); }
        }



        public override void FromModel(BindingSuggestion model)
        {
            Model = model;
        }

        public override void ToModel(BindingSuggestion model)
        {
            throw new NotImplementedException();
        }

        public BindingSuggestion Model { get; private set; }
    }
}
