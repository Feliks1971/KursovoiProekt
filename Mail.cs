using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovoiProekt
{
    internal  record class Mail
    {

        public static void Milo(string Pochta, string Topic, string Letter)
        {
            // отправка логина и пароля на почту пользователя  
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("felix", "skrepkalida@mail.ru"));
            message.To.Add(new MailboxAddress("polzovatel", Pochta));
            message.Subject = Topic;
            message.Body = new TextPart("plain")
            {
                Text = Letter
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.mail.ru", 587, false);
                // если аутентификация не работает - проверить настройку почты
                client.Authenticate("skrepkalida", "g8emxigjtenZcP8mibCb");
                client.Send(message);
                client.Disconnect(true);
            }

        }
    }
}
