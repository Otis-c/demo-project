using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Procurement.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Services
{
    public interface INotificationService
    {
        void SendEmail(string emailAddress, string subject, string mailMessage,
            string messageType, string payload = "");
    }

    public class NotificationService : INotificationService
    {
        private readonly IHostingEnvironment _env;
        private readonly ApiSettings _apiSettings;

        public NotificationService(IOptions<ApiSettings> apiSettings, IHostingEnvironment env)
        {
            _env = env;
            _apiSettings = apiSettings.Value;
        }
        public void SendEmail(string emailAddress, string subject, string mailMessage,
                              string messageType, string payload = "")
        {
            MailMessage message = new MailMessage(_apiSettings.SenderAddress, emailAddress);
            message.Subject = subject;
            if (messageType == "action")
            {
                message.Body = GenerateActionTemplate(subject, mailMessage, payload);
            }
            else if (messageType == "alert")
            {
                message.Body = GenerateAlertTemplate(subject, mailMessage);
            }

            message.IsBodyHtml = true;
            new SmtpClient()
            {
                Host = _apiSettings.SmtpServer,
                EnableSsl = false,
                UseDefaultCredentials = true,
                Port = _apiSettings.SmtpPort
            }.Send(message);
        }

        private string GenerateActionTemplate(string subject, string message, string payload)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (StreamReader streamReader = File.OpenText($"{_env.WebRootPath}/email_templates/action.html"))
                stringBuilder.Append(streamReader.ReadToEnd());
            stringBuilder.Replace("#Subject", subject);
            stringBuilder.Replace("#Message", message);
            stringBuilder.Replace("#actionUrl", payload);
            return stringBuilder.ToString();
        }

        private string GenerateAlertTemplate(string subject, string message)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (StreamReader streamReader = File.OpenText($"{_env.WebRootPath}/email_templates/alert.html"))
                stringBuilder.Append(streamReader.ReadToEnd());
            stringBuilder.Replace("#Subject", subject);
            stringBuilder.Replace("#Message", message);
            return stringBuilder.ToString();
        }
    }
}
