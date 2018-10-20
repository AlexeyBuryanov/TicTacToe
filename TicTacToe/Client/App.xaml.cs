namespace Client
{
    using System;
    using System.Threading;
    using System.Windows;
    using View;


    /// <summary>Логика взаимодействия приложения</summary>
    public partial class App
    {
        public Mutex Mutex { get; set; }

        public App()
        {
            InitializeComponent();
            
            // Перехват необработанных исключений
            DispatcherUnhandledException += (e, arg) =>
                MessageBox.Show(arg.Exception.Message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        } // App


        // Точка входа приложения
        [STAThread]
        private static void Main()
        {
            var app = new App();
            var win = new MainWindow();
            app.Run(win);
        }  // Main


        // Запуск только одного экземпляра приложения
        private void AppStartup(object sender, StartupEventArgs e)
        {
            const string mutexName = "AppMutexUnique123"; 

            Mutex = new Mutex(true, mutexName, out var createdNew);

            if (!createdNew) {
                //Shutdown();
            } // if
        } // ApplStartup
    } // class App
} // NetworkFileStorage