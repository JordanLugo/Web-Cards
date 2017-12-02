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
