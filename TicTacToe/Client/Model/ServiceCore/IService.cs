using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Client.Model.ServiceCore
{
    [ServiceContract]
    public interface IServiceCallback
    {
        [OperationContract]
        void SendMessageCallback(string msg);

        [OperationContract]
        void SendUserListCallback(List<string> userList);

        [OperationContract]
        void SendGameListCallback(List<string> gameList);

        [OperationContract]
        void SendInviteCallback(string from, string to);

        [OperationContract]
        void AcceptInviteCallback(string from, string to);

        [OperationContract]
        void MakeMoveCallback(int row, int col, bool isCross);

        [OperationContract]
        void ShowWinnerCallback(string player1, string player2, char winner);
    } // IServiceCallback


    [ServiceContract(CallbackContract = typeof(IServiceCallback))]
    public interface IService
    {
        [OperationContract]
        void AddUserToLobbyList(string userName);

        [OperationContract]
        void DeleteUserFromLobbyList(string userName);

        [OperationContract]
        void AddGameToGameList(string game);

        [OperationContract]
        void DeleteGameFromGameList(string game);

        [OperationContract]
        void GetCurrentGamesAsync(string userName);

        [OperationContract]
        void SendMessageToAll(string msg, string sender);

        [OperationContract]
        void SendInviteToAsync(string from, string to);

        [OperationContract]
        void AcceptInviteAsync(string from, string to);

        [OperationContract]
        void MakeMoveAsync(string player1, string player2, int row, int col, bool isCross);

        [OperationContract]
        void ShowWinnerAsync(string player1, string player2, char winner);

        [OperationContract]
        void TestConnection();
    } // IService


    /// <summary>Класс игрока</summary>
    [DataContract]
    public class User
    {
        [DataMember]
        public IServiceCallback Callback;

        [DataMember]
        public string UserName;
    } // User


    /// <summary>
    /// Крестики-нолики
    /// </summary>
    [DataContract]
    public class TicTacToe
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        [DataMember]
        public char[,] Map { get; set; }


        public TicTacToe()
        {
            Map = new char[3, 3];
        } // TicTacToe


        public char GetMap(int i, int j)
        {
            if (i < 0 || i >= 3 || j < 0 || j >= 3)
                return '\0';
            return Map[i, j];
        } // GetMap


        /// <summary>
        /// Изменение в Map, вывод крестика или нолика в экранном отображении игрового поля
        /// </summary>
        /// <param name="row">строка в игровом поле</param>
        /// <param name="col">столбец в игровом поле</param>
        /// <param name="sign">1 - X, 2 - 0</param>
        public void Fill(int row, int col, int sign)
        {
            // Запрет записи в занятую ячейку игрового поля
            if (Map[row, col] != '\0')
                return;

            switch (sign) {
                case 1:
                    Map[row, col] = 'X';
                    break;
                case 2:
                    Map[row, col] = 'O';
                    break;
            } // switch
        } // Fill


        /// <summary>
        /// Возврат true, если есть строка или столбец или диагональ
        /// заполненные одним и тем же символом
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsFinish()
        {
            // Счетчик занятых ячеек игрового поля
            var nOccuped = 0;

            // Проверка на заполнение всего поля
            for (var i = 0; i < 3; i++) {
                for (var j = 0; j < 3; j++) {
                    if (Map[i, j] != '\0')
                        nOccuped++;
                } // for j
            } // for i
            if (nOccuped == 9)
                return true;

            // Проверка строк на заполнение одинаковыми символами
            for (var i = 0; i < 3; i++) {
                // Пропускаем строки, у которых первая ячейка не заполнена
                if (Map[i, 0] == '\0')
                    continue;
                nOccuped = 1;
                for (var j = 1; j < 3; j++) {
                    if (Map[i, j] == Map[i, 0])
                        nOccuped++;
                } // for j
                if (nOccuped == 3)
                    return true;
            } // for i

            // Проверка столбцов на заполнение одинаковыми символами
            for (var j = 0; j < 3; j++) {
                // Пропускаем столбцы, у которых первая ячейка не заполнена
                if (Map[0, j] == '\0')
                    continue;
                nOccuped = 1;
                for (var i = 1; i < 3; i++) {
                    if (Map[i, j] == Map[0, j])
                        nOccuped++;
                } // for i
                if (nOccuped == 3)
                    return true;
            } // for j

            // Проверка главной диагонали
            nOccuped = 1;
            for (var i = 1; i < 3; i++) {
                if (Map[i, i] == Map[0, 0] && Map[0, 0] != '\0')
                    nOccuped++;
            } // for i
            if (nOccuped == 3)
                return true;

            // Проверка побочной диагонали
            nOccuped = 1;
            for (var i = 1; i < 3; i++) {
                if (Map[i, 2 - i] == Map[0, 2] && Map[0, 2] != '\0')
                    nOccuped++;
            } // for i
            return nOccuped == 3;
        } // IsFinish


        /// <summary>
        /// Возвращаем символ - признак победителя - X или O
        /// Если ничья, то возвращаем пробел
        /// </summary>
        /// <returns>X или O или '\0'</returns>
        public char GetWinner()
        {
            // Счетчик занятых ячеек
            int nOccuped;

            // Проверка строк на заполнение одинаковыми символами
            for (var i = 0; i < 3; i++) {
                // Пропускаем строки, у которых первая ячейка не заполнена
                if (Map[i, 0] == '\0')
                    continue;
                nOccuped = 1;
                for (var j = 1; j < 3; j++) {
                    if (Map[i, j] == Map[i, 0])
                        nOccuped++;
                } // for j
                if (nOccuped == 3)
                    return Map[i, 0];
            } // for i

            // Проверка столбцов на заполнение одинаковыми символами
            for (var j = 0; j < 3; j++) {
                // Пропускаем столбцы, у которых первая ячейка не заполнена
                if (Map[0, j] == '\0')
                    continue;
                nOccuped = 1;
                for (var i = 1; i < 3; i++) {
                    if (Map[i, j] == Map[0, j])
                        nOccuped++;
                } // for i
                if (nOccuped == 3)
                    return Map[0, j];
            } // for j

            // Проверка главной диагонали
            nOccuped = 1;
            for (var i = 1; i < 3; i++) {
                if (Map[i, i] == Map[0, 0] && Map[0, 0] != '\0')
                    nOccuped++;
            } // for i
            if (nOccuped == 3)
                return Map[0, 0];

            // Проверка побочной диагонали
            nOccuped = 1;
            for (var i = 1; i < 3; i++) {
                if (Map[i, 2 - i] == Map[0, 2] && Map[0, 2] != '\0')
                    nOccuped++;
            } // for i
            return nOccuped == 3 ? Map[0, 2] : '\0';
        } // GetWinner


        /// <summary>
        /// Очистка игрового поля
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    Map[i, j] = '\0';
        } // Clear
    } // TicTacToe
} // WcfServiceLibrary