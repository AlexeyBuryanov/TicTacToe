using System;

namespace Client.Model
{
    using ServiceCore;

    /// <summary>
    /// Основные свойства и методы, которые приходится 
    /// выносить и в главном окне (для обработки событий), и в ViewModel 
    /// для взаимодействия с ними в рамках паттерна MVVM
    /// </summary>
    public class Common
    {
        public IService Client { get; set; }
        public LoginData LoginData { get; set; }
        public IntPtr Hwnd { get; set; }

        public Common()
        {
            LoginData = new LoginData();
        } // BasicProp
    } // Common
} // Client.Model