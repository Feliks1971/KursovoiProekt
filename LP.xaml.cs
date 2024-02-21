//если вдруг забыли свои пароль и логин
using MimeKit;
using System.Windows;
using MailKit.Net.Smtp;

namespace KursovoiProekt
{
    /// <summary>
    /// Логика взаимодействия для LP.xaml
    /// </summary>
    public partial class LP : Window
    {
        public LP()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int countparol = 0;
            //подключаем базу данных с паролем и выгружаем её
            using (ApplicationContext db = new())
            {
                List<Login_parol> Vvostanovlenie = [.. db.LogPar];
                foreach (Login_parol LPV in Vvostanovlenie)
                {
                    if (LPV.Email == Email.Text)
                    {
                        countparol = 1;
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("felix", "skrepkalida@mail.ru"));
                        message.To.Add(new MailboxAddress("polzovatel", Email.Text));
                        message.Subject = "Vash login i parol";
                        message.Body = new TextPart("plain")
                        {
                            Text = $"Login - {LPV.Login}, parol - {LPV.Parol}"
                        };

                        using (var client = new SmtpClient())
                        {
                            client.Connect("smtp.mail.ru", 587, false);
                           // если аутентификация не работает - проверить настройку почты
                            client.Authenticate("skrepkalida", "g8emxigjtenZcP8mibCb");
                            client.Send(message);
                            client.Disconnect(true);
                        }
                        MessageBox.Show("На вашу почту отправлены ваши логин и пароль!");
                        this.Close();
                    }
                }
            }
            if (countparol == 0)
            {
                MessageBox.Show("Такой E-mail не найден,\n пройдите регистрацию!");
                this.Close();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
