using DatabaseManager.Services;
using Web_CardsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarLib;

namespace DatabaseManager
{
    public class Manager : IManager
    {
        /// <summary>
        /// Adds a new game save to database.
        /// </summary>
        /// <param name="currentGame">The byte[] of the serialized data that you wish to save.</param>
        /// <param name="gameID">The id of the game tyoe you are saving.</param>
        /// <param name="user">The user id of the current user that is saving.</param>
        /// <param name="saveName">The name that the user wants to save to.</param>
        public void SaveToDataBase(byte[] currentGame, int gameID, string user, string saveName)
        {
            using (CardsEntities db = new CardsEntities())
            {
                string savedNames = "";
                int iteration = 1;
                foreach (SaveTable saves in db.SaveTables.Where(save => save.gameId == gameID && save.userId.Equals(user)))
                {
                    if (iteration == 1)
                    {
                        savedNames = saves.saveName;
                    }
                    if (saves.saveName.Equals(savedNames))
                    { 
                        savedNames = saves.saveName + $"_({iteration++})";
                    }
                }
                SaveTable newGameSave = new SaveTable()
                {
                    userId = user,
                    gameId = gameID,
                    savedData = currentGame,
                    saveName = savedNames,
                    GameTable = db.GameTables.Where(game => game.id == gameID).First()
                };
                db.SaveTables.Add(newGameSave);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Looks through the Database for games for a specific user.
        /// </summary>
        /// <param name="user">The user ID</param>
        /// <returns>Return a dictonary of strings of the names of all the save games that the user has and has that save datas id.</returns>
        public Dictionary<string, long> GetGamesForUser(string user)
        {
            Dictionary<string, long> gameInfo = new Dictionary<string, long>();
            using(CardsEntities db = new CardsEntities())
            {
                for (int saves = 0; saves < db.SaveTables.Where(save => save.userId.Equals(user)).ToList().Count; saves++)
                {
                    gameInfo.Add(db.SaveTables.Where(save => save.userId.Equals(user)).ToList().ElementAt(saves).saveName, db.SaveTables.Where(save => save.userId.Equals(user)).ToList().ElementAt(saves).saveId);
                }
            }
            return gameInfo;
        }
        /// <summary>
        /// This returns the id of the game based off the name of the game
        /// </summary>
        /// <param name="gameName">The name of the game whose id you want to retrive</param>
        /// <returns>Returns the gameId of the game you are looking for.</returns>
        public int GetGameIdBasedOffNameOfGame(string gameName)
        {
            using (CardsEntities db = new CardsEntities())
            {
                return db.GameTables.Where(game => game.name.Equals(gameName)).First().id;
            }
        }
        /// <summary>
        /// This returns the gameID for a specific save that a user has.
        /// </summary>
        /// <param name="user">The user Id of the current user.</param>
        /// <param name="saveId">The save Id for the game that you want to retrive</param>
        /// <returns>Returns the gameID</returns>
        public int GetGameId(string user, long saveId)
        {
            using (CardsEntities db = new CardsEntities())
            {
                return db.SaveTables.Where(saves => saves.userId.Equals(user) && saves.saveId == saveId).First().gameId;
            }
        }
        /// <summary>
        /// This method returns the byte data of a save game based off the user ID and the gameId
        /// </summary>
        /// <param name="user">This is the current user.</param>
        /// <param name="gameId">This is the ID for the game that was saved </param>
        /// <returns>Returns a byte[] of the save data that is selected.</returns>
        public byte[] GetGameByIdAndUser(string user, int gameId, long saveId)
        {
            byte[] serializedData;
            using (CardsEntities db = new CardsEntities())
            {
                serializedData = db.SaveTables.Where(saves => saves.userId.Equals(user) && saves.gameId == gameId && saves.saveId == saveId).First().savedData;
            }
            return serializedData;
        }
        /// <summary>
        /// Remvoes save games from database.
        /// </summary>
        /// <param name="gameID">The game id of the save</param>
        /// <param name="userId">The user id of the current user</param>
        /// <param name="saveId">The save id of the save data that you wish to delete</param>
        public void DeleteGameByID(int gameID, string userId, long saveId)
        {
            using(CardsEntities db = new CardsEntities())
            {

                SaveTable saveToBeDeleted = db.SaveTables.Where(save => save.gameId == gameID && save.saveId == saveId && save.userId.Equals(userId)).First();

                db.SaveTables.Remove(saveToBeDeleted);

                db.SaveChanges();
            }
        }
    }
}
