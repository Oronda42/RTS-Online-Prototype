using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapElementBuildingSpot))]
public class MapElementBuildingSpotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    void OnDisable()
    {
        MapElementBuildingSpot myTarget = (MapElementBuildingSpot)target;
        myTarget.MapExtentElementModel.Element = myTarget.MapElementScriptable.MapElementModel;
        myTarget.name = "BuildingSpot_[" + StringToVector.Vec3ToStr(myTarget.transform.position) + "]";
    }

    void OnEnable()
    {

    }
}