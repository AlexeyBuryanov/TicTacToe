using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Threading;
using Hosting.ServiceCore;

namespace Hosting
{
    public partial class MainWindow
    {
        /// <summary>Хост</summary>
        private ServiceHost Host { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ButtonStart_OnClickAsync(this, null);
        } // MainWindow


        /// <summary>Старт службы</summary>
        private async void ButtonStart_OnClickAsync(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра класса-хоста, который публикует службу 
            // (указывается сервис-контракт и адрес сервиса (службы))
            Host = new ServiceHost(typeof(Service), new Uri($@"net.tcp://{TextBoxHostName.Text}:{TextBoxPort.Text}/TicTacToe"));

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
            
            // Задание конечных точек для сервиса
            Host.AddServiceEndpoint(typeof(IService), netTcpBinding, "Game");

            // Асинхронный вызов уведомления в статус-баре в текущем потоке диспетчера
            await Dispatcher.InvokeAsync(() => {
                Status.Text = "Запуск...";
                ButtonStart.IsEnabled = false;
            }, DispatcherPriority.Normal);

            // Старт сервиса (асинхронно)
            Host.BeginOpen(OpenCallbackAsync, null);
        } // ButtonStart_OnClickAsync

        private async void OpenCallbackAsync(IAsyncResult ar)
        {
            // Завершаем ассинхронную операцию открытия
            Host.EndOpen(ar);
            await Dispatcher.InvokeAsync(() => {
                Status.Text = "Сервер ожидает подключений...";
                ButtonStart.IsEnabled = true;
                TextBoxHostName.IsEnabled = false;
                TextBoxPort.IsEnabled = false;
            }, DispatcherPriority.Normal);
        } // OpenCallbackAsync


        private async void ButtonStop_OnClickAsync(object sender, RoutedEventArgs e)
        {
            // Начинаем ассинхронное закрытие объекта связи
            Host.BeginClose(StopCallbackAsync, null);
            await Dispatcher.InvokeAsync(() => {
                Status.Text = "Остановка сервера...";
                ButtonStop.IsEnabled = false;
            }, DispatcherPriority.Normal);
        } // ButtonStop_OnClickAsync

        private async void StopCallbackAsync(IAsyncResult ar)
        {
            // Завершаем ассинхронную операцию закрытия
            Host.EndClose(ar);
            await Dispatcher.InvokeAsync(() => {
                Status.Text = "Сервер остановлен.";
                ButtonStop.IsEnabled = true;
                TextBoxHostName.IsEnabled = true;
                TextBoxPort.IsEnabled = true;
            }, DispatcherPriority.Normal);
        } // StopCallbackAsync
    } // MainWindow
} // Hosting