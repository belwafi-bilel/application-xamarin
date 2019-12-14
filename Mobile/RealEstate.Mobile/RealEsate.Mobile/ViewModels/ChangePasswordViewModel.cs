using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using RealEstate.Mobile.Services;
using RealEstate.Mobile.Utils;
using RealEstate.Mobile.Views;
using RealEstate.Models.API.RealEstate;
using Xamarin.Forms;

namespace RealEstate.Mobile.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private ProductService ProductService => new ProductService();

        public ICommand SubmitCommand { get; private set; }

        public ChangePasswordViewModel()
        {
            SubmitCommand = new Command(SubmitButtonClicked);
        }

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set { SetProperty(ref newPassword, value); }
        }

        private string currentPassword;
        public string CurrentPassword
        {
            get { return currentPassword; }
            set { SetProperty(ref currentPassword, value); }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { SetProperty(ref confirmPassword, value); }
        }

        private bool isCurrentPasswordInValid;
        public bool IsCurrentPasswordInValid
        {
            get { return isCurrentPasswordInValid; }
            set { SetProperty(ref isCurrentPasswordInValid, value); }
        }

        private bool isNewPasswordInValid;
        public bool IsNewPasswordInValid
        {
            get { return isNewPasswordInValid; }
            set { SetProperty(ref isNewPasswordInValid, value); }
        }

        private bool isConfirmPasswordInValid;
        public bool IsConfirmPasswordInValid
        {
            get { return isConfirmPasswordInValid; }
            set { SetProperty(ref isConfirmPasswordInValid, value); }
        }

        private async void SubmitButtonClicked(object obj)
        {
            IsCurrentPasswordInValid = string.IsNullOrEmpty(CurrentPassword);
            IsNewPasswordInValid = string.IsNullOrEmpty(NewPassword);
            IsConfirmPasswordInValid = string.IsNullOrEmpty(ConfirmPassword) || NewPassword != ConfirmPassword;

            if (IsCurrentPasswordInValid || IsNewPasswordInValid || IsConfirmPasswordInValid)
                return;

            /* Remove commented code for production
             * For now, for demo purpose we diable this function          
            */
            //bool result = await ProductService.ChangePassword(new RealEstateAccountChangePasswordDto
            //{
            //    AccountId = App.AccountInfo.Id,
            //    CurrentPassword = CurrentPassword,
            //    NewPassword = NewPassword,
            //    ConfirmNewPassword = ConfirmPassword
            //});
            //if(result)
            //{
            //    JwtTokenHandler.Instance.RemoveToken();
            //    Application.Current.MainPage = new LoginPage();
            //}
        }
    }
}