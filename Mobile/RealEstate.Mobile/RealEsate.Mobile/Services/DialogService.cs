using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace RealEstate.Mobile.Services
{
    public class DialogService
    {
        public DialogService()
        {
        }
        public async Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            await UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }
        public Task<bool> ShowConfirmedAsync(string message, string title, string okText, string cancelText)
        {
            return UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
        }
        public void ShowToast(string message, int duration = 5000)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(duration);

            toastConfig.SetMessageTextColor(System.Drawing.Color.White);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(33, 44, 55));
            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
