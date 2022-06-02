using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapElementResource))]
public class MapElementResourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    void OnDisable()
    {
        MapElementResource myTarget = (MapElementResource)target;
        myTarget.MapExtentElementModel.Element = myTarget.MapElementScriptable.MapElementModel;
        myTarget.name = "Resource_[" + StringToVector.Vec3ToStr(myTarget.transform.position) + "]";




    }

    void OnEnable()
    {

    }
}