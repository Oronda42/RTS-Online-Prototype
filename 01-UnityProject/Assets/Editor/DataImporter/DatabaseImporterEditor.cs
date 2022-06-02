using RTS.Configuration;
using RTS.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DatabaseImporterEditor : Editor
{

    #region Properties

    public static string dataFilesPath = Application.dataPath + "/Data/RTS.Database.Extraction";

    #endregion

    [MenuItem("RTS/Import Database Files")]
    public static void ImportDatabaseFiles()
    {
        ImportResources();
        ImportBuildingState();
        ImportBuildingProducer();
        ImportBuildingPassive();
        ImportBuildingCenter();
        ImportMapElements();

        //Save all modified assets
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Create or update Resource scriptable object
    /// </summary>
    private static void ImportResources()
    {
        Debug.Log("Reading resources files at " + dataFilesPath);
        string assetPath = "Assets/Gameplay/Resources";
                
        foreach (ResourceModel resource in ResourceData.GetAllResources())
        {
            //Read the resource
            Resource resourceSO = null;

            switch (resource.type.id)
            {
                case 1:
                    resourceSO = (Resource)AssetDatabase.LoadAssetAtPath(assetPath + "/Consumables/" + resource.name + ".asset", typeof(Resource));
                    break;
                case 2:
                    resourceSO = (Resource)AssetDatabase.LoadAssetAtPath(assetPath + "/Consumables/" + resource.name + ".asset", typeof(Resource));
                    break;
                case 3:
                    resourceSO = (Resource)AssetDatabase.LoadAssetAtPath(assetPath + "/Persistents/" + resource.name + ".asset", typeof(Resource));
                    break;
                default:
                    break;
            }

            if (resourceSO == null)
            {
                //Create object
                resourceSO = ScriptableObject.CreateInstance<Resource>();

                //Create asset and get reference
                switch (resource.type.id)
                {
                    case 1:
                        AssetDatabase.CreateAsset(resourceSO, assetPath + "/Consumables/" + resource.name + ".asset");
                        resourceSO = (Resource)AssetDatabase.LoadAssetAtPath(assetPath + "/Consumables/" + resource.name + ".asset", typeof(Resource));
                        break;
                    case 2:
                        AssetDatabase.CreateAsset(resourceSO, assetPath + "/Consumables/" + resource.name + ".asset");
                        resourceSO = (Resource)AssetDatabase.LoadAssetAtPath(assetPath + "/Consumables/" + resource.name + ".asset", typeof(Resource));
                        break;
                    case 3:
                        AssetDatabase.CreateAsset(resourceSO, assetPath + "/Persistents/" + resource.name + ".asset");
                        resourceSO = (Resource)AssetDatabase.LoadAssetAtPath(assetPath + "/Persistents/" + resource.name + ".asset", typeof(Resource));
                        break;
                    default:
                        break;
                }


                Debug.Log("WARNING : a new resource has been created : " + resource.name + ". Type : " + resource.type.name);


            }

            //Update model and set dirty
            resourceSO.model = resource;
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(resourceSO)).SetAssetBundleNameAndVariant(Constants.Bundles.RESOURCES, "");
            EditorUtility.SetDirty(resourceSO);
        }

        Debug.Log("Resources updated successfully");

    }


    #region Buildings

    /// <summary>
    /// Create or update BuildingState scriptable object
    /// </summary>
    private static void ImportBuildingState()
    {
        Debug.Log("Reading building states files at " + dataFilesPath);
        string assetPath = "Assets/Gameplay/Buildings/State";

        foreach (BuildingStateModel buildingState in BuildingStateData.GetAllStates())
        {

            string buildingStateAssetPath = assetPath + "/" + buildingState.name + ".asset";
            BuildingState buildingStateSO = (BuildingState)AssetDatabase.LoadAssetAtPath(buildingStateAssetPath, typeof(BuildingState));
            
            if (buildingStateSO == null)
            {
                //Create object
                buildingStateSO = ScriptableObject.CreateInstance<BuildingState>();

                AssetDatabase.CreateAsset(buildingStateSO, buildingStateAssetPath);
                buildingStateSO = (BuildingState)AssetDatabase.LoadAssetAtPath(buildingStateAssetPath, typeof(BuildingState));
                Debug.Log("WARNING : a new building state has been created : " + buildingState.name + ". Id : " + buildingState.id);
            }

            //Update model and set dirty
            buildingStateSO.model = buildingState;
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(buildingStateSO)).SetAssetBundleNameAndVariant(Constants.Bundles.BUILDING_STATE, "");
            EditorUtility.SetDirty(buildingStateSO);
        }
    }

    /// <summary>
    /// Create or update Buildings producer scriptable object
    /// </summary>
    private static void ImportBuildingProducer()
    {
        Debug.Log("Reading building producer files at " + dataFilesPath);
        string assetPath = "Assets/Gameplay/Buildings/Producer";


        foreach (BuildingProducerModel buildingProducer in BuildingData.GetAllBuildings().Where(b => b.buildingType.id == (int)TypeOfBuilding.BUILDING_PRODUCER))
        {
            //Read the building producer
            string buildingProducerAssetPath = assetPath + "/" + buildingProducer.name + "/" + buildingProducer.name + ".asset";

            //Test if directory exists (Means that the scriptable should not exists)
            if (!AssetDatabase.IsValidFolder(assetPath + "/" + buildingProducer.name))
            {

                AssetDatabase.CreateFolder(assetPath, buildingProducer.name);
                AssetDatabase.Refresh();
                Debug.Log("New Building producer directory created : " + buildingProducer.name);
            }


            BuildingProducer buildingProducerSO = (BuildingProducer)AssetDatabase.LoadAssetAtPath(buildingProducerAssetPath, typeof(BuildingProducer));

            if (buildingProducerSO == null)
            {
                //Create object
                buildingProducerSO = ScriptableObject.CreateInstance<BuildingProducer>();

                AssetDatabase.CreateAsset(buildingProducerSO, buildingProducerAssetPath);
                buildingProducerSO = (BuildingProducer)AssetDatabase.LoadAssetAtPath(buildingProducerAssetPath, typeof(BuildingProducer));
                Debug.Log("WARNING : a new building producer has been created : " + buildingProducer.name + ". Id : " + buildingProducer.id);
            }


            //Update model and set dirty
            buildingProducerSO.Model = buildingProducer;
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(buildingProducerSO)).SetAssetBundleNameAndVariant(Constants.Bundles.BUILDING_PRODUCER, "");
            EditorUtility.SetDirty(buildingProducerSO);
        }
    }

    /// <summary>
    /// Create or update Buildings passive scriptable object
    /// </summary>
    private static void ImportBuildingPassive()
    {
        Debug.Log("Reading building passive files at " + dataFilesPath);
        string assetPath = "Assets/Gameplay/Buildings/Passive";

        foreach (BuildingPassiveModel buildingPassive in BuildingData.GetAllBuildings().Where(b => b.buildingType.id == (int)TypeOfBuilding.BUILDING_PASSIVE))
        {
            //Read the building producer
            string buildingPassiveAssetPath = assetPath + "/" + buildingPassive.name + "/" + buildingPassive.name + ".asset";

            //Test if directory exists (Means that the scriptable should not exists)
            if (!AssetDatabase.IsValidFolder(assetPath + "/" + buildingPassive.name))
            {

                AssetDatabase.CreateFolder(assetPath, buildingPassive.name);
                AssetDatabase.Refresh();
                Debug.Log("New Building passive directory created : " + buildingPassive.name);
            }


            BuildingPassive buildingPassiveSO = (BuildingPassive)AssetDatabase.LoadAssetAtPath(buildingPassiveAssetPath, typeof(BuildingPassive));

            if (buildingPassiveSO == null)
            {
                //Create object
                buildingPassiveSO = ScriptableObject.CreateInstance<BuildingPassive>();

                AssetDatabase.CreateAsset(buildingPassiveSO, buildingPassiveAssetPath);
                Debug.Log("WARNING : a new building passive has been created : " + buildingPassive.name + ". Id : " + buildingPassive.id);
            }


            //Update model and set dirty
            buildingPassiveSO.Model = buildingPassive;
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(buildingPassiveSO)).SetAssetBundleNameAndVariant(Constants.Bundles.BUILDING_PASSIVE, "");
            EditorUtility.SetDirty(buildingPassiveSO);
        }
    }


    /// <summary>
    /// Create or update Buildings passive scriptable object
    /// </summary>
    private static void ImportBuildingCenter()
    {
        Debug.Log("Reading building center files at " + dataFilesPath);
        string assetPath = "Assets/Gameplay/Buildings/Center";

        foreach (BuildingCenterModel buildingCenter in BuildingData.GetAllBuildings().Where(b => b.buildingType.id == (int)TypeOfBuilding.BUILDING_CENTER))
        {
            //Read the building producer
            string buildingCenterAssetPath = assetPath + "/" + buildingCenter.name + "/" + buildingCenter.name + ".asset";

            //Test if directory exists (Means that the scriptable should not exists)
            if (!AssetDatabase.IsValidFolder(assetPath + "/" + buildingCenter.name))
            {

                AssetDatabase.CreateFolder(assetPath, buildingCenter.name);
                AssetDatabase.Refresh();
                Debug.Log("New Building center directory created : " + buildingCenter.name);
            }


            BuildingCenter buildingCenterSO = (BuildingCenter)AssetDatabase.LoadAssetAtPath(buildingCenterAssetPath, typeof(BuildingCenter));

            if (buildingCenterSO == null)
            {
                //Create object
                buildingCenterSO = ScriptableObject.CreateInstance<BuildingCenter>();

                AssetDatabase.CreateAsset(buildingCenterSO, buildingCenterAssetPath);
                Debug.Log("WARNING : a new building center has been created : " + buildingCenter.name + ". Id : " + buildingCenter.id);
            }


            //Update model and set dirty
            buildingCenterSO.Model = buildingCenter;
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(buildingCenterSO)).SetAssetBundleNameAndVariant(Constants.Bundles.BUILDING_CENTER, "");
            EditorUtility.SetDirty(buildingCenterSO);
        }
    }

    /// <summary>
    /// Create or update Buildings passive scriptable object
    /// </summary>
    private static void ImportMapElements()
    {
        Debug.Log("Importing map elements at " + dataFilesPath);
        string assetPath = "Assets/Gameplay/Map/Scriptables/Elements";

        foreach (MapElementModel mapElement in MapElementData.GetAllElements())
        {
            //Read the building producer
            string mapElementAssetPath = assetPath + "/" + mapElement.Name + ".asset";

            //Test if directory exists (Means that the scriptable should not exists)
            if (!AssetDatabase.IsValidFolder(assetPath))
            {
                throw new System.Exception("Folder " + assetPath + " doesn't exists");
            }


            MapElementScriptable mapElementSO = (MapElementScriptable)AssetDatabase.LoadAssetAtPath(mapElementAssetPath, typeof(MapElementScriptable));

            if (mapElementSO == null)
            {
                //Create object
                mapElementSO = ScriptableObject.CreateInstance<MapElementScriptable>();

                AssetDatabase.CreateAsset(mapElementSO, mapElementAssetPath);
                //AssetImporter.GetAtPath(mapElementAssetPath).SetAssetBundleNameAndVariant(Constants.Bundles.BUILDING_PASSIVE, "");
                Debug.Log("WARNING : a new map element has been created : " + mapElement.Name + ". Id : " + mapElement.Id);
            }


            //Update model and set dirty
            mapElementSO.MapElementModel = mapElement;
            EditorUtility.SetDirty(mapElementSO);
        }
    }
    #endregion

}
