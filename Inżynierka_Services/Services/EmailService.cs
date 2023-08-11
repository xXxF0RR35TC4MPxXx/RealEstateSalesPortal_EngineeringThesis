using Inżynierka.Shared.DTOs.Email;
using Inżynierka_Common.Helpers;
using Inżynierka_Common.ServiceRegistrationAttributes;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class EmailService
    {

        public EmailService()
        {}


        public void SendConfirmationEmail(string receiver, string url)
        {
            string subject = MessageHelper.RegistrationSubject;
            string body = MessageHelper.RegistrationBody(url);
            MimeMessage mail = EmailHelper.CreateEmail(receiver, subject, body);

            SmtpAccountDTO? dto = new()
            {
                Host = "smtp.gmail.com",
                Login = "your@email",
                Password = "16-character long remote access password",
                Port = 465,
                Sender = "The name of the sender",
            };

            if (dto == null)
                return;

            EmailHelper.SendPredefinedEmail(mail, dto.Login, dto.Password, dto.Sender, dto.Host, dto.Port);
        }

        public bool SendPasswordRecoveryEmail(string email, string url)
        {
            string subject = MessageHelper.PasswordRecoverySubject;
            string body = MessageHelper.PasswordRecoveryBody(url);

            MimeMessage mail = EmailHelper.CreateEmail(email, subject, body);

            SmtpAccountDTO? dto = new()
            {
                Host = "smtp.gmail.com",
                Login = "your@email",
                Password = "16-character long remote access password",
                Port = 465,
                Sender = "The name of the sender",
            };

            if (dto == null)
                return false;

            EmailHelper.SendPredefinedEmail(mail, dto.Login, dto.Password, dto.Sender, dto.Host, dto.Port);

            return true;
        }

    }
}
