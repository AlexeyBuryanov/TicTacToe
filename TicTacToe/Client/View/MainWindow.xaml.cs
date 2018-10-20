namespace Client.View
{
    using System;
    using System.Windows;
    using Model;
    using ViewModel;
    using Windows;

    public partial class MainWindow
    {
        // Основные свойства с которыми будем работать.
        // Главное окно передаётся во ViewModel
        // Свойство Common делается public
        // Благодаря этому имеем доступ ко всем нужным свойствам отовсюду
        public Common Common { get; set; }

        public MainWindow()
        {
            Common = new Common();

            // Окно входа
            var win = new StartAppWindow();
            var show = win.ShowDialog();

            if (show != null && show.Value == false) {
                Application.Current.Shutdown();
            } else {
                // Если пользователь вошёл успешно
                InitializeComponent();

                var ss = new SplashScreen(@"Images\splashScreen.png");
                ss.Show(true, true);
                ss.Close(new TimeSpan(0, 0, 1));

                Common.LoginData = new LoginData {
                    HostName = win.TextBoxHostName.Text,
                    Port = win.TextBoxPort.Text,
                    UserName = win.TextBoxUserName.Text
                };

                DataContext = new ApplicationViewModel(this);
            } // if-else
        } // MainWindow


        /*ОБРАБОТЧИКИ СОБЫТИЙ
         *******************************************************************************/

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try {
                // Удаляем пользователя и обновляем список пользователей 
                Common.Client.DeleteUserFromLobbyList(Common.LoginData.UserName);
            } catch (Exception) {
                // ignored
            } // try-catch
        } // Window_Closing
    } // class MainWindow
} // Client