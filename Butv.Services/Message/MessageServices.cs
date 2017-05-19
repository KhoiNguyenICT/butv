﻿using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace BUTV.Services.Message
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("BUTV", email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                //client.LocalDomain = "smtp.gmail.com";
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true).ConfigureAwait(false);
                    await client.AuthenticateAsync("email", "pass");
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
                catch (System.Exception e)
                {

                }
            }
            //return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
