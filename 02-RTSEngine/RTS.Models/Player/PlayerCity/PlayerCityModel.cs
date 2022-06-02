using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    public class PlayerCityModel
    {
        public int playerId;

        public string position;

        public PlayerCityLevelModel level;

        public static PlayerCityModel CreateDefault(DateTime now, PlayerModel playerToCreate, List<PlayerCityModel> pNeighboors, int pRadius)
        {
            PlayerCityModel playerCityModel = new PlayerCityModel();

            //Set values
            playerCityModel.playerId = playerToCreate.id;
            playerCityModel.position = CreateNewPosition(pNeighboors, pRadius);
            playerCityModel.level = new PlayerCityLevelModel { id = 1 };

            //playerCenter.Init(pTime, pPlayer);
            return playerCityModel;
        }
        private static string CreateNewPosition(List<PlayerCityModel> pNeighboors, int pRadius)
        {
            string pos = "0,0";
            if (pNeighboors.Count==0)
            {
                return pos;
            }
            else
            {
                /// CONVERT string pos to Int Pos
                string[] positionsXY = pNeighboors[pNeighboors.Count - 1].position.Split(',');
                int LastCityPosX = int.Parse(positionsXY[0]);
                int LastCityPosY = int.Parse(positionsXY[1]);

                // Get A random Position in A radius
                System.Random random = new System.Random();
                var angle = (random.Next(5) * Math.PI) / 2;

                // COnvert result to int 
                int x =(int)(Math.Cos(angle) * pRadius)+ LastCityPosX;
                int y = (int)(Math.Sin(angle) * pRadius)+ LastCityPosY;
               
                pos = x + "," + y;

                return pos;
            }
        }
    }
}

