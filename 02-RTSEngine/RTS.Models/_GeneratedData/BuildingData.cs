











using System.Collections.Generic;

namespace RTS.Models
{
    public static class BuildingData
    {
		
		public static List<BuildingModel> GetAllBuildings(){
		
			return new List<BuildingModel>{

				BuildingData.CommandCenter,

				BuildingData.BroccoFarm,

				BuildingData.NeoGrav,

				BuildingData.MegaMine,

				BuildingData.AnifoodFactory,

				BuildingData.ConserveCompany,

				BuildingData.M3dCompany,

				BuildingData.Rbmk3,

				BuildingData.Motel,
					
			};
		}

        public static BuildingModel GetBuildingById(int pId)
        {
            switch (pId)
            {

				case 8 :
					return BuildingData.CommandCenter;

				case 1 :
					return BuildingData.BroccoFarm;

				case 2 :
					return BuildingData.NeoGrav;

				case 3 :
					return BuildingData.MegaMine;

				case 4 :
					return BuildingData.AnifoodFactory;

				case 5 :
					return BuildingData.ConserveCompany;

				case 6 :
					return BuildingData.M3dCompany;

				case 7 :
					return BuildingData.Rbmk3;

				case 9 :
					return BuildingData.Motel;

				default:
					return null;
            }
        }











		public static BuildingCenterModel CommandCenter
		{
			get { 
				return new BuildingCenterModel {
					id = 8,
					name = "Command center",
					constructionTime = 0,
					cost = new ResourceBagModel
					{
						id = 0,

						resources = new List<ResourceBagSlotModel>(),

					},
					buildingType = new BuildingTypeModel
					{
						id = 1
					},
					Levels = new List<BuildingCenterLevelModel>
					{
 
						new BuildingCenterLevelModel
						{
							persistentBag = new ResourceBagModel
							{
								id = 28,

								resources = new List<ResourceBagSlotModel>{
 
									new ResourceBagSlotModel(7,10,-1,0),			
 
									new ResourceBagSlotModel(8,10,-1,0),			

								}

							},
						},	

					}
				};
			}
		}


//TODO : Generate here ID 


		public static BuildingProducerModel BroccoFarm
			{
				get { 
					return new BuildingProducerModel {
						id = 1,
						name = "Brocco farm",
						defaultResourceIdProduced = 1,
						constructionTime = 15,
						cost = new ResourceBagModel
						{
							id = 1,
								resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,200,-1,0),			
								}
							},
						buildingType = new BuildingTypeModel
						{
							id = 2
						},
						Levels = new List<BuildingProducerLevelModel>
						{
	 
							new BuildingProducerLevelModel
							{
								id = 1,
								secondsToProduce = 300,
								amountProduced = 10,
								resourceIdProduced = 1,
								cost = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 29,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,1,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 2,
								secondsToProduce = 300,
								amountProduced = 15,
								resourceIdProduced = 1,
								cost = new ResourceBagModel
								{
									id = 2,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,600,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 30,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,1,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 3,
								secondsToProduce = 300,
								amountProduced = 20,
								resourceIdProduced = 1,
								cost = new ResourceBagModel
								{
									id = 3,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,1800,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 31,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,2,-1,0),			
										}
									},
							},	
							}
					};
				}
			}



		public static BuildingProducerModel NeoGrav
			{
				get { 
					return new BuildingProducerModel {
						id = 2,
						name = "Neo grav",
						defaultResourceIdProduced = 2,
						constructionTime = 15,
						cost = new ResourceBagModel
						{
							id = 4,
								resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,250,-1,0),			
								}
							},
						buildingType = new BuildingTypeModel
						{
							id = 2
						},
						Levels = new List<BuildingProducerLevelModel>
						{
	 
							new BuildingProducerLevelModel
							{
								id = 1,
								secondsToProduce = 300,
								amountProduced = 10,
								resourceIdProduced = 2,
								cost = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 32,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,3,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 2,
								secondsToProduce = 300,
								amountProduced = 15,
								resourceIdProduced = 2,
								cost = new ResourceBagModel
								{
									id = 5,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,750,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 33,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,3,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 3,
								secondsToProduce = 300,
								amountProduced = 20,
								resourceIdProduced = 2,
								cost = new ResourceBagModel
								{
									id = 6,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,2100,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 34,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,4,-1,0),			
										}
									},
							},	
							}
					};
				}
			}



		public static BuildingProducerModel MegaMine
			{
				get { 
					return new BuildingProducerModel {
						id = 3,
						name = "Mega mine",
						defaultResourceIdProduced = 3,
						constructionTime = 15,
						cost = new ResourceBagModel
						{
							id = 7,
								resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,300,-1,0),			
								}
							},
						buildingType = new BuildingTypeModel
						{
							id = 2
						},
						Levels = new List<BuildingProducerLevelModel>
						{
	 
							new BuildingProducerLevelModel
							{
								id = 1,
								secondsToProduce = 300,
								amountProduced = 30,
								resourceIdProduced = 3,
								cost = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 35,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,3,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 2,
								secondsToProduce = 300,
								amountProduced = 40,
								resourceIdProduced = 3,
								cost = new ResourceBagModel
								{
									id = 8,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,900,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 36,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,3,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 3,
								secondsToProduce = 300,
								amountProduced = 50,
								resourceIdProduced = 3,
								cost = new ResourceBagModel
								{
									id = 9,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,2700,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 37,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,4,-1,0),			
										}
									},
							},	
							}
					};
				}
			}



		public static BuildingProducerModel AnifoodFactory
			{
				get { 
					return new BuildingProducerModel {
						id = 4,
						name = "AniFood factory",
						defaultResourceIdProduced = 4,
						constructionTime = 60,
						cost = new ResourceBagModel
						{
							id = 10,
								resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,200,-1,0),			
								}
							},
						buildingType = new BuildingTypeModel
						{
							id = 2
						},
						Levels = new List<BuildingProducerLevelModel>
						{
	 
							new BuildingProducerLevelModel
							{
								id = 1,
								secondsToProduce = 300,
								amountProduced = 10,
								resourceIdProduced = 4,
								cost = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								consumptionBag = new ResourceBagModel
								{
									id = 47,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(1,20,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 38,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,2,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 2,
								secondsToProduce = 300,
								amountProduced = 15,
								resourceIdProduced = 4,
								cost = new ResourceBagModel
								{
									id = 11,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,600,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 48,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(1,30,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 39,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,2,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 3,
								secondsToProduce = 300,
								amountProduced = 20,
								resourceIdProduced = 4,
								cost = new ResourceBagModel
								{
									id = 12,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,1800,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 49,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(1,40,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 40,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,3,-1,0),			
										}
									},
							},	
							}
					};
				}
			}



		public static BuildingProducerModel ConserveCompany
			{
				get { 
					return new BuildingProducerModel {
						id = 5,
						name = "Conserve company",
						defaultResourceIdProduced = 5,
						constructionTime = 120,
						cost = new ResourceBagModel
						{
							id = 13,
								resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,300,-1,0),			
								}
							},
						buildingType = new BuildingTypeModel
						{
							id = 2
						},
						Levels = new List<BuildingProducerLevelModel>
						{
	 
							new BuildingProducerLevelModel
							{
								id = 1,
								secondsToProduce = 300,
								amountProduced = 10,
								resourceIdProduced = 5,
								cost = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								consumptionBag = new ResourceBagModel
								{
									id = 50,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(3,10,-1,0),			
	 
										new ResourceBagSlotModel(4,10,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 41,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,2,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 2,
								secondsToProduce = 300,
								amountProduced = 15,
								resourceIdProduced = 5,
								cost = new ResourceBagModel
								{
									id = 14,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,900,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 51,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(3,15,-1,0),			
	 
										new ResourceBagSlotModel(4,15,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 42,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,2,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 3,
								secondsToProduce = 300,
								amountProduced = 20,
								resourceIdProduced = 5,
								cost = new ResourceBagModel
								{
									id = 15,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,2700,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 52,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(3,20,-1,0),			
	 
										new ResourceBagSlotModel(4,20,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 43,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,3,-1,0),			
										}
									},
							},	
							}
					};
				}
			}



		public static BuildingProducerModel M3dCompany
			{
				get { 
					return new BuildingProducerModel {
						id = 6,
						name = "M3D company",
						defaultResourceIdProduced = 6,
						constructionTime = 300,
						cost = new ResourceBagModel
						{
							id = 16,
								resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,300,-1,0),			
								}
							},
						buildingType = new BuildingTypeModel
						{
							id = 2
						},
						Levels = new List<BuildingProducerLevelModel>
						{
	 
							new BuildingProducerLevelModel
							{
								id = 1,
								secondsToProduce = 300,
								amountProduced = 10,
								resourceIdProduced = 6,
								cost = new ResourceBagModel
								{
									id = 0,
										resources = new List<ResourceBagSlotModel>(),
									},
								consumptionBag = new ResourceBagModel
								{
									id = 53,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(2,10,-1,0),			
	 
										new ResourceBagSlotModel(3,10,-1,0),			
	 
										new ResourceBagSlotModel(4,10,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 44,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,2,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 2,
								secondsToProduce = 300,
								amountProduced = 15,
								resourceIdProduced = 6,
								cost = new ResourceBagModel
								{
									id = 17,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,900,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 54,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(2,15,-1,0),			
	 
										new ResourceBagSlotModel(3,15,-1,0),			
	 
										new ResourceBagSlotModel(4,15,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 45,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,2,-1,0),			
										}
									},
							},	
	 
							new BuildingProducerLevelModel
							{
								id = 3,
								secondsToProduce = 300,
								amountProduced = 20,
								resourceIdProduced = 6,
								cost = new ResourceBagModel
								{
									id = 18,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,2700,-1,0),			
										}
									},
								consumptionBag = new ResourceBagModel
								{
									id = 55,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(2,20,-1,0),			
	 
										new ResourceBagSlotModel(3,20,-1,0),			
	 
										new ResourceBagSlotModel(4,20,-1,0),			
										}
									},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 46,
										resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,3,-1,0),			
										}
									},
							},	
							}
					};
				}
			}













		public static BuildingPassiveModel Rbmk3
			{
				get { 
					return new BuildingPassiveModel {
						id = 7,
						name = "RBMK3",
						constructionTime = 300,
						cost = new ResourceBagModel
						{
							id = 19,
	
							resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,1500,-1,0),			
	
							}
	
						},
						buildingType = new BuildingTypeModel
						{
							id = 3
						},
						Levels = new List<BuildingPassiveLevelModel>
						{
	 
							new BuildingPassiveLevelModel
							{
								id = 1,
								secondsToConsume = 300,
								cost = new ResourceBagModel
								{
									id = 0,
	
									resources = new List<ResourceBagSlotModel>(),
	
								},
								consumptionBag = new ResourceBagModel
								{
									id = 24,
	
									resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(3,10,-1,0),			
	
									}
	
								},
								persistentBag = new ResourceBagModel
								{
									id = 22,
	
									resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,10,-1,0),			
	
									}
	
								},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 0,
	
									resources = new List<ResourceBagSlotModel>(),
	
								},
							},	
	 
							new BuildingPassiveLevelModel
							{
								id = 2,
								secondsToConsume = 300,
								cost = new ResourceBagModel
								{
									id = 20,
	
									resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(6,3000,-1,0),			
	
									}
	
								},
								consumptionBag = new ResourceBagModel
								{
									id = 25,
	
									resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(3,15,-1,0),			
	
									}
	
								},
								persistentBag = new ResourceBagModel
								{
									id = 23,
	
									resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,20,-1,0),			
	
									}
	
								},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 0,
	
									resources = new List<ResourceBagSlotModel>(),
	
								},
							},	
	
						}
					};
				}
			}


		public static BuildingPassiveModel Motel
			{
				get { 
					return new BuildingPassiveModel {
						id = 9,
						name = "Motel",
						constructionTime = 21600,
						cost = new ResourceBagModel
						{
							id = 21,
	
							resources = new List<ResourceBagSlotModel>{
	 
								new ResourceBagSlotModel(6,2500,-1,0),			
	
							}
	
						},
						buildingType = new BuildingTypeModel
						{
							id = 3
						},
						Levels = new List<BuildingPassiveLevelModel>
						{
	 
							new BuildingPassiveLevelModel
							{
								id = 1,
								secondsToConsume = 300,
								cost = new ResourceBagModel
								{
									id = 0,
	
									resources = new List<ResourceBagSlotModel>(),
	
								},
								consumptionBag = new ResourceBagModel
								{
									id = 0,
	
									resources = new List<ResourceBagSlotModel>(),
	
								},
								persistentBag = new ResourceBagModel
								{
									id = 26,
	
									resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(8,5,-1,0),			
	
									}
	
								},
								persistentBagNeeded = new ResourceBagModel
								{
									id = 27,
	
									resources = new List<ResourceBagSlotModel>{
	 
										new ResourceBagSlotModel(7,1,-1,0),			
	
									}
	
								},
							},	
	
						}
					};
				}
			}


	}

}
 