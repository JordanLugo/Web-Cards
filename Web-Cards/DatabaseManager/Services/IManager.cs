using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Services
{
    interface IManager
    {
        void SaveToDataBase(byte[] currentGame, int gameID, string user, string saveName);
        Dictionary<string, long> GetGamesForUser(string user, int gameId);
        int GetGameIdBasedOffNameOfGame(string gameName);
        int GetGameId(string user, long saveId);
        byte[] GetGameByIdAndUser(string user, int gameId, long saveId);
        void DeleteGameByID(int gameID, string userId, long saveId);
    }
}
