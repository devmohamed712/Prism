using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Helpers
{
    public class Mail
    {
        public readonly IConfiguration _configuration;

        public Mail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendEmail(dynamic model, List<string> sendToEmailAddresses)
        {
            var message = new MimeMessage()
            {
                From = { new MailboxAddress(_configuration["SMTP:SenderDisplayName"], model.Email) },
                Subject = model.Subject,
                Body = new TextPart("html") { Text = model.Message }
            };
            InternetAddressList list = new InternetAddressList();
            sendToEmailAddresses.ForEach(email =>
            {
                list.Add(new MailboxAddress("", email.Trim()));
            });
            message.To.AddRange(list);
            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["SMTP:Host"], Convert.ToInt32(_configuration["SMTP:Port"]), true);
                client.Authenticate(_configuration["SMTP:SenderMail"], _configuration["SMTP:GoogleAppPW"]);
                try
                {
                    client.Send(message);
                    client.Disconnect(true);
                    return true;
                }
                catch
                {
                    client.Disconnect(true);
                    return false;
                }
            }
        }
    }
}
