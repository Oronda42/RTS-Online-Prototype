











using System.Collections.Generic;

namespace RTS.Models
{
    public static class MapElementTypeData
    {
		public static List<MapElementTypeModel> GetAllTypes(){
		
			return new List<MapElementTypeModel>{ 

				MapElementTypeData.BuildingSpot,

				MapElementTypeData.Resource,

				MapElementTypeData.Decoration,

				MapElementTypeData.Random,

				MapElementTypeData.PlayerBuildingCenter,
				
			};
		}

		public static MapElementTypeModel GetTypeById(int pId)
		{
			switch (pId)
			{

				case 1 :
					return MapElementTypeData.BuildingSpot;

				case 2 :
					return MapElementTypeData.Resource;

				case 3 :
					return MapElementTypeData.Decoration;

				case 4 :
					return MapElementTypeData.Random;

				case 5 :
					return MapElementTypeData.PlayerBuildingCenter;

				default:
					return null;
			}
        }


		public static MapElementTypeModel BuildingSpot
        {
            get { return new MapElementTypeModel{
                Id = 1,
                Name = "Building spot",
				};
            }
        }


		public static MapElementTypeModel Resource
        {
            get { return new MapElementTypeModel{
                Id = 2,
                Name = "Resource",
				};
            }
        }


		public static MapElementTypeModel Decoration
        {
            get { return new MapElementTypeModel{
                Id = 3,
                Name = "Decoration",
				};
            }
        }


		public static MapElementTypeModel Random
        {
            get { return new MapElementTypeModel{
                Id = 4,
                Name = "Random",
				};
            }
        }


		public static MapElementTypeModel PlayerBuildingCenter
        {
            get { return new MapElementTypeModel{
                Id = 5,
                Name = "Player building center",
				};
            }
        }

	
		
	}
}