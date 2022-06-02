











using System.Collections.Generic;

namespace RTS.Models
{
    public static class ResourceData
    {
		public static List<ResourceModel> GetAllResources(){
		
			return new List<ResourceModel>{ 

				ResourceData.Broccoli,

				ResourceData.Minerals,

				ResourceData.Metals,

				ResourceData.AnimalFood,

				ResourceData.Preserves,

				ResourceData.BuildingMaterials,

				ResourceData.Energy,

				ResourceData.Population,
				
			};
		}

		public static ResourceModel GetResourceById(int pId)
		{
			switch (pId)
			{

				case 1 :
					return ResourceData.Broccoli;

				case 2 :
					return ResourceData.Minerals;

				case 3 :
					return ResourceData.Metals;

				case 4 :
					return ResourceData.AnimalFood;

				case 5 :
					return ResourceData.Preserves;

				case 6 :
					return ResourceData.BuildingMaterials;

				case 7 :
					return ResourceData.Energy;

				case 8 :
					return ResourceData.Population;

				default:
					return null;
			}
        }


		public static ResourceModel Broccoli
        {
            get { return new ResourceModel{
                id = 1,
                name = "Broccoli",
                AvailableInPlayerMap = true,
                type = new ResourceTypeModel {
                    id = 1,
                    name = "Common"
                    }
                };
            }
        }


		public static ResourceModel Minerals
        {
            get { return new ResourceModel{
                id = 2,
                name = "Minerals",
                AvailableInPlayerMap = true,
                type = new ResourceTypeModel {
                    id = 1,
                    name = "Common"
                    }
                };
            }
        }


		public static ResourceModel Metals
        {
            get { return new ResourceModel{
                id = 3,
                name = "Metals",
                AvailableInPlayerMap = true,
                type = new ResourceTypeModel {
                    id = 1,
                    name = "Common"
                    }
                };
            }
        }


		public static ResourceModel AnimalFood
        {
            get { return new ResourceModel{
                id = 4,
                name = "Animal food",
                AvailableInPlayerMap = false,
                type = new ResourceTypeModel {
                    id = 1,
                    name = "Common"
                    }
                };
            }
        }


		public static ResourceModel Preserves
        {
            get { return new ResourceModel{
                id = 5,
                name = "Preserves",
                AvailableInPlayerMap = false,
                type = new ResourceTypeModel {
                    id = 1,
                    name = "Common"
                    }
                };
            }
        }


		public static ResourceModel BuildingMaterials
        {
            get { return new ResourceModel{
                id = 6,
                name = "Building materials",
                AvailableInPlayerMap = false,
                type = new ResourceTypeModel {
                    id = 1,
                    name = "Common"
                    }
                };
            }
        }


		public static ResourceModel Energy
        {
            get { return new ResourceModel{
                id = 7,
                name = "Energy",
                AvailableInPlayerMap = false,
                type = new ResourceTypeModel {
                    id = 3,
                    name = "Persistent"
                    }
                };
            }
        }


		public static ResourceModel Population
        {
            get { return new ResourceModel{
                id = 8,
                name = "Population",
                AvailableInPlayerMap = false,
                type = new ResourceTypeModel {
                    id = 3,
                    name = "Persistent"
                    }
                };
            }
        }

	
		
	}
}