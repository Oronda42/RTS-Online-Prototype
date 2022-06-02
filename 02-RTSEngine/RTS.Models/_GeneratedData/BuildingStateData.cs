











using System.Collections.Generic;

namespace RTS.Models
{
    public static class BuildingStateData
    {
		public static List<BuildingStateModel> GetAllStates(){
		
			return new List<BuildingStateModel>{ 

				BuildingStateData.Construction,

				BuildingStateData.Active,

				BuildingStateData.Paused,

				BuildingStateData.OutOfRawMaterial,

				BuildingStateData.OutOfEnergy,
				
			};
		}

		public static BuildingStateModel GetStateById(int pId)
		{
			switch (pId)
			{

				case 1 :
					return BuildingStateData.Construction;

				case 2 :
					return BuildingStateData.Active;

				case 3 :
					return BuildingStateData.Paused;

				case 4 :
					return BuildingStateData.OutOfRawMaterial;

				case 5 :
					return BuildingStateData.OutOfEnergy;

				default:
					return null;
			}
        }


		public static BuildingStateModel Construction
        {
            get { return new BuildingStateModel( 
                1,
                "Construction",
				false,
				false,
				false);
            }
        }


		public static BuildingStateModel Active
        {
            get { return new BuildingStateModel( 
                2,
                "Active",
				true,
				true,
				true);
            }
        }


		public static BuildingStateModel Paused
        {
            get { return new BuildingStateModel( 
                3,
                "Paused",
				false,
				true,
				true);
            }
        }


		public static BuildingStateModel OutOfRawMaterial
        {
            get { return new BuildingStateModel( 
                4,
                "Out of raw material",
				false,
				true,
				true);
            }
        }


		public static BuildingStateModel OutOfEnergy
        {
            get { return new BuildingStateModel( 
                5,
                "Out of energy",
				false,
				false,
				true);
            }
        }

	
		
	}
}