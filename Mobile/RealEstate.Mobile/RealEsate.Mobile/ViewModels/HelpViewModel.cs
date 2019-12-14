using Plugin.Messaging;
using RealEstate.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RealEstate.Mobile.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        public Command<string> CallCommand { get; set; }

        public HelpViewModel()
        {
            CallCommand = new Command<string>(Call);
        }
        private void Call(string mobileNumber)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(mobileNumber);
        }
        public string VersionNumber
        {
            get
            {
                return DependencyService.Get<IAppVersion>().GetVersionNumber();
            }
        }
    }
}
