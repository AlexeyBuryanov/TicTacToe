namespace Client.Model
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Класс, объекты которого будут храниться в БД
    /// </summary>
    public class Login : INotifyPropertyChanged
    {
        public int Id { get; set; }


        private string _userName;
        public string UserName {
            get => _userName;
            set {
                _userName = value;
                OnPropertyChanged();
            } // set
        } // UserName


        private string _password;
        public string Password {
            get => _password;
            set {
                _password = value;
                OnPropertyChanged();
            } // set
        } // Password


        private string _email;
        public string Email {
            get => _email;
            set {
                _email = value;
                OnPropertyChanged();
            } // set
        } // Email


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        } // OnPropertyChanged
    } // Login


    /// <summary>
    /// Данные подключения текущего юзера
    /// </summary>
    public class LoginData
    {
        public string HostName { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
    } // LoginData
} // Client.Model