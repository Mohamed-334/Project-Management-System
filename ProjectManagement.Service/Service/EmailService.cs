using ProjectManagement.Domain.Shared.EmailModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Localization;
using MimeKit;

namespace ProjectManagement.Service.Service
{
    public class EmailService : IEmailService
    {
        #region Fields
        private readonly EmailSettings _emailSettings;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion
        #region Constructors
        public EmailService(EmailSettings emailSettings, IStringLocalizer<AppLocalization> stringLocalizer)
        {
            _emailSettings = emailSettings;
            _stringLocalizer = stringLocalizer;
        }

        #endregion
        #region Handle Functions
        public async Task<string> SendEmailAsync(string email, string Message, string? Subject)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                    client.Authenticate(_emailSettings.FromEmail, _emailSettings.Password);
                    var bodybuilder = new BodyBuilder
                    {
                        HtmlBody = $"{Message}",
                    };
                    var message = new MimeMessage
                    {
                        Body = bodybuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                    message.To.Add(new MailboxAddress("testing", email));
                    message.Subject = Subject == null ? "" : Subject;
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return _stringLocalizer[AppLocalizationKeys.Success];
            }
            catch (Exception ex)
            {
                return _stringLocalizer[AppLocalizationKeys.SendEmailFailed];
            }
        }
        #endregion
    }
}
