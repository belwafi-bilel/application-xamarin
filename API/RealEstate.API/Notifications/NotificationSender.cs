using FirebaseNet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
namespace RealEstate.API.Notifications
{
    public class NotificationSender
    {
        private static NotificationSender notificationSender = new NotificationSender();
        FCMClient fcmClient;
        private const string Topic = "Products";
        private NotificationSender()
        {
            fcmClient = new FCMClient(WebConfigurationManager.AppSettings["FirebaseKey"]);
        }
        public static NotificationSender Instance
        {
            get
            {
                if (notificationSender == null)
                    notificationSender = new NotificationSender();
                return notificationSender;
            }
        }

        public async Task Send(string title, string body)
        {
            var messageToAndroid = AndroidMesasgeBuilder(title, body);
            var messageToIOS = IOSMessageBuilder(title, body);
            await fcmClient.SendMessageAsync(messageToAndroid);
            await fcmClient.SendMessageAsync(messageToIOS);
        }
        private Message AndroidMesasgeBuilder(string title, string body)
        {
            var message = new Message()
            {
                To = $"/topics/{Topic}",
                Data = new Dictionary<string, string>
                {
                    {"title", title },
                    {"body", body }
                }
            };
            return message;
        }
        private Message IOSMessageBuilder(string title, string body)
        {
            var message = new Message()
            {
                To = $"/topics/{Topic}",
                Notification = new IOSNotification
                {
                    Body = body,
                    Title = title,
                }
            };
            return message;
        }
    }
}