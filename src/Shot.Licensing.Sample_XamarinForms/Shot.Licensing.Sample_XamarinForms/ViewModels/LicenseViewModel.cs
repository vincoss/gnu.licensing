using samplesl.Sample_XamarinForms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace samplesl.Sample_XamarinForms.ViewModels
{
    public class LicenseViewModel : BaseViewModel
    {
        private Guid _key;
        private readonly ILicenseService _licenseService;

        public LicenseViewModel(ILicenseService licenseService)
        {
            if (licenseService == null)
            {
                throw new ArgumentNullException(nameof(licenseService));
            }

            _licenseService = licenseService;

            ActivateCommand = new Command(OnActivateCommand, OnCanActivateCommand);
            PropertyChanged += LicenseViewModel_PropertyChanged;
        }

        public async override void Initialize()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                _key = Guid.Empty;
                RegisterKey = null;
                ErrorMessage = null;
                LicenseType = LicenseContants.Demo;
                LicenseKey = null;
                ShowError = false;
                AppId = Preferences.Get(LicenseContants.AppId, null);

                var result = await _licenseService.Validate();
                if(result.Successful)
                {
                    LicenseKey = result.License.Id.ToString();
                    LicenseType = LicenseContants.Full;
                    await _licenseService.SetLicenseKeyAsync(result.License.Id.ToString());
                }
                else
                {
                    ShowLicenseError(result);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        #region Private methods

        private void LicenseViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RegisterKey))
            {
                _key = Guid.Empty;
                ErrorMessage = null;

                if (string.IsNullOrEmpty(RegisterKey) == false)
                {
                    Guid id;
                    Guid.TryParse(RegisterKey, out id);

                    if (id == Guid.Empty)
                    {
                        ErrorMessage = "Invalid license key."; // TODO:
                    }
                    _key = id;
                }
            }

            ShowError = ErrorMessage != null;

            ((Command)ActivateCommand).ChangeCanExecute();
        }

        private void ShowLicenseError(LicenseResult result)
        {
            var sb = new StringBuilder();

            foreach (var f in result.Failures)
            {
                sb.AppendLine(f.Message);
                sb.AppendLine(f.HowToResolve);
            }

            if (result.Exception != null)
            {
                sb.AppendLine(result.Exception.Message); // TODO: generic failure instead
            }

            ErrorMessage = sb.Length > 0 ? sb.ToString() : null;
        }

        #endregion

        #region Command methods

        private async void OnActivateCommand()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;
                var result =  await _licenseService.RegisterAsync(_key, LicenseContants.ProductId);
                if(result.Successful)
                {
                    Initialize();
                }
                else
                {
                    ShowLicenseError(result);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool OnCanActivateCommand()
        {
            return _key != Guid.Empty;
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

        private string _appId;
        public string AppId
        {
            get { return _appId; }
            set { SetProperty(ref _appId, value); }
        }

        private string _licenseKey;
        public string LicenseKey
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