using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapElementPlayerBuildingCenter))]
public class MapElementPlayerBuildingCenterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    void OnDisable()
    {
        MapElementPlayerBuildingCenter myTarget = (MapElementPlayerBuildingCenter)target;
        myTarget.MapExtentElementModel.Element = myTarget.MapElementScriptable.MapElementModel;
        myTarget.name = "PlayerBuildingCenter_[" + StringToVector.Vec3ToStr(myTarget.transform.position) + "]";
    }

    void OnEnable()
    {

    }
}