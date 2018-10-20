/*==========================================================
**
** Логика взаимодействия для StartAppWindow.xaml
**
** Copyright(c) Alexey Bur'yanov, 2017
** <OWNER>Alexey Bur'yanov</OWNER>
** 
===========================================================*/

namespace Client.Windows
{
    using System;
    using System.Windows;
    using System.Linq;
    using LinqToSql;

    public partial class StartAppWindow
    {
        private TicTacToeDataContext Db { get; }

        public StartAppWindow()
        {
            InitializeComponent();

            Db = new TicTacToeDataContext();
        } // StartAppWindow


        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        } // ButtonExit_Click


        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxUserName.Text) || 
                string.IsNullOrWhiteSpace(PasswordBoxPassword.Password))
                return;

            try {
                // Проверяем введённые данные на авторизацию
                var clients = Db.Clients
                    .First(l => l.UserName == TextBoxUserName.Text && l.Password == PasswordBoxPassword.Password);

                DialogResult = true;
            } catch (Exception) {
                TextBlockWarning.Text = "Такого пользователя не существует!";
                TextBlockWarning.Visibility = Visibility.Visible;
            } // try-catch
        } // ButtonLogin_Click


        private void HyperLinkRegistration_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            var win = new RegistrationWindow();
            var show = win.ShowDialog();

            // Если регистрация юзера прошла, добавляем пользователя в базу
            if (show != null && show.Value) {
                var cl = new Client {
                    Email = win.Login.Email,
                    Id = win.Login.Id,
                    Password = win.Login.Password,
                    UserName = win.Login.UserName
                };

                Db.Clients.InsertOnSubmit(cl);
                Db.SubmitChanges();
            } // if

            ShowDialog();
        } // HyperLinkRegistration_Click
    } // StartAppWindow
} // Client