//проходим авторизацию или регистрацию

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using MimeKit;
using MailKit.Net.Smtp;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Linq;




namespace KursovoiProekt
{
    
    public partial class Registracia : Window
    {
        public bool flag1 = false;
        public bool flag2 = false;
        public bool flag3 = false;
        public bool flag4 = false;
        public Registracia()
        {
            InitializeComponent();
           
        }

        private void A_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Image? IM = FORPL;
            PasswordBox? elementWithFocus = FocusManager.GetFocusedElement(this) as PasswordBox;
            if (elementWithFocus == PP) IM = FORPP;
            else if (elementWithFocus == TBparol2) IM = FORPP2;
            else if (elementWithFocus == TBemail2) IM = FOREMAIL;
            IM.Source = new BitmapImage(new Uri("../Resources/otkrit.png", UriKind.Relative));
            IM.Visibility = Visibility.Visible;
            if (elementWithFocus?.Password.Length > 0) elementWithFocus.Opacity = 1;
            else elementWithFocus.Opacity = 0.5;
        }

        private void A1_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Image? IM = FORPL;
            TextBox? TBox = TBLogin;
            PasswordBox? elementWithFocus = FocusManager.GetFocusedElement(this) as PasswordBox;
            if (elementWithFocus == PP) { IM = FORPP; TBox = TBParol; }
            else if (elementWithFocus == TBparol2) { IM = FORPP2; TBox = TBPP; }
            else if (elementWithFocus == TBemail2) { IM = FOREMAIL; TBox = TBE;  }
            IM.Source = new BitmapImage(new Uri("../Resources/otkrit.png", UriKind.Relative));
            IM.Visibility = Visibility.Hidden;
            TBox.Text = elementWithFocus?.Password;
        }


        private void TB1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox? elementWithFocus = FocusManager.GetFocusedElement(this) as PasswordBox;
            if (elementWithFocus != null && elementWithFocus.Password.Length > 0)
                elementWithFocus.Opacity = 1;
            else if (elementWithFocus != null && elementWithFocus.Password.Length == 0)
                elementWithFocus.Opacity = 0.5;
        }

        private void Txt_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Image? IM = FORPL;
            PasswordBox? PBox = PL;
            TextBox? elementWithFocus = FocusManager.GetFocusedElement(this) as TextBox;
            if (elementWithFocus == TBParol) { IM = FORPP; PBox = PP; }
            else if (elementWithFocus == TBPP) { IM = FORPP2; PBox = TBparol2; }
            else if (elementWithFocus == TBE) { IM = FOREMAIL; PBox = TBemail2; }
            if (elementWithFocus?.Text.Length > 0) elementWithFocus.Opacity = 1;
            else elementWithFocus.Opacity = 0.5;
            IM.Source = new BitmapImage(new Uri("../Resources/zakrit.png", UriKind.Relative));
            IM.Visibility = Visibility.Visible;
            elementWithFocus.Text = PBox.Password;
        }

        private void TxtLost_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Image? IM = FORPL;
            PasswordBox? PBox = PL;
            TextBox? elementWithFocus = FocusManager.GetFocusedElement(this) as TextBox;
            if (elementWithFocus == TBParol) { IM = FORPP; PBox = PP; }
            else if (elementWithFocus == TBPP) { IM = FORPP2; PBox = TBparol2; }
            else if (elementWithFocus == TBE) { IM = FOREMAIL; PBox = TBemail2; }
            IM.Source = new BitmapImage(new Uri("../Resources/zakrit.png", UriKind.Relative));
            IM.Visibility = Visibility.Hidden;
            PBox.Password = elementWithFocus?.Text;
        }

        private void TxtChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? elementWithFocus = FocusManager.GetFocusedElement(this) as TextBox;
           if (elementWithFocus != null && elementWithFocus.Text.Length > 0)
                elementWithFocus.Opacity = 1;
            else if (elementWithFocus != null && elementWithFocus.Text.Length == 0)
                elementWithFocus.Opacity = 0.5;
        }

        //обработка клика на "глаз"
        private void ImgShowHide_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox? elementT = FocusManager.GetFocusedElement(this) as TextBox;
            PasswordBox? elementP = FocusManager.GetFocusedElement(this) as PasswordBox;

            if (elementT == TBLogin || elementP == PL)
            {
                if (flag1 == false)
                {
                    ShowPassword();
                    flag1 = true;
                }
                else
                {
                    HidePassword();
                    flag1 = false;
                }
            }
            else if (elementT == TBParol || elementP == PP)
            {
                if (flag2 == false)
                {
                    ShowPassword();
                    flag2 = true;
                }
                else
                {
                    HidePassword();
                    flag2 = false;
                }
            }
            else if (elementT == TBPP || elementP == TBparol2)
            {
                if (flag3 == false)
                {
                    ShowPassword();
                    flag3 = true;
                }
                else
                {
                    HidePassword();
                    flag3 = false;
                }
            }
            else if (elementT == TBE || elementP == TBemail2)
            {
                if (flag4 == false)
                {
                    ShowPassword();
                    flag4 = true;
                }
                else
                {
                    HidePassword();
                    flag4 = false;
                }
            }
        }

        void ShowPassword()
        {
            TextBox? TBox = TBLogin;
            PasswordBox? PBox = PL;
            PBox = FocusManager.GetFocusedElement(this) as PasswordBox;
            if (PBox == PP) TBox = TBParol; 
            else if (PBox == TBparol2) TBox = TBPP; 
            else if (PBox == TBemail2) TBox = TBE; 

            PBox.Visibility = Visibility.Hidden;
            TBox.Visibility = Visibility.Visible;
            Keyboard.Focus(TBox);
            TBox.Select(TBox.Text.Length, 0);
        }
        //скрыть пароль
        void HidePassword()
        {
            TextBox? TBox = TBLogin;
            PasswordBox? PBox = PL;
            TBox = FocusManager.GetFocusedElement(this) as TextBox;
            if (TBox == TBParol) PBox = PP;
            else if (TBox ==TBPP ) PBox = TBparol2;
            else if (TBox == TBE) PBox = TBemail2;

            TBox.Visibility = Visibility.Hidden;
            PBox.Visibility = Visibility.Visible;
            Keyboard.Focus(PBox);
        }
        // отмена
        public void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        // регистрация
        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            TBparol.Visibility = Visibility.Visible;
            TBparol1.Visibility = Visibility.Visible;
            TBPP.Visibility = Visibility.Hidden;
            TBE.Visibility = Visibility.Hidden;
            TBparol2.Visibility = Visibility.Visible;
            TBemail.Visibility = Visibility.Visible;
            TBemail1.Visibility = Visibility.Visible;
            TBemail2.Visibility = Visibility.Visible;
            Avtor.Visibility = Visibility.Collapsed;
            Registr.Visibility = Visibility.Visible;
            ENT.Visibility = Visibility.Collapsed;
            REG.Visibility = Visibility.Visible;
            Regi.Visibility = Visibility.Hidden;
        }
        
        //валидация данных кнопка регистрация
        public void REG_Click(object sender, RoutedEventArgs e)
        { int Count_valid = 0;
          
            //валидация логина
            string Login_Parol = @"\w*";
            if (Regex.IsMatch(PL.Password, Login_Parol, RegexOptions.IgnoreCase) && PL.Password.Length>5)
             Count_valid++; 
            else
            {
                MessageBox.Show("Логин не соответствует условию!"); ;
                PL.Password = "";
                PL.Opacity = 0.5;
            }
            //валидация пароля
            if (Regex.IsMatch(PP.Password, Login_Parol, RegexOptions.IgnoreCase) && PP.Password.Length > 5)
            Count_valid++;
            else
            {
                MessageBox.Show("Пароль не соответствует условию!"); ;
                PP.Password = "";
                PP.Opacity = 0.5;
            }
            //валидация подтверждения пароля
            if (PP.Password != TBparol2.Password)
            {
                MessageBox.Show("Подтверждение пароля не выполнено!"); ;
                TBparol2.Password = "";
                TBparol2.Opacity = 0.5;
            }
            else  Count_valid++; 
            //валидация электронной почты TBemail2.Password
            string LoginEmail = @"[0-9a-z]+[._]*[0-9a-z]*@[a-z]+\.[a-z]+";
            if (Regex.IsMatch(TBemail2.Password, LoginEmail, RegexOptions.IgnoreCase))
            Count_valid++; 
            else
            {
                MessageBox.Show("E-mail не соответствует условию!"); 
                PL.Password = "";
                PL.Opacity = 0.5;
            }
            //проверяем установлен ли Chrome на компьютере
            string[] fileEntries = Directory.GetFiles(@"C:\Program Files\Google", "chrome.exe", SearchOption.AllDirectories);
            if (fileEntries[0].Length !=0) Count_valid++;
            else
            {
                MessageBox.Show("Для работы приложения требуется\nустановленный браузер Chrome\nв папке Program Files, диска С");
            }
            //проверяем есть ли в базе логин и e-mail
            using (ApplicationContext db = new())
            {
                List<Login_parol> Poisk = [.. db.LogPar];
                foreach (Login_parol log_email in Poisk)
                {
                    if (PL.Password == log_email.Login )
                    {
                        MessageBox.Show("Такой логин уже зарегестрирован!");
                        Count_valid--;
                    }
                    if (PL.Password == log_email.Login )
                    {
                        MessageBox.Show("Такой E-mail уже зарегестрирован!");
                        Count_valid--;
                    }
                }
            }
            if (Count_valid == 5) 
            {
                // отправка логина и пароля на почту пользователя  
                Mail.Milo($"{TBemail2.Password}", "Vash login i parol", $"Login - {PL.Password}, parol - {PP.Password}");
                

                // объект логин-пароль
                using (ApplicationContext db = new())
                {
                     Login_parol logpar = new()
                    {
                     Login = PL.Password,
                     Parol = PP.Password,
                     Email = TBemail2.Password
                    };
                    db.LogPar.Add(logpar);
                    db.SaveChanges();
                  
                    List<Login_parol> Proverka = [.. db.LogPar];
                    foreach (Login_parol login_ in Proverka)
                    {
                        MessageBox.Show($"{login_.Login} - {login_.Parol}");
                    }
                }
                MessageBox.Show("Регистрация прошла успешно, на вашу эл,почту отправлены логин и пароль");
                MainWindow MainWindow = new();
                MainWindow.Show();
                this.Close();
            }

        }

        public  void ENT_Click(object sender, RoutedEventArgs e)
        {
            int countparol = 0;
            //подключаем базу данных с паролем и выгружаем её3
            using (ApplicationContext db = new())
            {
               List<Login_parol> Proverka = [..db.LogPar];
                foreach (Login_parol login_ in Proverka)
                {
                    if (PL.Password == login_.Login && PP.Password == login_.Parol)
                    {
                        MainWindow MainWindow = new();
                        MainWindow.Show();
                        countparol = 1;
                        this.Close();
                    }
                }
            }
            if (countparol == 0)
            {
                MessageBox.Show("Такой логин или пароль отсутствуют");
                PL.Password = "";
                PL.Opacity = 0.5;
                PP.Password = "";
                PP.Opacity = 0.5;
                LP LPWindow = new ();
                LPWindow.Show();
            }
        }

    }
}
