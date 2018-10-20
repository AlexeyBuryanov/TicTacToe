/*==========================================================
**
** Логика взаимодействия для AboutWindow.xaml
**
** Copyright(c) Alexey Bur'yanov, 2017
** <OWNER>Alexey Bur'yanov</OWNER>
** 
===========================================================*/

namespace Client.Windows
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();

            ProductName.Text = AssemblyProduct;
            Version.Text     = $"Версия {AssemblyVersion}";
            Copyright.Text   = AssemblyCopyright;
            CompanyName.Text = AssemblyCompany;
            Description.Text = AssemblyDescription;
        } // AboutWindow
        public AboutWindow(Window owner) : this()
        {
            Owner = owner;
        } // AboutWindow


        #region Методы доступа к атрибутам сборки
        // Заголовок программы
        public string AssemblyTitle {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length <= 0)
                    return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                return titleAttribute.Title != "" ? titleAttribute.Title : System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            } // get
        } // AssemblyTitle

        // Версия
        public string AssemblyVersion
            => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        // Описание
        public string AssemblyDescription {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            } // get
        } // AssemblyDescription

        // Имя продукта (приложения)
        public string AssemblyProduct {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            } // get
        } // AssemblyProduct

        // Авторские права
        public string AssemblyCopyright {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            } // get
        } // AssemblyCopyright

        // Организация
        public string AssemblyCompany {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            } // get
        } // AssemblyCompany
        #endregion


        private void AboutWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        } // AboutWindow_MouseLeftButtonDown
    } // class AboutWindow : Window
} // namespace WPF_Template.Windows
