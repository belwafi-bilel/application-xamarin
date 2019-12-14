using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using RealEstate.Mobile.AppSettings;
using RealEstate.Mobile.Models;
using RealEstate.Mobile.Services;
using RealEstate.Mobile.Utils;
using RealEstate.Mobile.Views;
using Xamarin.Forms;

namespace RealEstate.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        AuthenticationService AuthenticationService => new AuthenticationService();
       
        public ICommand LoginCommand { get; private set; }
        public LoginViewModel()
        {
            LoginCommand = new Command(LoginButtonClicked);
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        private bool isNameEmpty;
        public bool IsNameEmpty
        {
            get { return isNameEmpty; }
            set { SetProperty(ref isNameEmpty, value); }
        }

        private bool isPasswordEmpty;
        public bool IsPasswordEmpty
        {
            get { return isPasswordEmpty; }
            set { SetProperty(ref isPasswordEmpty, value); }
        }     

        private async void LoginButtonClicked(object obj)
        {
            IsNameEmpty = string.IsNullOrEmpty(UserName);
            IsPasswordEmpty = string.IsNullOrEmpty(Password);
            if (IsNameEmpty || IsPasswordEmpty)
            {
                return;
            }
            else
            {
                IsBusy = true;
                await Task.Delay(1000);
                var userToken = AuthenticationService.Login(UserName, Password);
                if (userToken == null)
                {
                    IsBusy = false;
                    Application.Current.MainPage.DisplayAlert("Warning", "Server errror ! Please contact with service provider", "OK");
                    return;
                }
                if (userToken.AccessToken != null)
                {
                    await JwtTokenHandler.Instance.SaveToken(userToken.AccessToken);
                    await JwtTokenHandler.Instance.SetAccountInfo();
                    IsBusy = false;
                    Application.Current.MainPage = new MainPage();
                }
                else
                {
                    IsBusy = false;
                    var responseMessage = JsonConvert.DeserializeObject<MessageResponse>(userToken.Message);
                    Application.Current.MainPage.DisplayAlert("Warning", responseMessage?.Description, "OK");
                }
            }
        }
    }
}