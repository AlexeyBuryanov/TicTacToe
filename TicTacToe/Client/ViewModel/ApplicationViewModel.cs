/*==========================================================
** 
** ApplicationViewModel
** 
** Copyright(c) Alexey Bur'yanov, 2017
** <OWNER>Alexey Bur'yanov</OWNER>
** 
===========================================================*/

namespace Client.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;
    using System.Windows;
    using System.Windows.Threading;
    using System.Threading.Tasks;
    using LinqToSql;
    using Model;
    using Model.ServiceCore;
    using Service;
    using View;
    using Windows;


    public class ApplicationViewModel : INotifyPropertyChanged, IServiceCallback
    {
        private Common Common { get; }
        private MainWindow MainWindow { get; }
        private DispatcherTimer Timer { get; }
        private TicTacToe Game { get; }
        private GameWindow Gw { get; set; }
        private TicTacToeDataContext Db { get; }
        private string[] PlayersName { get; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        } // OnPropertyChanged


        public ApplicationViewModel(MainWindow mainWindow)
        {
            StatusText = "Добро пожаловать";

            PlayersName = new string[2];
            Db = new TicTacToeDataContext();
            Game = new TicTacToe();

            // Получаем главное окно из конструктора MainWindow
            MainWindow = mainWindow;
            // Получаем общие свойства с того самого public-свойства
            Common = mainWindow.Common;

            //***** СОЗДАНИЕ КОМАНД *****//
            ExitCommand         = new RelayCommand(Exit);
            MinimizeCommand     = new RelayCommand(Minimize);
            MaximizeCommand     = new RelayCommand(Maximize);
            AboutCommand        = new RelayCommand(About);
            SendMessageCommand  = new RelayCommand(SendMessage);
            InviteToGameCommand = new RelayCommand(InviteToGame);

            // Установка таймера
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);

            ConnectWithServer();
            StatusText = $"Подключено к {Common.LoginData.HostName}:{Common.LoginData.Port}";
            Timer.Start();
        } // ApplicationViewModel


        /// <summary>Проверяем коннект с сервером</summary>
        private async void Timer_Tick(object sender, EventArgs e)
        {
            try {
                Common.Client.TestConnection();
            } catch (CommunicationException) {
                StatusText = "Не удаётся подключиться к серверу. Возможно сервер недоступен.";
                await MainWindow.Dispatcher.InvokeAsync(() => {
                    MainWindow.DataGridWaitUsers.Items.Clear();
                });
            } // try-catch
        } // Timer_Tick


        /// <summary>Соединение с сервером. В данном случае с конечной точкой лобби.</summary>
        public void ConnectWithServer()
        {
            // Для связи используем протокол TCP-IP
            var netTcpBinding = new NetTcpBinding {
                CloseTimeout = new TimeSpan(0, 0, 2, 0),
                OpenTimeout = new TimeSpan(0, 0, 2, 0),
                ReceiveTimeout = new TimeSpan(0, 0, 10, 0),
                SendTimeout = new TimeSpan(0, 0, 2, 0),
                TransactionFlow = false,
                TransferMode = TransferMode.Buffered,
                TransactionProtocol = TransactionProtocol.OleTransactions,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                // Максимум игроков в очереди на подключение
                ListenBacklog = 10,
                MaxBufferPoolSize = 1000000,
                MaxBufferSize = 2055360000,
                MaxReceivedMessageSize = 2055360000,
                // Максимум игроков 20
                MaxConnections = 20
            };

            // Формируем callback-context
            var context = new InstanceContext(this);

            // Создание фабрики
            var channelFactory =
                new DuplexChannelFactory<IService>(context, netTcpBinding,
                    new EndpointAddress($@"net.tcp://{Common.LoginData.HostName}:{Common.LoginData.Port}/TicTacToe/Game"));

            // Создание канала для связи с сервисом
            Common.Client = channelFactory.CreateChannel();

            try {
                // Добавляем вновь подключившегося пользователя в лобби
                Common.Client.AddUserToLobbyList(Common.LoginData.UserName);
                Common.Client.GetCurrentGamesAsync(Common.LoginData.UserName);
                MainWindow.Title = $"Крестики-нолики | {Common.LoginData.UserName}";
            } catch (EndpointNotFoundException) {
                MainWindow.Title = $"Крестики-нолики | {Common.LoginData.UserName}";
                StatusText = "Не удаётся подключиться к серверу. Возможно сервер недоступен.";
            } // try-catch
        } // ConnectWithServer


        //******************** СВОЙСТВА ДЛЯ СВЯЗИ С ИНТЕРФЕЙСОМ ***********************//

        /// <summary>Текстовка в статус-баре</summary>
        private string _statusText;
        public string StatusText {
            get => _statusText;
            private set {
                _statusText = value;
                OnPropertyChanged();
            } // set
        } // StatusText


        //********************************* КОМАНДЫ ***********************************//

        /// <summary>Выход</summary>
        public RelayCommand ExitCommand { get; set; }

        /// <summary>Свернуть</summary>
        public RelayCommand MinimizeCommand { get; set; }

        /// <summary>Развернуть</summary>
        public RelayCommand MaximizeCommand { get; set; }

        /// <summary>О программе</summary>
        public RelayCommand AboutCommand { get; set; }

        /// <summary>Отправить сообщение в чат</summary>
        public RelayCommand SendMessageCommand { get; set; }

        /// <summary>Приглашение в игру</summary>
        public RelayCommand InviteToGameCommand { get; set; }


        //**************************** РЕАЛИЗАЦИИ КОМАНД ******************************//

        /// <summary>Выход</summary>
        private static void Exit(object obj)
        {
            Application.Current.Shutdown();
        } // Exit


        /// <summary>Свернуть</summary>
        private void Minimize(object obj)
        {
            MainWindow.WindowState = WindowState.Minimized;
        } // Minimize


        /// <summary>Развернуть</summary>
        private void Maximize(object obj) {
            MainWindow.WindowState =
                MainWindow.WindowState != WindowState.Maximized 
                ? WindowState.Maximized 
                : WindowState.Normal;
        } // Maximize


        /// <summary>О программе</summary>
        private static void About(object obj)
        {
            var win = new AboutWindow(Application.Current.MainWindow);
            win.ShowDialog();
        } // About


        /// <summary>Отправить сообщение в чат</summary>
        private void SendMessage(object obj)
        {
            // Отправка идёт всем
            Common.Client.SendMessageToAll($"{Common.LoginData.UserName}: {MainWindow.TextBoxMessage.Text}",
                                           $"{Common.LoginData.UserName}");
            AddMsgToChatAsync($"{Common.LoginData.UserName}: {MainWindow.TextBoxMessage.Text}");

            MainWindow.TextBoxMessage.Text = "";
        } // SendMessage


        /// <summary>Пригласить в игру</summary>
        private async void InviteToGame(object obj)
        {
            // Самому себе слать приглашения не разрешаем
            if ((MainWindow.DataGridWaitUsers.SelectedItem as PlayerInfo)?.UserName == Common.LoginData.UserName)
                return;

            await MainWindow.Dispatcher.InvokeAsync(() => {
                // Отображаем эффект ожидания
                MainWindow.ImgSpinner.Visibility = Visibility.Visible;
            });

            // Уведомляем
            StatusText = $"Приглашение пользователю {(MainWindow.DataGridWaitUsers.SelectedItem as PlayerInfo)?.UserName} " +
                         "отправлено. Ожидание...";
            
            // Отправка инвайта
            Common.Client.SendInviteToAsync($"{Common.LoginData.UserName}",
                                       (MainWindow.DataGridWaitUsers.SelectedItem as PlayerInfo)?.UserName);
        } // InviteToGame


        /* Методы
         **********************************************************************
         */

        /// <summary>Добавляет сообщение в листбокс-чат</summary>
        private async void AddMsgToChatAsync(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return;
            await MainWindow.Dispatcher.InvokeAsync(() => {
                MainWindow.ListBoxChat.Items.Add(msg);
            });
        } // AddMsgToChatAsync


        /// <summary>Изменяет список пользователей в лобби (в списке "В СЕТИ")</summary>
        private async void SetLobbyUserListAsync(IEnumerable<string> userList)
        {
            // Очищаем список
            await MainWindow.Dispatcher.InvokeAsync(() => {
                MainWindow.DataGridWaitUsers.Items.Clear();
            });

            userList
                .ToList()
                .ForEach(async uname => {
                    try {
                        // Проверяем есть ли такой пользователь в базе истории
                        var gameHistory = Db.GameHistories.First(history => history.Who == uname);

                        // Если всё-таки есть, смотрим его историю
                        Db.GameHistories
                           .Join(Db.GameResults, g => g.IdResult, g => g.Id, (g, r) => new {
                               g.Who,
                               g.WithWhom,
                               g.GameTime,
                               r.State
                           })
                           .GroupBy(g => g.Who)
                           .Select(g => new {
                               Name = g.Key,
                               Wins = g.Count(w => w.State == "Victory"),
                               Defeats = g.Count(w => w.State == "Defeat")
                           })
                           .ToList()
                           .ForEach(async p => {
                               // Создаём игрока с мини-статистикой
                               var pf = new PlayerInfo {
                                   // Имя
                                   UserName = p.Name,
                                   Wins = p.Wins,
                                   Defeats = p.Defeats
                               };

                               await MainWindow.Dispatcher.InvokeAsync(() => {
                                   MainWindow.DataGridWaitUsers.Items.Add(pf);
                               });
                           });
                    } catch (Exception) {
                        // Если нет, то
                        // создаём игрока с нулевой статистикой
                        var pf = new PlayerInfo {
                            // Имя
                            UserName = uname,
                            Wins = 0,
                            Defeats = 0
                        };

                        // Добавляем
                        await MainWindow.Dispatcher.InvokeAsync(() => {
                            MainWindow.DataGridWaitUsers.Items.Add(pf);
                        });
                    } // try-catch
                });
        } // SetLobbyUserListAsync


        private async void SetListCurrentGamesAsync(List<string> gameList)
        {
            if (gameList.Count == 0) {
                await MainWindow.Dispatcher.InvokeAsync(() => {
                    MainWindow.ListBoxUsersPlay.Items.Clear();
                });
            } // if
            gameList.ForEach(async s => {
                await MainWindow.Dispatcher.InvokeAsync(() => {
                    MainWindow.ListBoxUsersPlay.Items.Add(s);
                });
            });
        } // SetListCurrentGamesAsync


        /// <summary>
        /// Задаёт вопрос о принятии и начинает игру, если приглашение принято
        /// </summary>
        /// <param name="from">От кого прислано приглашение</param>
        /// <param name="to">Кому</param>
        private async void ReplyForGameAsync(string from, string to)
        {
            var reply =
                MessageBox.Show($"{from} приглашает Вас в игру.", "Запрос на игру", 
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes, 
                    MessageBoxOptions.DefaultDesktopOnly);

            if (reply != MessageBoxResult.Yes)
                return;

            await Task.Factory.StartNew(() => {
                Common.Client.AcceptInviteAsync(from, to);
            });
        } // ReplyForGameAsync


        private async void SpinnerOffAndShowGameAsync(string from, string to)
        {
            // Убираем эффект ожидания
            await MainWindow.Dispatcher.InvokeAsync(() => {
                MainWindow.ImgSpinner.Visibility = Visibility.Collapsed;
            });
            StatusText = "Вход в игру...";

            // Добавляем пользователей в список "В ИГРЕ"
            await MainWindow.Dispatcher.InvokeAsync(() => {
                MainWindow.ListBoxUsersPlay.Items.Add($"{from} vs {to}");
            });

            await MainWindow.Dispatcher.InvokeAsync(() => {
                PlayersName[0] = from;
                PlayersName[1] = to;
                Gw = new GameWindow(MainWindow, from, to);
                Gw.Show();
                // Крестик или нолик? Выбор
                var win2 = new CrossOrNullWindow(Gw);
                Gw.IsCross = win2.ShowDialog();
            });
        } // SpinnerOffAndShowGameAsync


        private async void MakeMoveAsync(int row, int col, bool isCross)
        {
            Game.Fill(row, col, isCross ? 1 : 2);

            for (var i = 0; i < 3; i++) {
                for (var j = 0; j < 3; j++) {
                    if (Game.GetMap(i, j) == '\0')
                        continue;
                    if (i == 0 && j == 0) Gw.DrawSign(0, 0, isCross);
                    if (i == 0 && j == 1) Gw.DrawSign(0, 1, isCross);
                    if (i == 0 && j == 2) Gw.DrawSign(0, 2, isCross);
                    if (i == 1 && j == 0) Gw.DrawSign(1, 0, isCross);
                    if (i == 1 && j == 1) Gw.DrawSign(1, 1, isCross);
                    if (i == 1 && j == 2) Gw.DrawSign(1, 2, isCross);
                    if (i == 2 && j == 0) Gw.DrawSign(2, 0, isCross);
                    if (i == 2 && j == 1) Gw.DrawSign(2, 1, isCross);
                    if (i == 2 && j == 2) Gw.DrawSign(2, 2, isCross);
                } // for j
            } // for i

            if (Game.IsFinish()) {
                await MainWindow.Dispatcher.InvokeAsync(() => {
                    Common.Client.ShowWinnerAsync(PlayersName[0], PlayersName[1], Game.GetWinner());
                });
            } // if
        } // MakeMoveAsync


        private async void ShowWinnerAsync(string player1, string player2, char winner)
        {
            await MainWindow.Dispatcher.InvokeAsync(async () => {
                await Task.Factory.StartNew(() => {
                    var reply = MessageBox.Show(winner != '\0'
                        ? $"Победитель \"{winner}\".\nСыграем ещё?"
                        : "Ничья.\nСыграем ещё?", "Игра окончена",
                        MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);

                    //if (reply != MessageBoxResult.Yes)
                    //    return;
                });
            });
            

            // TODO: сыграть ещё и отмена игры
        } // ShowWinnerAsync


        //***************** РЕАЛИЗАЦИЯ ИНТЕРФЕЙСА IServiceCallback ********************//

        public void SendMessageCallback(string msg) => 
            AddMsgToChatAsync(msg);

        public void SendUserListCallback(List<string> userList) => 
            SetLobbyUserListAsync(userList);

        public void SendGameListCallback(List<string> gameList) =>
            SetListCurrentGamesAsync(gameList);

        public void SendInviteCallback(string from, string to) => 
            ReplyForGameAsync(from, to);

        public void AcceptInviteCallback(string from, string to) => 
            SpinnerOffAndShowGameAsync(from, to);

        public void MakeMoveCallback(int row, int col, bool isCross) =>
            MakeMoveAsync(row, col, isCross);

        public void ShowWinnerCallback(string player1, string player2, char winner) =>
            ShowWinnerAsync(player1, player2, winner);
    } // class ApplicationViewModel
} // Client