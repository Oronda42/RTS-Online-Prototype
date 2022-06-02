using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    public delegate void PlayerCityDelegate(PlayerCityManagerModel pPlayerCityManagerModel, PlayerCityModel pPlayerCityModel);

    public class PlayerCityManagerModel
    {
        public List<PlayerCityModel> neighboorsCities;

        public PlayerCityModel playerCity;

        public event PlayerCityDelegate OnCityCreated, OnCityDeleted; 

        public bool AddNeighboor(PlayerCityModel pPlayerCityModel)
        {
            //Add the player building
            neighboorsCities.Add(pPlayerCityModel);

            OnCityCreated?.Invoke(this, pPlayerCityModel);
            //SuscribeToPlayerBuildingEvents(pPlayerCityModel);

            return true;
        }

        public PlayerCityModel GetByPlayerId(int pPlayerId)
        {
            return neighboorsCities.Where(x => x.playerId == pPlayerId).FirstOrDefault();
        }

        public bool RemoveByPlayerId(int pPlayerId)
        {
            PlayerCityModel playerCity = GetByPlayerId(pPlayerId);
            if (!Equals(playerCity, null))
            {
                //UnsuscribeToPlayerBuildingEvents(playerBuilding);

                //Remove the building from the list
                neighboorsCities.Remove(playerCity);

                //Fire event
                OnCityDeleted?.Invoke(this, playerCity);

                return true;
            }
            return false;
        }


    }
}
