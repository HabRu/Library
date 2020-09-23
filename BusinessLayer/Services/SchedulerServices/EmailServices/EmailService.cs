using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Library.Services.EmailServices
{
    //Сервис для отправки уведомлений
    public class EmailService
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var from = new MailAddress("aa.d.m.i.n@yandex.ru", "Администрация");
                var to = new MailAddress(email);
                var m = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                var smtp = new SmtpClient("smtp.yandex.ru", 25)
                {
                    Credentials = new NetworkCredential("aa.d.m.i.n@yandex.ru", "Admin_Admin_1"),
                    EnableSsl = true
                };
                logger.LogDebug(email);
                await smtp.SendMailAsync(m);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
