using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.Services;
using Xamarin.Forms;

namespace UPMC_App.ViewModels
{
    public class AuthViewModel:BaseViewModel
    {
        #region Events
        public event EventHandler<string> PinAuthError;
        public event EventHandler PinAuthSuccess;

        private void OnPinAuthError(string msg)
        {
            PinAuthError?.Invoke(this, msg);
        }

        private void OnPinAuthSuccess()
        {
            PinAuthSuccess?.Invoke(this, null);
        }
        #endregion

        #region Properties
        private AuthService _authService = new AuthService();

        private string _pin;
        private int _pinLenght = 0;
        private string _pinResultText="";
        private Color _pinResultColor = Color.Green;


        private string _username;
        private string _password;
        
        public string Pin
        {
            get => _pin;
            set
            {
                SetProperty(ref _pin, value);
                PinLenght = _pin.Length;
                if (PinLenght == 4)
                {
                    CheckPin();
                }
            }
        }

        public int PinLenght { get => _pinLenght; set =>SetProperty(ref _pinLenght, value); }
        public string PinResultText { get => _pinResultText; set =>SetProperty(ref _pinResultText, value); }
        public Color PinResultColor { get => _pinResultColor; set => SetProperty(ref _pinResultColor, value); }
        public string Username { get => _username; set { SetProperty(ref _username, value); OnPropertyChanged(nameof(IsLoginButtonEnabled)); } }
        public string Password { get => _password; set { SetProperty(ref _password, value); OnPropertyChanged(nameof(IsLoginButtonEnabled)); } }
        public static string StoredUsername
        {
            get => CrossSettings.Current.GetValueOrDefault("storedUsername", String.Empty);
            set => CrossSettings.Current.AddOrUpdateValue("storedUsername", value);
        }
        public static string StoredPass
        {
            get => CrossSettings.Current.GetValueOrDefault("storedPass", String.Empty);
            set => CrossSettings.Current.AddOrUpdateValue("storedPass", value);
        }

        public bool IsLoginButtonEnabled => (!string.IsNullOrEmpty(Username)) && (!string.IsNullOrEmpty(Password));
        #endregion

        private async void CheckPin()
        {
            IsProgressActive = true;
            PinResultText = "";
            var res = await _authService.AuthorizeWithPin("ThornMarc", Pin);

            if ((res != null) && (res.Code==100))
            {
                //save pass
                PinResultColor = Color.Green;
                PinResultText = "PIN is correct";
                Pin = String.Empty;
                ViewModelContainer.HomeVM.FillDataFromAuthResult(res);
                OnPinAuthSuccess();
            }
            else
            {
                PinResultColor = Color.Red;
                PinResultText = "PIN is incorrect";
                string errorString = "";
                if (res == null)
                {
                    errorString = "Server error during authorization. Please, try later.";
                }
                else
                {
                    if ((res.ErrorMessages != null) && (res.ErrorMessages.Count > 0))
                        errorString = res.ErrorMessages[0];
                    else
                        errorString = "Authorization failed";
                }

                await Task.Delay(100);
                Pin = String.Empty;
                OnPinAuthError(errorString);
            }
            IsProgressActive = false;
        }

        public async Task<string> CheckPassword(bool isUseStored = false)
        {
            IsProgressActive = true;
            ServerDataModels.Api_AuthResponse res;
            if (isUseStored)
            {
                res = await _authService.AuthorizeWithPassword(StoredUsername, StoredPass);
            }
            else
            {
                res = await _authService.AuthorizeWithPassword(Username, Password);
            }

            string ret = string.Empty;
            if ((res != null) && !String.IsNullOrEmpty(res.UserData.sessionKey))
            {
                //save pass if it is NO auth with stored data
                if (!isUseStored)
                {
                    StoredUsername = Username;
                    StoredPass = Password;
                }
                ViewModelContainer.HomeVM.FillDataFromAuthResult(res);
            }
            else
            {
                if (res != null)
                {
                    Password = String.Empty;
                    StoredPass = String.Empty;
                    ret =  res.UserData.errorMessage;
                }
                else
                {
                    ret = "Server error during authorization. Please, try later.";
                }
            }
            IsProgressActive = false;

            return ret;
        }
    }
}
