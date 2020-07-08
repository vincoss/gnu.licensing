using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;


namespace Gnu.Licensing.Sample_XamarinForms.ViewModels
{
    public class LicenseViewModel : BaseViewModel
    {
        private Guid _key;
        private readonly ILicenseService _licenseService;
        private readonly IApplicationContext _ctx;

        public LicenseViewModel(ILicenseService licenseService, IApplicationContext ctx)
        {
            if (licenseService == null)
            {
                throw new ArgumentNullException(nameof(licenseService));
            }
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }

            _licenseService = licenseService;
            _ctx = ctx;

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
                LicenseKey = null;
                ShowError = false;
                LicenseGlobals.Set(AppLicense.Demo);
                Description = $"Product Name - {LicenseGlobals.Get()}";

                var result = await _licenseService.ValidateAsync();
                if(result.Successful)
                {
                    LicenseGlobals.Set(AppLicense.Full);
                    LicenseKey = result.License.Id.ToString();
                    LicensedTo = result.License.Customer.Name;
                    ShowActivated = true;
                }
                else
                {
                    ShowLicenseError(result);
                }
                Description = $"Product Name - {LicenseGlobals.Get()}";
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
                        ErrorMessage = "Invalid license key."; // TODO: localize or call result form service
                    }
                    _key = id;
                }
            }

            ShowError = string.IsNullOrWhiteSpace(ErrorMessage) == false;

            ((Command)ActivateCommand).ChangeCanExecute();
        }

        private void ShowLicenseError(LicenseResult result)
        {
            var sb = new StringBuilder();

            foreach (var f in result.Failures)
            {
                sb.AppendLine(f.Code);          // TODO: localize
                sb.AppendLine(f.Message);       // TODO: localize
                sb.AppendLine(f.HowToResolve);  // TODO: localize
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
                
                var result =  await _licenseService.RegisterAsync(_key);
                
                if(result.Successful)
                {
                    await _ctx.SetLicenseKeyAsync(result.License.Id.ToString());
                    LicenseGlobals.Set(AppLicense.Full);
                    IsBusy = false;
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

        private bool _showError;

        public bool ShowError
        {
            get { return _showError; }
            set { SetProperty(ref _showError, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _licensedTo;

        public string LicensedTo
        {
            get { return _licensedTo; }
            set { SetProperty(ref _licensedTo, value); }
        }

        private string _licenseKey;

        public string LicenseKey
        {
            get { return _licenseKey; }
            set { SetProperty(ref _licenseKey, value); }
        }

        private bool _showActivated;

        public bool ShowActivated
        {
            get { return _showActivated; }
            set { SetProperty(ref _showActivated, value); }
        }
        
        #endregion
    }
}