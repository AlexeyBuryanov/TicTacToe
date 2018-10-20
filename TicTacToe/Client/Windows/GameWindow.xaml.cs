namespace Client.Windows
{
    using System;
    using System.ServiceModel;
    using System.Windows;
    using System.Windows.Input;
    using Model;
    using View;


    /// <summary>Окно с игрой</summary>
    public partial class GameWindow
    {
        /// <summary> Главное окно </summary>
        private MainWindow MainWindow { get; }
        /// <summary> Общие свойства </summary>
        private Common Common { get; }
        /// <summary> Имена игроков </summary>
        private string[] PlayersName { get; }
        /// <summary> Если не крестик, то нолик </summary>
        public bool? IsCross { get; set; }

        public GameWindow()
        {
            InitializeComponent();
        } // GameWindow
        public GameWindow(MainWindow mainWindow, string user1, string user2) : this()
        {
            IsCross = new Random().Next(0, 1) == 0;

            PlayersName = new string[2];
            PlayersName[0] = user1;
            PlayersName[1] = user2;

            // Получаем главное окно
            MainWindow = mainWindow;

            Common = MainWindow.Common;

            try {
                Common.Client.TestConnection();
                Title = $"Крестики-нолики | {PlayersName[0]} vs {PlayersName[1]}";
            } catch (EndpointNotFoundException) {
                Title = "Крестики-нолики | Не удаётся подключиться к серверу. Возможно сервер недоступен.";
            } // try-catch
        } // GameWindow


        public async void DrawSign(int row, int col, bool isCross)
        {
            if (row == 0 && col == 0) {
                if (isCross && Border00Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border00Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border00Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border00Null.Visibility = Visibility.Visible;
                    });
            } // if
            if (row == 0 && col == 1) {
                if (isCross && Border01Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border01Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border01Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border01Null.Visibility = Visibility.Visible;
                    });
            } // if
            if (row == 0 && col == 2) {
                if (isCross && Border02Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border02Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border02Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border02Null.Visibility = Visibility.Visible;
                    });
            } // if


            if (row == 1 && col == 0) {
                if (isCross && Border10Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border10Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border10Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border10Null.Visibility = Visibility.Visible;
                    });
            } // if
            if (row == 1 && col == 1) {
                if (isCross && Border11Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border11Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border11Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border11Null.Visibility = Visibility.Visible;
                    });
            } // if
            if (row == 1 && col == 2) {
                if (isCross && Border12Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border12Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border12Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border12Null.Visibility = Visibility.Visible;
                    });   
            } // if


            if (row == 2 && col == 0) {
                if (isCross && Border20Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border20Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border20Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border20Null.Visibility = Visibility.Visible;
                    });
            } // if
            if (row == 2 && col == 1) {
                if (isCross && Border21Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border21Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border21Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border21Null.Visibility = Visibility.Visible;
                    });
            } // if
            if (row == 2 && col == 2) {
                if (isCross && Border22Null.Visibility != Visibility.Visible) {
                    await Dispatcher.InvokeAsync(() => {
                        Border22Cross.Visibility = Visibility.Visible;
                    });
                } else if (Border22Cross.Visibility != Visibility.Visible)
                    await Dispatcher.InvokeAsync(() => {
                        Border22Null.Visibility = Visibility.Visible;
                    });
            } // if
        } // DrawSign


        private void Border00_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 0, 0, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 0, 0, IsCross != null && IsCross.Value);
            } // if
        } // Border00_MouseDown

        private void Border01_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 0, 1, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 0, 1, IsCross != null && IsCross.Value);
            } // if
        } // Border01_MouseDown

        private void Border02_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 0, 2, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 0, 2, IsCross != null && IsCross.Value);
            } // if
        } // Border02_MouseDown

        private void Border10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 1, 0, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 1, 0, IsCross != null && IsCross.Value);
            } // if
        } // Border10_MouseDown

        private void Border11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 1, 1, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 1, 1, IsCross != null && IsCross.Value);
            } // if
        } // Border11_MouseDown

        private void Border12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 1, 2, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 1, 2, IsCross != null && IsCross.Value);
            } // if
        } // Border12_MouseDown

        private void Border20_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 2, 0, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 2, 0, IsCross != null && IsCross.Value);
            } // if
        } // Border20_MouseDown

        private void Border21_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 2, 1, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 2, 1, IsCross != null && IsCross.Value);
            } // if
        } // Border21_MouseDown

        private void Border22_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCross != null && IsCross.Value) {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 2, 2, IsCross.Value);
            } else {
                Common.Client.MakeMoveAsync(PlayersName[0], PlayersName[1], 2, 2, IsCross != null && IsCross.Value);
            } // if
        } // Border22_MouseDown


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Common.Client.DeleteGameFromGameList($"{PlayersName[0]} vs {PlayersName[1]}");
        } // Window_Closing
    } // GameWindow
} // Client.Windows