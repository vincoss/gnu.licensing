using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace samplesl.Sample_XamarinForms.ViewModels
{
    public class LicenseViewModel : BaseViewModel
    {
        private Guid? _key;

        public LicenseViewModel()
        {
            ActivateCommand = new Command(OnActivateCommand, OnCanActivateCommand);

            PropertyChanged += LicenseViewModel_PropertyChanged;
        }

        public override void Initialize()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                _key = null;
                RegisterKey = null;
                ErrorMessage = null;
                LicenseType = "Demo";
                LicenseKey = null;
                ShowError = false;

                // TODO: try to read license file D84BAF7E-F371-4C75-9C10-82EE14FEBEC3
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        #region Events

        private void LicenseViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RegisterKey))
            {
                _key = null;
                ErrorMessage = null;

                if (string.IsNullOrEmpty(RegisterKey) == false)
                {
                    Guid id;
                    Guid.TryParse(RegisterKey, out id);

                    if (id == Guid.Empty)
                    {
                        ErrorMessage = "Invalid license key.";
                    }
                    _key = id;
                }
            }

            ShowError = ErrorMessage != null;

            ((Command)ActivateCommand).ChangeCanExecute();
        } 

        #endregion

        #region Command methods

        private void OnActivateCommand()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                // TODO: call register _service.Register(_key);

                if (true)
                {
                    LicenseKey = _key;
                    LicenseType = "Full";
                }
            }
            finally
            {
                IsBusy = false;
                Initialize();
            }
        }

        private bool OnCanActivateCommand()
        {
            return _key != null && _key != Guid.Empty;
        }

        #endregion

        #region Commands

        public ICommand ActivateCommand { get; private set; }

        #endregion

        #region Properties

        private string _registerKey;

        public string RegisterKey
        {
            get { return _registerKey; }
            set { SetProperty(ref _registerKey, value); }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        private Guid? _licenseKey;
        public Guid? LicenseKey
        {
            get { return _licenseKey; }
            set { SetProperty(ref _licenseKey, value); }
        }

        private string _licenseType;

        public string LicenseType
        {
            get { return _licenseType; }
            set { SetProperty(ref _licenseType, value); }
        }

        private bool _showError;

        public bool ShowError
        {
            get { return _showError; }
            set { SetProperty(ref _showError, value); }
        }
        
        #endregion
    }
}