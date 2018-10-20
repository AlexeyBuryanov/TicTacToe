using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Hosting.ServiceCore
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    [CallbackBehavior(IncludeExceptionDetailInFaults = true)]
    public class Service : IService
    {
        private IServiceCallback Callback { get; set; }
        private List<User> UserList { get; set; }
        private List<string> GameList { get; set; }
        private TicTacToe Game { get; set; }


        public Service()
        {
            UserList = new List<User>();
            GameList = new List<string>();
            Game = new TicTacToe();
        } // Service


        /// <summary>Добавляет нового юзера в список "В СЕТИ"</summary>
        /// <param name="userName">Ник игрока</param>
        public async void AddUserToLobbyList(string userName)
        {
            Callback = OperationContext.Current.GetCallbackChannel<IServiceCallback>();
            var user = new User {
                UserName = userName,
                Callback = Callback
            };
            UserList.Add(user);

            // Формируем список имён всех пользователей
            var namesLst = UserList.Select(usr => usr.UserName).ToList();

            // Перебираем пользователей и шлём им новый список имён
            await SendListToAll(namesLst);
        } // AddUserToLobbyList


        /// <summary>Удаляет юзера со списка "В СЕТИ"</summary>
        /// <param name="userName">Ник игрока</param>
        public async void DeleteUserFromLobbyList(string userName)
        {
            // Формируем список без пользователя, что вышел
            UserList = UserList
                .Where(u => u.UserName != userName)
                .ToList();

            // Формируем новый список имён
            var namesLst = UserList
                .Select(usr => usr.UserName)
                .ToList();

            // Перебираем пользователей, что остались и шлём им новый список имён
            await SendListToAll(namesLst);
        } // DeleteUserFromLobbyList


        /// Служебный метод для AddUserToLobbyList и DeleteUserFromLobbyList
        /// <summary>Перебирает всех пользователей и по Callback-вызову обновляет список "В СЕТИ"</summary>
        /// <param name="userList">Список, который отправляется</param>
        public Task SendListToAll(List<string> userList)
        {
            return Task.Factory.StartNew(() => {
                UserList
                    .ForEach(user => {
                        user.Callback?.SendUserListCallback(userList);
                    });
            });
        } // SendListToAll


        /// <summary>Добавляет строку с текущей игрой в список "В ИГРЕ"</summary>
        /// <param name="game">Строка кто против кого</param>
        public async void AddGameToGameList(string game)
        {
            GameList.Add(game);

            // Перебираем пользователей и шлём им новый список имён
            await SendGamesListToAllAsync(GameList);
        } // AddGameToGameList


        /// <summary>Удаляет строку с игрой со списка "В ИГРЕ"</summary>
        /// <param name="game">Строка кто против кого</param>
        public async void DeleteGameFromGameList(string game)
        {
            // Формируем список без игры
            GameList = GameList
                .Where(g => g != game)
                .ToList();

            // Перебираем пользователей и шлём им новый список текущих игр
            await SendGamesListToAllAsync(GameList);
        } // DeleteGameFromGameList


        /// <summary>
        /// Делает callback-вызов у пользователя, который хочет получить список текущих игр
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public async void GetCurrentGamesAsync(string userName)
        {
            await Task.Factory.StartNew(() => {
                UserList
                    .ForEach(user => {
                        if (user.UserName == userName)
                            user.Callback?.SendGameListCallback(GameList);
                    });
            });
        } // GetCurrentGamesAsync


        /// Служебный метод для AddUserToInGameList и DeleteUserFromInGameList
        /// <summary>Перебирает всех пользователей и по Callback-вызову обновляет список "В ИГРЕ"</summary>
        /// <param name="gameList">Список игр</param>
        public Task SendGamesListToAllAsync(List<string> gameList)
        {
            return Task.Factory.StartNew(() => {
                UserList
                    .ForEach(user => {
                        user.Callback?.SendGameListCallback(gameList);
                    });
            });
        } // SendGamesListToAllAsync


        /// <summary>Отправляет сообщение всем юзерам (кроме отправителя)</summary>
        /// <param name="msg">Сообщение</param>
        /// <param name="sender">Отправитель</param>
        public void SendMessageToAll(string msg, string sender)
        {
            UserList.ForEach(user => {
                if (sender != user.UserName) {
                    user.Callback.SendMessageCallback(msg);
                } // if
            });
        } // SendMessageToAll


        /// <summary>Отправляет приглашение вступить в игру</summary>
        /// <param name="from">От кого</param>
        /// <param name="to">Кому</param>
        public async void SendInviteToAsync(string from, string to)
        {
            await Task.Factory.StartNew(() => {
                foreach (var user in UserList.Where(u => u.UserName == to)) {
                    Callback = user.Callback;
                } // foreach
                Callback?.SendInviteCallback(from, to);
            });
        } // SendInviteToAsync


        /// <summary>
        /// Даёт знать, что пользователь принял приглашение на игру
        /// </summary>
        /// <param name="from">От кого</param>
        /// <param name="to">Кому</param>
        public async void AcceptInviteAsync(string from, string to)
        {
            await Task.Factory.StartNew(() => {
                var players = new User[2];

                UserList.ForEach(user => {
                    if (from == user.UserName) {
                        players[0] = user;
                    } // if
                    if (to == user.UserName) {
                        players[1] = user;
                    } // if
                });
                players[0].Callback.AcceptInviteCallback(from, to);
                players[1].Callback.AcceptInviteCallback(from, to);
            });
        } // AcceptInviteAsync


        /// <summary>
        /// Делает ход. Отображает ход у обоих игроков
        /// </summary>
        /// <param name="player1">Имя игрока 1</param>
        /// <param name="player2">Имя игрока 2</param>
        /// <param name="row">Строка, где был совершён ход</param>
        /// <param name="col">Столбец, где был совершён ход</param>
        /// <param name="isCross">Крестик это? Если false, то нолик</param>
        public async void MakeMoveAsync(string player1, string player2, int row, int col, bool isCross)
        {
            await Task.Factory.StartNew(() => {
                var p1 = new User();
                var p2 = new User();

                foreach (var user in UserList) {
                    if (user.UserName == player1) {
                        p1 = user;
                    } // if
                    if (user.UserName == player2) {
                        p2 = user;
                    } // if
                } // foreach

                Game.Fill(row, col, isCross ? 1 : 2);

                p1.Callback?.MakeMoveCallback(row, col, isCross);
                p2.Callback?.MakeMoveCallback(row, col, isCross);
            });
        } // MakeMoveAsync


        /// <summary>
        /// Показывает победителя
        /// </summary>
        /// <param name="player1">Имя игрока 1</param>
        /// <param name="player2">Имя игрока 2</param>
        /// <param name="winner">Знак победителя. X - O или '\0', если ничья</param>
        public async void ShowWinnerAsync(string player1, string player2, char winner)
        {
            await Task.Factory.StartNew(() => {
                var p1 = new User();
                var p2 = new User();

                foreach (var user in UserList) {
                    if (user.UserName == player1) {
                        p1 = user;
                    } // if
                    if (user.UserName == player2) {
                        p2 = user;
                    } // if
                } // foreach

                p1.Callback?.ShowWinnerCallback(player1, player2, winner);
                p2.Callback?.ShowWinnerCallback(player1, player2, winner);
            });
        } // ShowWinnerAsync


        /// <summary>Метод для проверки соединения</summary>
        public void TestConnection() {}
    } // Service
} // WcfServiceLibrary