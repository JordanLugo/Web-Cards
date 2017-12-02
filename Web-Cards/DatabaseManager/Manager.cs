using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_CardsDAL;

namespace DatabaseManager
{
    public class Manager
    {
        /// <summary>
        /// Adds a War Game to the Database
        /// </summary>
        /// <param name="currentGame">The serialized form of War that is to be saved</param>
        public void AddWarGameToDataBase(byte[] currentGame)
        {
            using (cardsEntities db = new cardsEntities())
            {               
                WarSetup currentGameState = (db.WarSetups.Count() == 0)
                ?
                new WarSetup()
                {
                    GameID = 0,
                    UserId = 0,
                    SerializedData = currentGame
                }
                :
                new WarSetup()
                {
                    GameID = db.WarSetups.Last().GameID + 1,
                    UserId = 0,
                    SerializedData = currentGame
                }
                ;
                db.WarSetups.Add(currentGameState);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Looks through the Database for a specific game then takes the serialized Data from that Database and passes that data along
        /// </summary>
        /// <param name="ID">The ID for the Game that you want</param>
        /// <returns>The Serialized Data for the specified instance of War</returns>
        public byte[] GetGameDataByID(int ID)
        {
            using(cardsEntities db = new cardsEntities())
            {
                return db.WarSetups.Where(x => x.GameID == ID).First().SerializedData;
            }
        }
        /// <summary>
        /// Removes a game of War from the Database
        /// </summary>
        /// <param name="gameID">The ID of the game that is to be Deleted</param>
        public void DeleteWarGameByID(int gameID)
        {
            using(cardsEntities db = new cardsEntities())
            {
                WarSetup warSetupToBeDeleted = db.WarSetups.Where(w => w.GameID == gameID).First();

                db.WarSetups.Remove(warSetupToBeDeleted);

                db.SaveChanges();
            }
        }
    }
}
