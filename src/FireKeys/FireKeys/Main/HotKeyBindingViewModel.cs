using Apex.MVVM;
using FireKeysAPI;

namespace FireKeys.Main
{
    [ViewModel]
    public class HotKeyBindingViewModel : ViewModel<HotKeyBinding>
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

        public override void FromModel(HotKeyBinding model)
        {
            Model = model;
            HotKeyBinding = model;
            ErrorSettingBinding = !model.HotKeyRegisteredSuccessfully;
        }

        public override void ToModel(HotKeyBinding model)
        {
            throw new System.NotImplementedException();
        }

        public HotKeyBinding Model { get; private set; }

        
        /// <summary>
        /// The NotifyingProperty for the ErrorSettingBinding property.
        /// </summary>
        private readonly NotifyingProperty ErrorSettingBindingProperty =
          new NotifyingProperty("ErrorSettingBinding", typeof(bool), default(bool));

        /// <summary>
        /// Gets or sets ErrorSettingBinding.
        /// </summary>
        /// <value>The value of ErrorSettingBinding.</value>
        public bool ErrorSettingBinding
        {
            get { return (bool)GetValue(ErrorSettingBindingProperty); }
            set { SetValue(ErrorSettingBindingProperty, value); }
        }
    }
}
