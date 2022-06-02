











using System.Collections.Generic;

namespace RTS.Models
{
    public static class MapElementData
    {
		public static List<MapElementModel> GetAllElements(){
		
			return new List<MapElementModel>{ 

				MapElementData.Random,

				MapElementData.BuildingSpotBasic,

				MapElementData.Resource,

				MapElementData.Decoration,

				MapElementData.PlayerBuildingCenter,
				
			};
		}

		public static MapElementModel GetTypeById(int pId)
		{
			switch (pId)
			{

				case 1 :
					return MapElementData.Random;

				case 2 :
					return MapElementData.BuildingSpotBasic;

				case 3 :
					return MapElementData.Resource;

				case 4 :
					return MapElementData.Decoration;

				case 5 :
					return MapElementData.PlayerBuildingCenter;

				default:
					return null;
			}
        }


		public static MapElementModel Random
        {
            get { return new MapElementModel{
                Id = 1,
                Name = "Random",
				Type = new MapElementTypeModel{
					Id = 4,
					Name = "Random",
					},
				};
            }
        }


		public static MapElementModel BuildingSpotBasic
        {
            get { return new MapElementModel{
                Id = 2,
                Name = "Building spot basic",
				Type = new MapElementTypeModel{
					Id = 1,
					Name = "Building spot",
					},
				};
            }
        }


		public static MapElementModel Resource
        {
            get { return new MapElementModel{
                Id = 3,
                Name = "Resource",
				Type = new MapElementTypeModel{
					Id = 2,
					Name = "Resource",
					},
				};
            }
        }


		public static MapElementModel Decoration
        {
            get { return new MapElementModel{
                Id = 4,
                Name = "Decoration",
				Type = new MapElementTypeModel{
					Id = 3,
					Name = "Decoration",
					},
				};
            }
        }


		public static MapElementModel PlayerBuildingCenter
        {
            get { return new MapElementModel{
                Id = 5,
                Name = "Player building center",
				Type = new MapElementTypeModel{
					Id = 5,
					Name = "Player building center",
					},
				};
            }
        }

	
		
	}
}