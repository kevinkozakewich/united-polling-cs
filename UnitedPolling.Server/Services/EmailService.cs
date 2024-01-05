using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace UnitedPolling.Services
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<Configurations.SendGrid> config)
        {
            Options = config.Value;
        }

        public Configurations.SendGrid Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.apiKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.apiKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.fromEmail, "Password Recovery"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            msg.SetClickTracking(false, false);

            // Throws an exception if fails.
            var response = await client.SendEmailAsync(msg);
        }
    }
}
