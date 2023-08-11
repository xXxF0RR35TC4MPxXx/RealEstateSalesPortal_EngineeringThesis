using MimeKit;
using MimeKit.Text;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Common.Helpers
{
    public static class EmailHelper
    {
        public static MimeMessage CreateEmail(string to, string subject, string body)
        {
            MimeMessage mailMessage = new();
            mailMessage.To.Add(MailboxAddress.Parse(to));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart(TextFormat.Plain) { Text = body };

            return mailMessage;
        }

        public static void SendPredefinedEmail(MimeMessage mailMessage,
                                        string login,
                                        string password,
                                        string sender,
                                        string host,
                                        int port)
        {
            mailMessage.From.Add(new MailboxAddress(sender, login));

            using (SmtpClient smtp = new())
            {
                try
                {
                    smtp.Connect(host, port, true);
                    smtp.Authenticate(login, password);
                    smtp.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                }
            }
        }
    }
}
