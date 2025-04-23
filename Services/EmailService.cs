using AuthAPI.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
namespace AuthApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _configuration;

        public EmailService(IOptions<MailSettings> _mailSettings, IConfiguration _configuration)
        {
            this._mailSettings = _mailSettings.Value;
            this._configuration = _configuration;
        }

        /// <inheritdoc/>
        public async Task SendVerificationEmail(string recipient, string token)
        {
            var email = new MailMessage();
            email.From = new MailAddress(_mailSettings.Sender, _mailSettings.DisplayName);
            email.Sender = new MailAddress(_mailSettings.Sender, _mailSettings.DisplayName);
            email.To.Add(recipient);
            email.Subject = "Email Verification";
            
            email.IsBodyHtml = true;
            var verificationLink = $"{_configuration["AppSettings:BaseUrl"]}/verify-email?email={WebUtility.UrlEncode(recipient)}&token={WebUtility.UrlEncode(token)}";
            email.Body = $"Please verify your email by clicking the link: <a href='{verificationLink}'>Verify Email</a>"; 
            
            using var smtp = new SmtpClient(_mailSettings.Host)
            {
                Port = _mailSettings.Port,
                Credentials = new NetworkCredential(_mailSettings.User, _mailSettings.Password),
                EnableSsl = true,
            };
            
            await smtp.SendMailAsync(email);
        }
    }
}