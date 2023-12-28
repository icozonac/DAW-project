
using System.Diagnostics;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace TW.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSettings _mailJetSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return ExecuteAsync(email, subject, htmlMessage);
        }

        public async Task ExecuteAsync(string email, string subject, string body)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey);
            MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.FromEmail, "ioncozonac@proton.me")
                .Property(Send.FromName, "Mailjet Pilot")
                .Property(Send.Subject, subject)
                .Property(Send.HtmlPart, body)
                .Property(Send.Recipients, new JArray {
                    new JObject {
                        {"Email", email}
                    }
                });
            MailjetResponse response = await client.PostAsync(request);
            Debug.WriteLine(response.IsSuccessStatusCode);
        }
    }
}
