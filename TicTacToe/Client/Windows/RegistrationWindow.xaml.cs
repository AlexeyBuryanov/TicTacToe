namespace Client.Windows
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using Model;


    public partial class RegistrationWindow
    {
        // Для проверки e-mail
        private const string Pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + 
                                       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        public Login Login { get; private set; }


        public RegistrationWindow()
        {
            InitializeComponent();
        } // RegistrationWindow


        private void ButtonRegistr_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxUserName.Text) ||
                !string.IsNullOrWhiteSpace(TextBoxEmail.Text) ||
                !string.IsNullOrWhiteSpace(PasswordBoxPassword.Password) ||
                !string.IsNullOrWhiteSpace(PasswordBoxPasswordRepeat.Password)) {

                if (!Regex.IsMatch(TextBoxEmail.Text, Pattern, RegexOptions.IgnoreCase)) {
                    TextBlockWarning.Text = "Некорректный e-mail";
                    TextBlockWarning.Visibility = Visibility.Visible;
                    return;
                } // if

                if (PasswordBoxPassword.Password != PasswordBoxPasswordRepeat.Password) {
                    TextBlockWarning.Text = "Пароли не совпадают";
                    TextBlockWarning.Visibility = Visibility.Visible;
                    return;
                } // if

                var login = new Login {
                    UserName = TextBoxUserName.Text,
                    Password = PasswordBoxPassword.Password,
                    Email = TextBoxEmail.Text
                };

                Login = login;

                DialogResult = true;
            } else {
                TextBlockWarning.Text = "Введите логин, пароль и электронную почту";
                TextBlockWarning.Visibility = Visibility.Visible;
            } // if-else
        } // ButtonRegistr_Click
    } // RegistrationWindow
} // Client.Windows