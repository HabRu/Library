using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Library.Services.EmailServices
{
    //Сервис для отправки уведомлений
    public class EmailService
    {
        public async Task SendEmailAsync(string email,string subject,string message)
        {
            try
            {
                MailAddress from = new MailAddress("aa.d.m.i.n@yandex.ru", "Администрация");
                MailAddress to = new MailAddress(email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = subject;
                m.Body =message;
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
                smtp.Credentials = new NetworkCredential("aa.d.m.i.n@yandex.ru", "Admin_Admin_1");
                smtp.EnableSsl = true;
                Console.WriteLine(email);
                await smtp.SendMailAsync(m);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
